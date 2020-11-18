using System;
using System.Collections.Generic;
using System.Linq;

namespace ObjectDetectionWeb.Data
{
    public class RpsGame
    {
        private readonly List<string> _keys = Enum.GetNames(typeof(RpsKeys)).ToList();
        private readonly RpsKeys[] _values = 
            Enum.GetValues(typeof(RpsKeys)).Cast<RpsKeys>().ToArray();

        private readonly Dictionary<RpsKeys, RpsKeys> _winMap =
            new Dictionary<RpsKeys, RpsKeys>
            {
                {RpsKeys.Paper, RpsKeys.Rock},
                {RpsKeys.Rock, RpsKeys.Scissors},
                {RpsKeys.Scissors, RpsKeys.Paper}
            };

        private readonly ObjectDetectionService _detectionService;
        private readonly Random _rand = new Random();
        private DateTimeOffset _lastEvent;
        private readonly List<RpsKeys> _samples = new List<RpsKeys>();
        public int ThresholdMax = 5;
        public float ConfidenceMin = .6f;
        public float GameRefreshIn = 5;

        public RpsGame(ObjectDetectionService detectionService)
        {
            _detectionService = detectionService;
        }

        public bool GameEnabled { get; private set; }
        public string AIMove { get; private set; }
        public string Winner { get; private set; }
        public string Status { get; private set; }
         
        public void StartGame()
        {
            GameEnabled = true;
        }

        public void StopGame()
        {
            GameEnabled = false;
        }

        public void OnHeartbeat()
        {
            if (!GameEnabled)
            {
                Cleanup();
                return;
            }

            if (!_detectionService.IsEnabled)
                return;
            var tDiff =
                DateTimeOffset.Now - _lastEvent;

            if (_samples.Count >= ThresholdMax && tDiff < TimeSpan.FromSeconds(GameRefreshIn))
            {
                Status = $"Refreshing game in {(GameRefreshIn - tDiff.TotalSeconds):F2}s";
                return;
            }

            if (_samples.Count < ThresholdMax &&
                DateTimeOffset.Now - _lastEvent < TimeSpan.FromSeconds(3))
            {
                Status = "AI thinking...";
                return;
            }

            var label = _detectionService.DetectionResult.Prediction;
            if (_detectionService.DetectionResult.Score.Max() < ConfidenceMin)
            {
                if (_samples.Count >= ThresholdMax)
                {
                    _samples.Clear();
                }

                return;
            }

            var userRaw = _keys.Where(x =>
                label.Contains(x,
                    StringComparison.InvariantCultureIgnoreCase)).ToArray();

            if (userRaw.Any())
            {
                var user = _values[_keys.IndexOf(userRaw.First())];
                if (_samples.Count < ThresholdMax)
                {
                    _samples.Add(user);
                    return;
                }
                var randMove = _values[_rand.Next(0, _keys.Count)];
                AIMove = randMove.ToString();
                if (_winMap[user] == randMove)
                {
                    Winner = "You";
                }
                else if (_winMap[randMove] == user)
                {
                    Winner = "AI";
                }
                else
                {
                    Winner = "DRAW!";
                }

                Winner += $"\nYou[{user}] {(Winner == "AI" ? "<" : "")}={(Winner == "You" ? ">" : "")} [{AIMove}]AI";
                Status = "We have winners";
                _lastEvent = DateTimeOffset.Now;
            }
            else
            {
                Cleanup();
            }
        }

        private void Cleanup()
        {
            Winner = AIMove = default;
            Status = "Sleeping";
        }
    }
}