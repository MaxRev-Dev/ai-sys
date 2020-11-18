using AISysML.Model;
using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Vision;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

namespace ObjectDetectionWeb.Data
{
    public class ModelBuilder
    {
        private readonly LogQueue _logger;

        public ModelBuilder(LogQueue logger)
        {
            _logger = logger;
        }

        public void CreateModel(string modelPath, string datasetPath,
            string testDatasetPath = default,
            CancellationToken cancellationToken = default, Action<string> onLog = default)
        {
            var mlContext = new MLContext(seed: 1);

            var trainingDataView = mlContext.Data.ShuffleRows(
                mlContext.Data.LoadFromEnumerable(LoadImagesFromDirectory(datasetPath, token: cancellationToken)));
            IDataView testDatasetView = default;
            if (testDatasetPath != default)
            {
                testDatasetView =
                    mlContext.Data.LoadFromEnumerable(
                        LoadImagesFromDirectory(testDatasetPath, token: cancellationToken));
            }

            void Log(string message)
            {
                onLog?.Invoke(message);
            }

            mlContext.Log += (s, e) => Log(e.Message);

            // Build training pipeline
            var dataTransformPipeline = DataTransformPipeline(mlContext);

            var trainingPipeline = BuildTrainPipeline(mlContext, dataTransformPipeline);

            // Train Model
            var mlModel = TrainModel(trainingDataView, trainingPipeline);

            // Evaluate quality of Model
            EvaluateModel(mlContext, trainingDataView, trainingPipeline, mlModel, testDatasetView);

            // Save model
            SaveModel(mlContext, mlModel, modelPath, trainingDataView.Schema);
        }

        private IEstimator<ITransformer> BuildTrainPipeline(MLContext mlContext, IEstimator<ITransformer> dataPipeline)
        {
            // Set the training algorithm  
            var trainer = mlContext.MulticlassClassification.Trainers.ImageClassification(
                    new ImageClassificationTrainer.Options
                    {
                        LabelColumnName = "LabelAsKey",
                        FeatureColumnName = "Image",
                        MetricsCallback = e => _logger.Enqueue(e.ToString()),
                        Epoch = 500,
                    })
                .Append(mlContext.Transforms.Conversion
                    .MapKeyToValue("PredictedLabel", "PredictedLabel"));
            var trainingPipeline = dataPipeline.Append(trainer);
            return trainingPipeline;
        }

        public IEstimator<ITransformer> DataTransformPipeline(MLContext mlContext)
        {
            // Data process configuration with pipeline data transformations 
            var dataProcessPipeline = mlContext.Transforms.Conversion
                    .MapValueToKey("LabelAsKey", "Label");
            return dataProcessPipeline;
        }

        public ITransformer TrainModel(
            IDataView trainingDataView,
            IEstimator<ITransformer> trainingPipeline)
        {
            _logger.Enqueue("=============== Training  model ===============");

            var model = trainingPipeline.Fit(trainingDataView);

            _logger.Enqueue("=============== End of training process ===============");
            return model;
        }

        // Evaluate the trained model on the passed test dataset.
        private void EvaluateModel(MLContext mlContext, IDataView trainingDataView,
            IEstimator<ITransformer> trainingPipeline, ITransformer trainedModel,
            IDataView testDataset = default)
        {
            _logger.Enqueue("Making bulk predictions and evaluating model's quality...");
            if (testDataset != default)
            {
                var predictions = trainedModel.Transform(testDataset);
                var metrics =
                    mlContext.MulticlassClassification.Evaluate(predictions, labelColumnName: "LabelAsKey");

                _logger.Enqueue($"Micro-accuracy: {metrics.MicroAccuracy}");
                _logger.Enqueue($"Macro-accuracy: {metrics.MacroAccuracy}");
                _logger.Enqueue(
                    $"Log loss reduction: {metrics.LogLossReduction}");
            }
            else
            {
                _logger.Enqueue("=============== Cross-validating to get model's accuracy metrics ===============");
                var crossValidationResults = mlContext.MulticlassClassification.CrossValidate(trainingDataView,
                    trainingPipeline, numberOfFolds: 5, labelColumnName: "LabelAsKey");
                PrintMulticlassClassificationFoldsAverageMetrics(crossValidationResults);
            }

            _logger.Enqueue("Predicting and Evaluation complete.");
        }

        private void SaveModel(MLContext mlContext, ITransformer mlModel, string modelRelativePath, DataViewSchema modelInputSchema)
        {
            // Save/persist the trained model to a .ZIP file
            _logger.Enqueue($"=============== Saving the model  ===============");
            var file = new FileInfo(GetAbsolutePath(modelRelativePath));
            file.Directory!.Create();
            mlContext.Model.Save(mlModel, modelInputSchema, file.FullName);
            _logger.Enqueue($"The model is saved to {file.FullName}");
        }

        public string GetAbsolutePath(string relativePath)
        {
            var _dataRoot = new FileInfo(typeof(Program).Assembly.Location);
            var assemblyFolderPath = _dataRoot.Directory!.FullName;

            var fullPath = Path.Combine(assemblyFolderPath, relativePath);

            return fullPath;
        }

        public void PrintMulticlassClassificationMetrics(MulticlassClassificationMetrics metrics)
        {
            _logger.Enqueue($"************************************************************");
            _logger.Enqueue($"*    Metrics for multi-class classification model   ");
            _logger.Enqueue($"*-----------------------------------------------------------");
            _logger.Enqueue($"    MacroAccuracy = {metrics.MacroAccuracy:0.####}, a value between 0 and 1, the closer to 1, the better");
            _logger.Enqueue($"    MicroAccuracy = {metrics.MicroAccuracy:0.####}, a value between 0 and 1, the closer to 1, the better");
            _logger.Enqueue($"    LogLoss = {metrics.LogLoss:0.####}, the closer to 0, the better");
            for (var i = 0; i < metrics.PerClassLogLoss.Count; i++)
            {
                _logger.Enqueue($"    LogLoss for class {i + 1} = {metrics.PerClassLogLoss[i]:0.####}, the closer to 0, the better");
            }
            _logger.Enqueue($"************************************************************");
        }

        public void PrintMulticlassClassificationFoldsAverageMetrics(IEnumerable<TrainCatalogBase.CrossValidationResult<MulticlassClassificationMetrics>> crossValResults)
        {
            var metricsInMultipleFolds = crossValResults.Select(r => r.Metrics).ToArray();

            var microAccuracyValues = metricsInMultipleFolds.Select(m => m.MicroAccuracy).ToArray();
            var microAccuracyAverage = microAccuracyValues.Average();
            var microAccuraciesStdDeviation = CalculateStandardDeviation(microAccuracyValues);
            var microAccuraciesConfidenceInterval95 = CalculateConfidenceInterval95(microAccuracyValues);

            var macroAccuracyValues = metricsInMultipleFolds.Select(m => m.MacroAccuracy).ToArray();
            var macroAccuracyAverage = macroAccuracyValues.Average();
            var macroAccuraciesStdDeviation = CalculateStandardDeviation(macroAccuracyValues);
            var macroAccuraciesConfidenceInterval95 = CalculateConfidenceInterval95(macroAccuracyValues);

            var logLossValues = metricsInMultipleFolds.Select(m => m.LogLoss).ToArray();
            var logLossAverage = logLossValues.Average();
            var logLossStdDeviation = CalculateStandardDeviation(logLossValues);
            var logLossConfidenceInterval95 = CalculateConfidenceInterval95(logLossValues);

            var logLossReductionValues = metricsInMultipleFolds.Select(m => m.LogLossReduction).ToArray();
            var logLossReductionAverage = logLossReductionValues.Average();
            var logLossReductionStdDeviation = CalculateStandardDeviation(logLossReductionValues);
            var logLossReductionConfidenceInterval95 = CalculateConfidenceInterval95(logLossReductionValues);

            _logger.Enqueue($"*************************************************************************************************************");
            _logger.Enqueue($"*       Metrics for Multi-class Classification model      ");
            _logger.Enqueue($"*------------------------------------------------------------------------------------------------------------");
            _logger.Enqueue($"*       Average MicroAccuracy:    {microAccuracyAverage:0.###}  - Standard deviation: ({microAccuraciesStdDeviation:#.###})  - Confidence Interval 95%: ({microAccuraciesConfidenceInterval95:#.###})");
            _logger.Enqueue($"*       Average MacroAccuracy:    {macroAccuracyAverage:0.###}  - Standard deviation: ({macroAccuraciesStdDeviation:#.###})  - Confidence Interval 95%: ({macroAccuraciesConfidenceInterval95:#.###})");
            _logger.Enqueue($"*       Average LogLoss:          {logLossAverage:#.###}  - Standard deviation: ({logLossStdDeviation:#.###})  - Confidence Interval 95%: ({logLossConfidenceInterval95:#.###})");
            _logger.Enqueue($"*       Average LogLossReduction: {logLossReductionAverage:#.###}  - Standard deviation: ({logLossReductionStdDeviation:#.###})  - Confidence Interval 95%: ({logLossReductionConfidenceInterval95:#.###})");
            _logger.Enqueue($"*************************************************************************************************************");

        }

        public double CalculateStandardDeviation(IEnumerable<double> values)
        {
            var enumerable = values as double[] ?? values.ToArray();
            var average = enumerable.Average();
            var sumOfSquaresOfDifferences = enumerable.Select(val => (val - average) * (val - average)).Sum();
            var standardDeviation = Math.Sqrt(sumOfSquaresOfDifferences / (enumerable.Count() - 1));
            return standardDeviation;
        }

        public double CalculateConfidenceInterval95(IEnumerable<double> values)
        {
            var enumerable = values as double[] ?? values.ToArray();
            var confidenceInterval95 = 1.96 * CalculateStandardDeviation(enumerable) / Math.Sqrt((enumerable.Count() - 1));
            return confidenceInterval95;
        }

        //Load the Image Data from input directory.
        public IEnumerable<ModelInput> LoadImagesFromDirectory(string folder,
            bool useFolderNameAsLabel = true,
            CancellationToken token = default)
        {
            var files = Directory.GetFiles(folder, "*",
                searchOption: SearchOption.AllDirectories);
            foreach (var file in files)
            {
                token.ThrowIfCancellationRequested();

                var ext = Path.GetExtension(file);
                if (ext != ".jpg" && ext != ".png" && ext != ".jpeg" && ext != ".gif") continue;
                var label = Path.GetFileName(file);
                if (useFolderNameAsLabel)
                    label = Directory.GetParent(file).Name;
                else
                {
                    for (var index = 0; index < label.Length; index++)
                    {
                        if (!char.IsLetter(label[index]))
                        {
                            label = label.Substring(0, index);
                            break;
                        }
                    }
                }

                yield return new ModelInput
                {
                    Image = File.ReadAllBytes(file),
                    Label = label
                };
            }
        }
    }
}
