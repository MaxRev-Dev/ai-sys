namespace VM_GA
{
    public interface IGAInfo
    {
        float MinFitness { get; }
        float MaxFitness { get; }
        float AverageFitness { get; }
        int Generation { get; }
        int OperatorWorld { get; }
    }
}