using System;
using System.Linq;
using System.Text;

namespace MaxRev.Helpers
{
    public static class HelperExtensions
    {
        private static readonly Random _rand = new Random();
        public const string LoremIpsum = @"Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.";

        public static readonly Exception VectorSizeDiffers =
            new ArgumentException("Vector size differs");
        public static readonly Exception ConfigurationError =
            new ArgumentException("You must configure algorithm before use");

        public static bool[] BitwiseAnd(this bool[] a, bool[] b)
        {
            if (a.Length != b.Length)
            {
                throw VectorSizeDiffers;
            }

            var result = new bool[a.Length];
            for (var i = 0; i < a.Length; i++)
                result[i] = a[i] & b[i];
            return result;
        }

        public static int Count(this bool[] v)
        {
            return v.Count(x => x);
        }

        public static string GetRandomText(int wordCount, int senSize = 5)
        {
            if (wordCount <= 0) throw new ArgumentOutOfRangeException(nameof(wordCount));
            var sb = new StringBuilder(wordCount);
            var l = LoremIpsum.Split(' ').Select(x => x.Trim(',', '.')).ToArray();
            var i = 0;

            while (i++ < wordCount)
            {
                if (i > 1)
                    sb.Append(' ');
                var w = l[_rand.Next(0, l.Length - 1)];
                if (i == 1 || i % senSize == 0)
                    w = char.ToUpper(w[0]) + w.Substring(1);
                sb.Append(w);
                if ((i + 1) % senSize == 0)
                    sb.Append('.');
            }

            return sb.ToString();
        }
    }
}