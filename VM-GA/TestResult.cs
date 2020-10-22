using VM_GA.Abstractions;

namespace VM_GA
{
    public class TestResult : ITestResult
    {
        public Chromosome Chromosome { get; }
        public int World { get; }
        public int Generation { get; }

        public TestResult(Chromosome chromosome, in int world, in int generation)
        {
            Chromosome = chromosome;
            World = world;
            Generation = generation;
        }

        public void Passed()
        {
            PassedTests++;
        }

        public int PassedTests { get; private set; }
    }
}