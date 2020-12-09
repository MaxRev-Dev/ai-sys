using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using OpenCVKitchen.Data.Operators.Abstractions;
using OpenCVKitchen.Data.Operators.Configuration;
using OpenCvSharp;

namespace OpenCVKitchen.Data.Operators
{
    public class FaceDetectionOperator : ImageOperator
    {
        public enum ClassifierType
        {
            HAAR,
            LBP
        }

        private readonly Dictionary<ClassifierType, CascadeClassifier> _cached =
            new Dictionary<ClassifierType, CascadeClassifier>();

        private readonly IWebHostEnvironment _env;

        private readonly FileConfig _fileConfig;

        private Mat _prevFrame;

        public FaceDetectionOperator(FileConfig fileConfig,
            IWebHostEnvironment env)
        {
            _fileConfig = fileConfig;
            _env = env;
        }

        public ClassifierType Classifier { get; set; } = ClassifierType.HAAR;

        public HaarDetectionType DetectionType { get; set; } =
            HaarDetectionType.DoCannyPruning;

        public int Size { get; set; } = 40;

        public override Mat Preview(Mat frame)
        {
            if (Size % 2 != 1)
                Size++;
            if (_prevFrame != default)
            {
                var root = Path.Combine(_env.ContentRootPath, "Data", "Text");
                CascadeClassifier _currentClassifier =
                    _cached.ContainsKey(Classifier)
                        ? _cached[Classifier]
                        : _cached[Classifier] = Classifier switch
                        {
                            ClassifierType.HAAR =>
                                new CascadeClassifier(Path.Combine(root,
                                    _fileConfig.HaarCascade)),
                            ClassifierType.LBP =>
                                new CascadeClassifier(Path.Combine(root,
                                    _fileConfig.LbpCascade)),
                            _ => throw new ArgumentOutOfRangeException()
                        };
                return DetectFace(_currentClassifier, frame);
            }

            _prevFrame = frame;
            return frame;
        }

        private Mat DetectFace(CascadeClassifier cascade, Mat src)
        {
            using var gray = new Mat();
            Mat result = src.Clone();
            Cv2.CvtColor(src, gray, ColorConversionCodes.BGR2GRAY);

            var faces = cascade.DetectMultiScale(
                gray, 1.08, 4,
                DetectionType,
                new Size(Size, Size));

            foreach (Rect face in faces)
            {
                var center = new Point
                {
                    X = (int) (face.X + face.Width * 0.5),
                    Y = (int) (face.Y + face.Height * 0.5)
                };
                var axes = new Size
                {
                    Width = (int) (face.Width * 0.5),
                    Height = (int) (face.Height * 0.5)
                };
                Cv2.Ellipse(result, center, axes, 0, 0, 360,
                    new Scalar(255, 0, 255), 4);
            }

            return result;
        }
    }
}