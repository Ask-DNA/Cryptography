using System.Numerics;

namespace Cryptography.Encoders.Asymmetric
{
    public class RSA(int? seed = null)
    {
        private readonly Random _random = (seed == null) ? new() : new(seed.Value);

        public void GenerateKeys(out (byte[] e, byte[] n) publicKey, out (byte[] d, byte[] n) privateKey)
        {
            GenerateKeys(out (BigInteger, BigInteger) publicKeyInt, out (BigInteger, BigInteger) privateKeyInt);
            publicKey = (publicKeyInt.Item1.ToByteArray(), publicKeyInt.Item2.ToByteArray());
            privateKey = (privateKeyInt.Item1.ToByteArray(), privateKeyInt.Item2.ToByteArray());
        }

        public void GenerateKeys(out (BigInteger e, BigInteger n) publicKey, out (BigInteger d, BigInteger n) privateKey)
        {
            BigInteger min = BigIntegerExtensions.MinPositive(32);

            BigInteger p = BigIntegerExtensions.ProbablePrime(min, _random);
            BigInteger q = BigIntegerExtensions.ProbablePrime(min, _random);
            BigInteger n = p * q;
            BigInteger euler = (p - 1) * (q - 1);
            BigInteger e = 65537;
            BigInteger d = e.ModularMultiplicativeInverse(euler);
            publicKey = (e, n);
            privateKey = (d, n);
        }

        public static byte[] Sifer(byte[] data, (byte[] exp, byte[] n) key)
        {
            (BigInteger exp, BigInteger n) = (new(key.exp), new(key.n));
            return BigInteger.ModPow(new(data), exp, n).ToByteArray();
        }

        public static BigInteger Sifer(BigInteger data, (BigInteger exp, BigInteger n) key)
        {
            return BigInteger.ModPow(data, key.exp, key.n);
        }
    }
}
