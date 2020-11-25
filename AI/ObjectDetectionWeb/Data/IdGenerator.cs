using System;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace ObjectDetectionWeb.Data
{
    public static class IdGenerator
    {
        [SuppressMessage("ReSharper", "StringLiteralTypo")]
        private static readonly char[] Base62Chars =
            "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ"
                .ToCharArray();

        private static readonly Random Random = new Random();

        public static string GetBase62(int length)
        {
            if (length <= 0) throw new ArgumentOutOfRangeException(nameof(length));
            var sb = new StringBuilder(length);

            for (var i = 0; i < length; i++)
                sb.Append(Base62Chars[Random.Next(62)]);

            return sb.ToString();
        }

        public static string GetBase36(int length)
        {
            if (length <= 0) throw new ArgumentOutOfRangeException(nameof(length));
            var sb = new StringBuilder(length);

            for (var i = 0; i < length; i++)
                sb.Append(Base62Chars[Random.Next(36)]);

            return sb.ToString();
        }
    }
}