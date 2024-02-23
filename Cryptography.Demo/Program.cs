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
            RSADemo();
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
            int message = new Random().Next();

            encoder.GenerateKeys(out (BigInteger e, BigInteger n) publicKey, out (BigInteger e, BigInteger n) privateKey);
            BigInteger encoded = RSA.Encode(message, publicKey);
            BigInteger decoded = RSA.Decode(encoded, privateKey);

            Console.WriteLine(message.ToString());
            Console.WriteLine(encoded.ToString());
            Console.WriteLine(decoded.ToString());
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
