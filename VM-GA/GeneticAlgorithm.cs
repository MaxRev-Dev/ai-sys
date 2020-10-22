using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace VM_GA
{
    public class GeneticAlgorithm
    {
        private Random _random = new Random();
        private int _currentResultLength;
        private static readonly Exception LockEx = new InvalidOperationException("This instance is locked for changes");
        public float CrossProbability
        {
            get => _crossProbability;
            set
            {
                if (_locked)
                    throw LockEx;
                _crossProbability = value;
            }
        }

        public float MutationProbability
        {
            get => _mutationProbability;
            set {
                if (_locked)
                    throw LockEx;
                _mutationProbability = value;
            }
        }

        public int MinResultLength
        {
            get => _minResultLength;
            set {
                if (_locked)
                    throw LockEx;
                _minResultLength = value;
            }
        }

        public int MaxResultLength
        {
            get => _maxResultLength;
            set {
                if (_locked)
                    throw LockEx;
                _maxResultLength = value;
            }
        }

        public int ChromosomeCount
        {
            get => _chromosomeCount;
            set {
                if (_locked)
                    throw LockEx;
                _chromosomeCount = value;
            }
        }

        public int MaxGenerations
        {
            get => _maxGenerations;
            set {
                if (_locked)
                    throw LockEx;
                _maxGenerations = value;
            }
        }

        private GAInfo _info = new GAInfo();
        private bool _locked;
        private int _maxGenerations = 100000;
        private int _chromosomeCount = 10;
        private int _maxResultLength = 20;
        private int _minResultLength = 10;
        private float _crossProbability = 0.7f;
        private float _mutationProbability = 0.9f;
        public event Action<IGAInfo> NewGeneration = null!;
        public event Action<IGAInfo> NewLoop = null!;

        public bool Busy => _locked;

        public bool Process(CancellationToken cancellationToken)
        {
            _locked = true;
            _info = new GAInfo();
            for (int i = _info.OperatorWorld = MinResultLength; i < MaxResultLength; i++, _info.OperatorWorld++)
            {
                NewLoop?.Invoke(_info);
                _currentResultLength = i;
                if (ProcessOnce(cancellationToken))
                {
                    _locked = false;
                    return true;
                }

                _info.Crossovers = _info.Mutations = 0;
            }

            _locked = false;
            return false;
        }

        public bool ProcessOnce(CancellationToken cancellationToken)
        {
            Func<float> randArg = () => _random.Next(-30, 31);
            Func<Op> randInstruction = () => (Op)_random.Next(0, (int)Op.MAX_INSTRUCTION);
            var vm = new VirtualMachine();
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
                        if (_random.Next(0, ChromosomeCount) <= retF)
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
                for (int j = 0; j < ChromosomeCount-1; j += 2)
                {
                    var p1 = selectParent();
                    var p2 = selectParent();
                    var ch1 = j;
                    var ch2 = j + 1;
                    repro(p1, p2, ch1, ch2);
                }
            }

            Func<float[], float> ans = args => args[0] * args[0] * args[0] + args[1] * args[1] + args[2];
            Func<float[]> getRandArgs = () => new float[3].Select(x => randArg()).ToArray();

            bool fitnessCheck()
            {
                minF = 1000f;
                maxF = 0f;
                totF = 0f;
                for (var i = 0; i < curPop.Count; i++)
                {
                    var args1 = getRandArgs();
                    var result = vm.STM(curPop[i].Result, args1);
                    var ch = curPop[i];
                    if (vm.Error == Err.NONE)
                    {
                        ch.Fitness += 10f;
                        if (result - ans(args1) < 0.00001)
                        {
                            // we have candidate. let's test it
                            for (int j = 0, v = 0; j < 10; j++)
                            {
                                // preform final check
                                var args2 = getRandArgs();
                                if (Math.Abs(vm.STM(curPop[i].Result, args2) - ans(args2)) < 0.00001)
                                {
                                    ch.Fitness += 1000f;
                                    if (++v > 8)
                                    {
                                        Result = ch;
                                        return true;
                                    }
                                }
                            }
                        }
                    }

                    if (cancellationToken.IsCancellationRequested) return false;

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
                if (cancellationToken.IsCancellationRequested) return false;

                _info.Generation++;
                NewGeneration?.Invoke(_info);
            }

            return false;
        }

        public Chromosome Result { get; private set; }

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
        }
    }
}