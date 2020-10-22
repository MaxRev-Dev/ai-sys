using System;

namespace VM_GA
{
    public class Chromosome
    {
        public Chromosome(int resultLength)
        {
            Result = new Op[resultLength];
        }

        public float Fitness { get; set; }
        public Op[] Result { get; set; }

        public Chromosome InitRandom(Func<Op> next)
        {
            for (var i = 0; i < Result.Length; i++)
            {
                Result[i] = next();
            }

            return this;
        }
    }
}