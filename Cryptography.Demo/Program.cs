using Cryptography.Encoders.Symmetric;
using Cryptography.Encoders.Asymmetric;
using System.Collections;
using System.Text;
using System.Numerics;

namespace Cryptography.Demo
{
    internal class Program
    {
        static void Main()
        {
            //VigenereCipherDemo();
            //DESDemo();
            //RSADemo();
            AESDemo();
        }

        static void VigenereCipherDemo()
        {
            string alphabet;
            alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            //alphabet = "AaBbCcDdEeFfGgHhIiJjKkLlMmNnOoPpQqRrSsTtUuVvWwXxYyZz";
            VigenereCipher encoder = new(alphabet);
            string? message, key, encoded, decoded;

            while (true)
            {
                Console.WriteLine("Alphabet: " + alphabet + Environment.NewLine);
                try
                {
                    Console.Write("Enter message: ");
                    message = Console.ReadLine() ?? throw new Exception("Incorrect input.");
                    Console.WriteLine();

                    Console.Write("Enter key: ");
                    key = Console.ReadLine() ?? throw new Exception("Incorrect input.");
                    Console.WriteLine();

                    encoded = encoder.Encode(message, key);
                    decoded = encoder.Decode(encoded, key);
                    Console.WriteLine("Encoded message: " + encoded + Environment.NewLine);
                    Console.WriteLine("Decoded message: " + decoded + Environment.NewLine);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Exception: " + ex.Message + Environment.NewLine);
                }

                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
                Console.Clear();
            }
        }

        static void DESDemo()
        {
            DataEncryptionStandard DES = new();
            Random r = new();
            string messageStr, messageDecodedStr;
            BitArray messageBits, key, encodedBits, decodedBits;
            while (true)
            {

                try
                {
                    Console.WriteLine("Enter message: ");
                    messageStr = Console.ReadLine() ?? throw new Exception("Incorrect input.");
                    Console.WriteLine();

                    key = new(56, false);
                    for (int i = 0; i < 56; i++)
                        key[i] = r.Next(10000) >= 5000;

                    Console.WriteLine("Generated binary key:");
                    DemoUtils.PrintBitArray(key);
                    Console.WriteLine();
                    Console.WriteLine();

                    messageBits = DemoUtils.StringToBitArray(messageStr);
                    encodedBits = DES.Encode(messageBits, key);
                    decodedBits = DES.Decode(encodedBits, key);
                    messageDecodedStr = DemoUtils.BitArrayToString(decodedBits);

                    Console.WriteLine("Message (binary form):");
                    DemoUtils.PrintBitArray(messageBits);
                    Console.WriteLine();
                    Console.WriteLine();

                    Console.WriteLine("Encoded message (binary form):");
                    DemoUtils.PrintBitArray(encodedBits);
                    Console.WriteLine();
                    Console.WriteLine();

                    Console.WriteLine("Decoded message (binary form):");
                    DemoUtils.PrintBitArray(decodedBits);
                    Console.WriteLine();
                    Console.WriteLine();

                    Console.WriteLine("Decoded message: " + Environment.NewLine + messageDecodedStr + Environment.NewLine);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Exception: " + ex.Message + Environment.NewLine);
                }

                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
                Console.Clear();
            }
        }

        static void RSADemo()
        {
            RSA encoder = new();
            Random rnd = new();
            Console.WriteLine("Generating keys...");
            encoder.GenerateKeys(out (byte[] e, byte[] n) publicKey, out (byte[] e, byte[] n) privateKey);
            Console.Clear();
            byte[] message = new byte[32];
            byte[] encoded, decoded;

            while (true)
            {
                rnd.NextBytes(message);
                encoded = RSA.Cipher(message, publicKey);
                decoded = RSA.Cipher(encoded, privateKey);

                Console.WriteLine("Key module length (bits): " + new BigInteger(publicKey.n).GetBitLength().ToString());
                Console.WriteLine();
                Console.WriteLine("Message: " + BitConverter.ToString(message) + Environment.NewLine);
                Console.WriteLine("Encoded: " + BitConverter.ToString(encoded) + Environment.NewLine);
                Console.WriteLine("Decoded: " + BitConverter.ToString(decoded) + Environment.NewLine);

                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
                Console.Clear();
            }
            
        }

        static void AESDemo()
        {
            Random r = new(1);
            string messageOriginalStr, messageDecodedStr;
            byte[] messageOriginalBytes, messageEncodedBytes, messageDecodedBytes;
            byte[] key = new byte[16];
            while (true)
            {
                try
                {
                    Console.Write("Enter message: ");
                    messageOriginalStr = Console.ReadLine() ?? throw new Exception("Incorrect input.");
                    Console.WriteLine();

                    messageOriginalBytes = DemoUtils.StringToBytes(messageOriginalStr);
                    Console.WriteLine($"Message bytes: {BitConverter.ToString(messageOriginalBytes)}");
                    Console.WriteLine();

                    r.NextBytes(key);
                    Console.WriteLine($"Generated key: {BitConverter.ToString(key)}");
                    Console.WriteLine();

                    messageEncodedBytes = AdvancedEncryptionStandard.Encrypt(messageOriginalBytes, key);
                    Console.WriteLine($"Encoded message bytes: {BitConverter.ToString(messageEncodedBytes)}");
                    Console.WriteLine();

                    messageDecodedBytes = AdvancedEncryptionStandard.Decrypt(messageEncodedBytes, key);
                    Console.WriteLine($"Decoded message bytes: {BitConverter.ToString(messageDecodedBytes)}");
                    Console.WriteLine();

                    messageDecodedStr = DemoUtils.BytesToString(messageDecodedBytes);
                    Console.WriteLine($"Decoded message: {messageDecodedStr}");
                    Console.WriteLine();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Exception: " + ex.Message + Environment.NewLine);
                }

                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
                Console.Clear();
            }
        }
    }

    static class DemoUtils
    {
        public static byte[] StringToBytes(string s)
        {
            if (s.Length == 0)
                return [];

            List<byte> byteList = new(s.Length * 2);
            for (int i = 0; i < s.Length; i++)
                byteList.AddRange(BitConverter.GetBytes(s[i]));

            return [.. byteList];
        }

        public static string BytesToString(byte[] bytes)
        {
            byte[] tmp;
            if (bytes.Length != 0 && bytes.Length % 2 != 0)
            {
                tmp = new byte[bytes.Length + 1];
                bytes.CopyTo(tmp, 0);
                tmp[^1] = 0;
                bytes = tmp;
            }
            StringBuilder sb = new();
            for (int i = 0; i < bytes.Length - 1; i += 2)
                sb.Append(BitConverter.ToChar(bytes, i));
            return sb.ToString();
        }

        public static BitArray StringToBitArray(string s) => new(StringToBytes(s));

        public static string BitArrayToString(BitArray bitArray)
        {
            if (bitArray.Length == 0)
                throw new InvalidOperationException();
            if (bitArray.Length % 8 != 0)
                throw new InvalidOperationException();

            byte[] bytes = new byte[bitArray.Length / 8];
            bitArray.CopyTo(bytes, 0);

            return BytesToString(bytes);
        }

        public static void PrintBitArray(BitArray bitArray)
        {
            for (int i = 0; i < bitArray.Length; i++)
                Console.Write(bitArray[i] ? "1" : "0");
        }
    }
}
