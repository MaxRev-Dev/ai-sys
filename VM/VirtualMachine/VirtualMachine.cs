using System;
using System.Collections.Generic;
using System.Linq;

namespace VirtualMachine
{
    public class VirtualMachine
    {
        private static int
            STACK_DEPTH = 25;

        private int _stackPointer;
        private readonly float[] _stack;

        public VirtualMachine()
        {
            _stack = new float[STACK_DEPTH];
        }

        private void Push(float arg)
        {
            _stack[_stackPointer++] = arg;
        }
        private float Pop()
        {
            return _stack[--_stackPointer];
        }

        private float Peek()
        {
            return _stack[_stackPointer - 1];
        }

        public IReadOnlyCollection<float> Stack => _stack;
        public int Pointer => _stackPointer;
        public Err Error { get; private set; } = Err.NONE;

        public float STM(Op[] program, IEnumerable<float> args)
        {
            Error = 0;
            _stackPointer = 0;
            int pc = 0;
            float a, b;

            foreach (var t in args.Reverse()) Push(t);

            while (pc < program.Length)
            {
                switch (program[pc++])
                {
                    case Op.DUP:
                        if (AssertStackElems(1)) break;
                        if (AssertStackNotFull()) break;
                        Push(Peek());
                        break;
                    case Op.SWAP:
                        if (AssertStackElems(2)) break;
                        a = _stack[_stackPointer - 1];
                        _stack[_stackPointer - 1] = _stack[_stackPointer - 2];
                        _stack[_stackPointer - 2] = a;
                        break;
                    case Op.MUL:
                        if (AssertStackElems(2)) break;
                        a = Pop();
                        b = Pop();
                        Push(a * b);
                        break;
                    case Op.ADD:
                        if (AssertStackElems(2)) break;
                        a = Pop();
                        b = Pop();
                        Push(a + b);
                        break;
                    case Op.DIV:
                        if (AssertStackElems(2)) break;
                        if (AssertDivBy0()) break;
                        a = Pop();
                        b = Pop();
                        Push(b / a);
                        break;
                    case Op.OVER:
                        if (AssertStackElems(2)) break;
                        Push(_stack[_stackPointer - 2]);
                        break;
                    case Op.NOP:
                        break;
                    case Op.MAX_INSTRUCTION:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            return _stack[_stackPointer - 1];
        }

        private bool AssertDivBy0()
        {
            if (_stack[_stackPointer - 1] == 0)
                Error = Err.MATH_VIOLATION;
            return (int)Error != 0;
        }

        private bool AssertStackElems(int i)
        {
            if (_stackPointer < i)
                Error = Err.STACK_VIOLATION;
            return (int)Error != 0;
        }

        private bool AssertStackNotFull()
        {
            if (_stackPointer == STACK_DEPTH)
                Error = Err.STACK_VIOLATION;
            return (int)Error > 0;
        }
    }
}