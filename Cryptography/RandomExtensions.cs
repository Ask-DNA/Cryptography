using System.Numerics;

namespace Cryptography
{
    internal static class RandomExtensions
    {
        public static BigInteger NextBigInteger(this Random random, BigInteger maxValue)
        {
            ArgumentOutOfRangeException.ThrowIfNegative(maxValue);

            if (maxValue <= long.MaxValue)
                return random.NextInt64((long)maxValue);

            byte[] buffer;
            int nBytes;
            BigInteger result;
            do
            {
                nBytes = random.Next(1, maxValue.GetByteCount() + 1);
                buffer = new byte[nBytes];
                random.NextBytes(buffer);
                buffer[^1] = (byte)random.Next(128);
                result = new(buffer);
            }
            while (result >= maxValue);

            return result;
        }

        public static BigInteger NextBigInteger(this Random random, BigInteger minValue, BigInteger maxValue)
        {
            if (minValue > maxValue)
                throw new ArgumentException($"'{nameof(minValue)}' must be greater thaon or equal to '{nameof(maxValue)}'.");

            if (minValue == maxValue)
                return maxValue;

            if (minValue >= long.MinValue && maxValue <= long.MaxValue)
                return random.NextInt64((long)minValue, (long)maxValue);

            return random.NextBigInteger(maxValue - minValue) + minValue;
        }
    }
}
