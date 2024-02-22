using Cryptography.Encoders.Symmetric;
using System.Collections;
using System.Text;

namespace Cryptography.Demo
{
    internal class Program
    {
        static void Main()
        {
            //VigenereCipherDemo();
            DESDemo();
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
                    PrintBitArray(key);
                    Console.WriteLine();
                    Console.WriteLine();

                    messageBits = StringToBinary(messageStr);
                    encodedBits = DES.Encode(messageBits, key);
                    decodedBits = DES.Decode(encodedBits, key);
                    messageDecodedStr = BinaryToString(decodedBits);

                    Console.WriteLine("Message (binary form):");
                    PrintBitArray(messageBits);
                    Console.WriteLine();
                    Console.WriteLine();

                    Console.WriteLine("Encoded message (binary form):");
                    PrintBitArray(encodedBits);
                    Console.WriteLine();
                    Console.WriteLine();

                    Console.WriteLine("Decoded message (binary form):");
                    PrintBitArray(decodedBits);
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

            static void PrintBitArray(BitArray input)
            {
                for (int i = 0; i < input.Length; i++)
                    Console.Write(input[i] ? "1" : "0");
            }

            static BitArray StringToBinary(string input)
            {
                List<byte> byteList = new(input.Length * 2);
                for (int i = 0; i < input.Length; i++)
                    byteList.AddRange(BitConverter.GetBytes(input[i]));

                byte[] bytes = [.. byteList];
                return new(bytes);
            }

            static string BinaryToString(BitArray input)
            {
                if (input.Length == 0)
                    throw new InvalidOperationException();
                if (input.Length % 8 != 0)
                    throw new InvalidOperationException();

                byte[] bytes = new byte[input.Length / 8];
                input.CopyTo(bytes, 0);

                StringBuilder sb = new();
                for (int i = 0; i < bytes.Length - 1; i += 2)
                    sb.Append(BitConverter.ToChar(bytes, i));
                return sb.ToString();
            }
        }
    }
}
