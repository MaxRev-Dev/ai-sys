using System.Collections.Generic;

namespace VM_GA.Abstractions
{
    public interface IGAInfo
    {
        float MinFitness { get; }
        float MaxFitness { get; }
        float AverageFitness { get; }
        int Generation { get; }
        int OperatorWorld { get; }
        int Crossovers { get; }
        int Mutations { get; }
        List<ITestResult> Results { get; }
    }
}