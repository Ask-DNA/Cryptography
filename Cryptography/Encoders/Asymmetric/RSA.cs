using System.Numerics;

namespace Cryptography.Encoders.Asymmetric
{
    public class RSA
    {
        private readonly Random _random;
        private readonly BigInteger _minP, _minQ;

        public RSA(int pMinByteLen = 32, int qMinByteLen = 32, int? seed = null)
        {
            ArgumentOutOfRangeException.ThrowIfLessThan(pMinByteLen, 2);
            ArgumentOutOfRangeException.ThrowIfLessThan(qMinByteLen, 2);
            _random = (seed == null) ? new() : new(seed.Value);
            _minP = BigIntegerExtensions.MinPositive(pMinByteLen);
            _minQ = BigIntegerExtensions.MinPositive(qMinByteLen);
        }

        public void GenerateKeys(out (byte[] e, byte[] n) publicKey, out (byte[] d, byte[] n) privateKey)
        {
            GenerateKeys(out (BigInteger, BigInteger) publicKeyInt, out (BigInteger, BigInteger) privateKeyInt);
            publicKey = (publicKeyInt.Item1.ToByteArray(), publicKeyInt.Item2.ToByteArray());
            privateKey = (privateKeyInt.Item1.ToByteArray(), privateKeyInt.Item2.ToByteArray());
        }

        public void GenerateKeys(out (BigInteger e, BigInteger n) publicKey, out (BigInteger d, BigInteger n) privateKey)
        {
            BigInteger p = BigIntegerExtensions.ProbablePrime(_minP, _random);
            BigInteger q = BigIntegerExtensions.ProbablePrime(_minQ, _random);
            BigInteger module = p * q;
            BigInteger euler = (p - 1) * (q - 1);
            BigInteger publicExponent = 65537;
            BigInteger privateExponent = publicExponent.ModularMultiplicativeInverse(euler);
            publicKey = (publicExponent, module);
            privateKey = (privateExponent, module);
        }

        public static byte[] Cipher(byte[] data, (byte[] exponent, byte[] module) key)
        {
            (BigInteger exponentInt, BigInteger moduleInt) = (new(key.exponent), new(key.module));
            BigInteger dataInt = new(data);
            if (dataInt >= moduleInt)
                throw new ArgumentException($"Integer representation of '{nameof(data)}' must be less than '{nameof(key.module)}'.");
            return BigInteger.ModPow(dataInt, exponentInt, moduleInt).ToByteArray();
        }

        public static BigInteger Cipher(BigInteger data, (BigInteger exponent, BigInteger module) key)
        {
            if (data >= key.module)
                throw new ArgumentException($"'{nameof(data)}' must be less than '{nameof(key.module)}'.");
            return BigInteger.ModPow(data, key.exponent, key.module);
        }

        public static bool TryCipher(byte[] data, (byte[] exponent, byte[] module) key, out byte[] result)
        {
            (BigInteger exponentInt, BigInteger moduleInt) = (new(key.exponent), new(key.module));
            BigInteger dataInt = new(data);
            if (dataInt >= moduleInt)
            {
                result = [];
                return false;
            }
            result = BigInteger.ModPow(dataInt, exponentInt, moduleInt).ToByteArray();
            return true;
        }

        public static bool TryCipher(BigInteger data, (BigInteger exponent, BigInteger module) key, out BigInteger result)
        {
            if (data >= key.module)
            {
                result = 0;
                return false;
            }
            result = BigInteger.ModPow(data, key.exponent, key.module);
            return true;
        }
    }
}
