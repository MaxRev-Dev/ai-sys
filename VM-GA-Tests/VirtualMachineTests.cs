using System.Linq;
using VM_GA;
using Xunit;

namespace VM_GA_Tests
{
    public class VirtualMachineTests
    {
        [Theory]
        [InlineData(
            new[] { Op.ADD },
            new[] { 1, 2 }, 3)]
        [InlineData(
            new[] { Op.DUP },
            new[] { 2 }, 2)]
        [InlineData(
            new[] { Op.DUP },
            new[] { 2, 6 }, 2)]
        [InlineData(
            new[] { Op.SWAP },
            new[] { 2, 1 }, 1)]
        [InlineData(
            new[] { Op.MUL },
            new[] { 1, 2 }, 2)]
        [InlineData(
            new[] { Op.DIV },
            new[] { 0, 2 }, 0)]
        [InlineData(
            new[] { Op.DIV },
            new[] { 2, 2 }, 1)]
        public void VmAcceptsCommands1(Op[] ops, int[] vsInts, int expected)
        {
            var vm = new VirtualMachine();
            Assert.Equal(expected, vm.STM(ops, vsInts.Select(x=>(float)x)));
        }

        [Theory]
        [InlineData(
            new[] { Op.DUP, Op.MUL, Op.DUP, Op.MUL, Op.DUP, Op.MUL },
            new[] { 2 }, 256)]
        [InlineData(
            new[] { Op.ADD, Op.MUL, Op.ADD },
            new[] { 5, 1, 3, 2 }, 20)]
        [InlineData(
            new[] { Op.OVER, Op.ADD, Op.MUL },
            new[] { 1, 3 }, 12)] // (x + y) * x
        [InlineData(
            new[] { Op.DUP, Op.DUP, Op.MUL, Op.MUL,
                Op.SWAP, Op.DUP, Op.MUL,
                Op.SWAP, Op.ADD,
                Op.SWAP, Op.SWAP, Op.ADD },
            new[] { 1, 2, 3 }, 8)]
        public void VmAcceptsCommands2(Op[] ops, int[] vsInts, int expected)
        {
            var vm = new VirtualMachine();
            Assert.Equal(expected, vm.STM(ops, vsInts.Select(x => (float)x)));
        }
    }
}
