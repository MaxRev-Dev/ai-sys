namespace VM_GA.Abstractions
{
    public interface ITestResult
    {
        Chromosome Chromosome { get; }
        int PassedTests { get; }
    }
}