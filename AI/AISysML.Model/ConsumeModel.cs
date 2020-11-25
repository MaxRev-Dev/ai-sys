using System;
using Microsoft.ML;

namespace AISysML.Model
{
    public class ConsumeModel
    {
        public static PredictionEngine<ModelInput, ModelOutput> Instance;

        public static ModelOutput Predict(string modelPath, ModelInput input)
        {
            ModelOutput result = (Instance ??= CreatePredictionEngine(modelPath)).Predict(input);
            return result;
        }

        public static PredictionEngine<ModelInput, ModelOutput> CreatePredictionEngine(string modelPath)
        {
            try
            {
                var mlContext = new MLContext();
                var mlModel = mlContext.Model.Load(modelPath, out _);
                var predEngine = Instance = mlContext.Model.CreatePredictionEngine<ModelInput, ModelOutput>(mlModel);
                return predEngine;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return default;
            }
        }
    }
}
