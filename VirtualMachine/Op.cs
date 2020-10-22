namespace VirtualMachine
{
    public enum Op
    {
        DUP,
        SWAP,
        MUL,
        ADD,
        DIV,
        OVER,
        NOP,
        MAX_INSTRUCTION = NOP + 1,
    }
}