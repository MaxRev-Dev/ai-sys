using System;
using System.Linq;

namespace VM_GA
{
    public class VirtualMachine
    {
        public enum Op
        {
            DUP = 0x00,
            SWAP = 0x01,
            MUL = 0x02,
            ADD = 0x03,
            OVER = 0x04,
            NOP = 0x05,
            MAX_INSTRUCTION = NOP + 1,
        }

        private static int
            NONE = 0,
            STACK_VIOLATION = 1,
            MATH_VIOLATION = 2,
            STACK_DEPTH = 25;

        private int stackPointer;
        private readonly int[] stack; 
        
        public VirtualMachine()
        {
            stack = new int[STACK_DEPTH];
        }

        private void Push(int arg)
        {
            stack[stackPointer++] = arg;
        }
        private int Pop()
        {
            return stack[stackPointer--];
        }

        private int Peek()
        {
            return stack[stackPointer];
        }

        public void STM(Op[] program, int[] args)
        {
            int pc = 0; 
            int a , b;

            foreach (var t in args.Reverse()) Push(t);

            while (pc < program.Length)
            {
                switch (program[pc++])
                {
                    case Op.DUP:
                        Push(Peek());
                        break;
                    case Op.SWAP:
                        a = stack[stackPointer];
                        stack[stackPointer] = stack[stackPointer - 1];
                        stack[stackPointer - 1] = a;
                        break;
                    case Op.MUL:
                        a = Pop();
                        b = Pop();
                        Push(a * b);
                        break;
                    case Op.ADD:
                        a = Pop();
                        b = Pop();
                        Push(a + b);
                        break;
                    case Op.OVER:
                        Push(stack[stackPointer - 1]);
                        break;
                    case Op.NOP:
                        break;
                    case Op.MAX_INSTRUCTION:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }
    }
}