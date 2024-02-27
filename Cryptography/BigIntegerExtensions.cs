using System.Numerics;

namespace Cryptography
{
    internal static class BigIntegerExtensions
    {
        public static BigInteger MinPositive(int nBytes)
        {
            ArgumentOutOfRangeException.ThrowIfLessThan(nBytes, 2);
            byte[] bytes = new byte[nBytes];
            for (int i = 0; i < bytes.Length - 2; i++)
                bytes[i] = 0;
            bytes[^2] = 128;
            bytes[^1] = 0;
            return new(bytes);
        }

        public static BigInteger ProbablePrime(BigInteger minValue, Random rnd)
        {
            List<int> primes = new SieveOfEratosthenes(1000000).Primes;
            BigInteger prime = primes[rnd.Next(primes.Count)];
            BigInteger candidate, randEven, randInt, d;
            bool check;

            while (prime < minValue)
            {
                check = true;

                randEven = rnd.NextBigInteger(prime, 2 * (2 * prime + 1) + 1);
                if (!randEven.IsEven)
                    randEven += 1;

                candidate = prime * randEven + 1;

                for (int i = 0; i < primes.Count; i++)
                {
                    if (candidate < primes[i])
                        break;
                    if (candidate % primes[i] == 0)
                    {
                        check = false;
                        break;
                    }
                }

                if (!check)
                    continue;

                do
                {
                    check = true;
                    randInt = rnd.NextBigInteger(2, candidate);
                    if (BigInteger.ModPow(randInt, candidate - 1, candidate) != 1)
                        check = false;
                    d = BigInteger.GreatestCommonDivisor(BigInteger.ModPow(randInt, randEven, candidate) - 1, candidate);
                    if (d != 1)
                        check = false;
                }
                while (d == candidate);

                if (!check)
                    continue;

                prime = candidate;
            }

            return prime;
        }

        public static BigInteger ModularMultiplicativeInverse(this BigInteger a, BigInteger m)
        {
            (BigInteger gcd, BigInteger x, _) = a.GreatestCommonDivisorExtended(m);
            if (gcd != 1)
                return -1;
            return (x % m + m) % m;
        }

        private static (BigInteger gcd, BigInteger x, BigInteger y) GreatestCommonDivisorExtended(this BigInteger a, BigInteger b)
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

        //public static BigInteger ProbablePrimeShort(Random rnd)
        //{
        //    byte[] buffer = new byte[6];
        //    rnd.NextBytes(buffer);
        //    BigInteger p = new(buffer);
        //    p = BigInteger.Abs(p);
        //    if (p.IsEven)
        //        p++;

        //    while (!p.IsProbablePrime())
        //        p += 2;

        //    return p;
        //}

        //private static bool IsProbablePrime(this BigInteger a)
        //{
        //    if (a == 2 || a == 3)
        //        return true;

        //    if (a <= 1 || a % 2 == 0 || a % 3 == 0)
        //        return false;

        //    for (BigInteger i = 5; i * i <= a; i += 6)
        //    {
        //        if (a % i == 0 || a % (i + 2) == 0)
        //            return false;
        //    }

        //    return true;
        //}

        //public static BigInteger FastPow(BigInteger a, BigInteger b)
        //{
        //    BitArray expBits = b.ToBitArray();
        //    expBits = expBits.DropLast();
        //    BigInteger result = 1;
        //    for (int i = expBits.Length - 1; i >= 0; i--)
        //    {
        //        result *= result;
        //        if (expBits[i])
        //            result *= a;
        //    }
        //    return result;
        //}

        //private static BitArray ToBitArray(this BigInteger a)
        //{
        //    BitArray result = new(a.ToByteArray());
        //    bool sign = result[^1];

        //    while (!result[^1] && result.Length > 0)
        //        result = result.DropLast();
        //    if (!sign)
        //        result = result.Append(new(1, false));

        //    return result;
        //}
    }
}
