using System.Numerics;
using System.Collections;

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
            BigInteger p = GetRandomPrimeNumber(4);
            BigInteger q = GetRandomPrimeNumber(4);
            BigInteger n = p * q;
            BigInteger euler = (p - 1) * (q - 1);
            BigInteger e = 65537;
            BigInteger d = e.ModularMultiplicativeInverse(euler);
            publicKey = (e, n);
            privateKey = (d, n);
        }

        public static BigInteger Encode(BigInteger data, (BigInteger e, BigInteger n) publicKey)
        {
            return data.PowMod(publicKey.e, publicKey.n); 
        }

        public static BigInteger Decode(BigInteger data, (BigInteger d, BigInteger n) privateKey)
        {
            return data.PowMod(privateKey.d, privateKey.n);
        }

        private BigInteger GetRandomPrimeNumber(int nBytes)
        {
            byte[] bytes = new byte[nBytes];

            BigInteger result = 0;
            BitArray bitArray;
            while (result == 0)
            {
                _random.NextBytes(bytes);
                bitArray = new(bytes);
                result = bitArray.ToBigInteger();
            }
            
            if (result < 0)
                result = BigInteger.Abs(result);

            if (result % 2 == 0)
                result -= 1;

            while (!result.IsPrime())
                result -= 2;

            return result;
        }
    }
}
