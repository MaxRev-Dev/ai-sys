using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using VirtualMachine;
using VM_GA.Abstractions;

namespace VM_GA
{
    public class GeneticAlgorithm
    {
        #region Fields

        private static readonly Exception LockEx = new InvalidOperationException("This instance is locked for changes");
        private static readonly Random _random = new Random();
        private int _currentResultLength;
        private GAInfo _info = new GAInfo();
        private int _maxGenerations = 100000;
        private int _chromosomeCount = 10;
        private int _maxResultLength = 20;
        private int _minResultLength = 1;
        private float _crossProbability = 0.9f;
        private float _mutationProbability = 0.05f;
        public event Action<IGAInfo> NewGeneration = null!;
        public event Action<IGAInfo> NewLoop = null!;
        private Func<Random, float[]> _getTestArgs = r => new float[2].Select(x => (float)(r ?? _random).Next(-30, 31)).ToArray();
        private Func<float[], float> _testAnswer = args => args[0] * args[0] + args[1] * args[1]; // x^2+y^2
        // args[0] * args[0] * args[0] + args[1] * args[1] + args[2]; // x^3+y^2+z

        #endregion

        #region Properties

        public Func<float[], float> TestAnswer
        {
            get => _testAnswer;
            set {
                if (Busy)
                    throw LockEx;
                _testAnswer = value;
            }
        }

        public Func<Random, float[]> GetTestArgs
        {
            get => _getTestArgs;
            set {
                if (Busy)
                    throw LockEx;
                _getTestArgs = value;
            }
        }

        public float CrossProbability
        {
            get => _crossProbability;
            set {
                if (Busy)
                    throw LockEx;
                _crossProbability = value;
            }
        }

        public float MutationProbability
        {
            get => _mutationProbability;
            set {
                if (Busy)
                    throw LockEx;
                _mutationProbability = value;
            }
        }

        public int MinResultLength
        {
            get => _minResultLength;
            set {
                if (Busy)
                    throw LockEx;
                _minResultLength = value;
            }
        }

        public int MaxResultLength
        {
            get => _maxResultLength;
            set {
                if (Busy)
                    throw LockEx;
                _maxResultLength = value;
            }
        }

        public int ChromosomeCount
        {
            get => _chromosomeCount;
            set {
                if (Busy)
                    throw LockEx;
                _chromosomeCount = value;
            }
        }

        public int MaxGenerations
        {
            get => _maxGenerations;
            set {
                if (Busy)
                    throw LockEx;
                _maxGenerations = value;
            }
        }

        public bool Busy { get; private set; }

        #endregion

        public bool Process(CancellationToken cancellationToken)
        {
            Busy = true;
            _info = new GAInfo();
            for (int i = _info.OperatorWorld = MinResultLength; i < MaxResultLength; i++, _info.OperatorWorld++)
            {
                NewLoop?.Invoke(_info);
                _currentResultLength = i;
                if (ProcessOnce(cancellationToken))
                {
                    Busy = false;
                    return true;
                }

                _info.Crossovers = _info.Mutations = 0;
            }

            Busy = false;
            return _info.Results.Any();
        }

        public bool ProcessOnce(CancellationToken cancellationToken)
        {
            Func<Op> randInstruction = () => (Op)_random.Next(0, (int)Op.MAX_INSTRUCTION);
            var vm = new VirtualMachine.VirtualMachine();
            var curPop = InitializePopulation(ChromosomeCount, randInstruction).ToList();
            var nextPop = InitializePopulation(ChromosomeCount, randInstruction).ToList();
            var generation = 0;
            float minF;
            float maxF;
            float totF;
            int selectParent()
            {
                var j = 0;
                while (true)
                {
                    if (cancellationToken.IsCancellationRequested) return 0;
                    if (j == ChromosomeCount) j = 0;
                    if (maxF == 0) return j;
                    var retF = curPop[j].Fitness / maxF;
                    if (curPop[j].Fitness >= minF)
                        if (_random.Next(0, ChromosomeCount) < retF)
                        {
                            return j;
                        }
                    j++;
                }
            }

            void repro(int p1, int p2, int ch1, int ch2)
            {
                int crossPoint, i;
                if (_random.NextDouble() <= CrossProbability)
                {
                    crossPoint = _random.Next(0, Math.Max(curPop[p1].Result.Length, curPop[p2].Result.Length));
                    _info.Crossovers++;
                }
                else
                {
                    crossPoint = _currentResultLength;
                }

                for (i = 0; i < crossPoint; i++)
                {
                    nextPop[ch1].Result[i] = mutate(curPop[p1].Result[i]);
                    nextPop[ch2].Result[i] = mutate(curPop[p2].Result[i]);
                }
                for (; i < _currentResultLength; i++)
                {
                    nextPop[ch1].Result[i] = mutate(curPop[p2].Result[i]);
                    nextPop[ch2].Result[i] = mutate(curPop[p1].Result[i]);
                }
            }

            Op mutate(Op gene)
            {
                if (_random.NextDouble() <= MutationProbability)
                {
                    gene = randInstruction();
                    _info.Mutations++;
                }

                return gene;
            }

            void selection()
            {
                for (int j = 0; j < ChromosomeCount - 1; j += 2)
                {
                    var p1 = selectParent();
                    var p2 = selectParent();
                    var ch1 = j;
                    var ch2 = j + 1;
                    repro(p1, p2, ch1, ch2);
                }
            }

            bool fitnessCheck()
            {
                minF = 1000f;
                maxF = 0f;
                totF = 0f;
                for (var i = 0; i < curPop.Count; i++)
                {
                    var args1 = GetTestArgs(_random);
                    var result = vm.STM(curPop[i].Result, args1);
                    var ch = curPop[i];
                    if (vm.Error == Err.NONE)
                    {
                        var diff = Math.Abs(result - TestAnswer(args1));
                        ch.Fitness += 1;
                        if (diff < 0.001)
                        {
                            var testResult = new TestResult(ch, _currentResultLength, generation);
                            // we have candidate. let's test it
                            for (int j = 0, v = 1; j < 10; j++)
                            {
                                // perform final check
                                var args2 = GetTestArgs(_random);
                                if (Math.Abs(vm.STM(curPop[i].Result, args2) - TestAnswer(args2)) < 0.001)
                                {
                                    testResult.Passed();
                                    ch.Fitness += 1000f;
                                    if (v++ >= 5)
                                    {
                                        if (!_info.Results.Contains(testResult))
                                            _info.AddResult(testResult);
                                    }
                                }
                            }
                        }
                    }

                    if (cancellationToken.IsCancellationRequested)
                        return _info.Results.Any();

                    if (minF > ch.Fitness)
                    {
                        minF = ch.Fitness;
                        _info.MinFitness = minF;
                    }
                    else if (maxF < ch.Fitness)
                    {
                        maxF = ch.Fitness;
                        _info.MaxFitness = maxF;
                    }

                    totF += ch.Fitness;
                    _info.AverageFitness = totF / curPop.Count;
                }

                return false;
            }

            fitnessCheck();
            while (generation++ < MaxGenerations)
            {
                selection();
                var next = curPop;
                curPop = nextPop;
                nextPop = next;
                curPop = curPop.OrderByDescending(x => x.Fitness).ToList();
                if (fitnessCheck())
                {
                    return true;
                }
                if (cancellationToken.IsCancellationRequested)
                    return _info.Results.Any();

                _info.Generation++;
                NewGeneration?.Invoke(_info);
            }

            return false;
        }

        private IEnumerable<Chromosome> InitializePopulation(int chrCount, Func<Op> randSelector)
        {
            return Enumerable.Range(0, chrCount)
                .Select(_ => new Chromosome(_currentResultLength)
                    .InitRandom(randSelector));
        }

        private class GAInfo : IGAInfo
        {
            public float MinFitness { get; set; }
            public float MaxFitness { get; set; }
            public float AverageFitness { get; set; }
            public int Generation { get; set; }
            public int OperatorWorld { get; set; }
            public int Crossovers { get; set; }
            public int Mutations { get; set; }
            public List<ITestResult> Results { get; } = new List<ITestResult>();

            public void AddResult(ITestResult result)
            {
                Results.Add(result);
            }
        }
    }
}