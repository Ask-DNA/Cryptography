using System.Numerics;

namespace Cryptography
{
    internal static class BigIntegerExtensions
    {
        public static bool IsPrime(this BigInteger a)
        {
            if (a == 2 || a == 3)
                return true;

            if (a <= 1 || a % 2 == 0 || a % 3 == 0)
                return false;

            for (BigInteger i = 5; i * i <= a; i += 6)
            {
                if (a % i == 0 || a % (i + 2) == 0)
                    return false;
            }

            return true;
        }

        public static (BigInteger gcd, BigInteger x, BigInteger y) GreatestCommonDivisorExtended(this BigInteger a, BigInteger b)
        {
            if (b == 0)
                return (a, 1, 0);
            else
            {
                (BigInteger gcd, BigInteger x1, BigInteger y1) = GreatestCommonDivisorExtended(b, a % b);
                BigInteger x = y1;
                BigInteger y = x1 - (a / b) * y1;
                return (gcd, x, y);
            }
        }

        public static BigInteger ModularMultiplicativeInverse(this BigInteger a, BigInteger m)
        {
            (BigInteger gcd, BigInteger x, _) = a.GreatestCommonDivisorExtended(m);
            if (gcd != 1)
                return -1;
            return (x % m + m) % m;
        }

        public static BigInteger PowMod(this BigInteger a, BigInteger b, BigInteger m)
        {
            if (b == 0)
                return 1;
            BigInteger temp = PowMod(a, b / 2, m);
            if (b % 2 == 0)
                return (temp * temp) % m;
            else
                return ((a % m) * ((temp * temp) % m)) % m;
        }
    }
}
