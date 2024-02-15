using System.Collections;
using System.Text;

namespace DataEncryptionStandard
{
    internal class Program
    {
        static void Main()
        {
            DataEncryptionStandardEncoder DES = new();
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
        }

        static void PrintBitArray(BitArray input)
        {
            for (int i = 0; i < input.Length; i++)
                Console.Write(input[i] ? "1" : "0");
        }

        static BitArray StringToBinary(string input)
        {
            byte[] bytes;
            BitArray result = new(BitConverter.GetBytes(input[0]));
            for (int i = 1; i < input.Length; i++)
            {
                bytes = BitConverter.GetBytes(input[i]);
                result = result.Append(new(bytes));
            }
            return result;
        }

        static string BinaryToString(BitArray input)
        {
            byte[] bytes = input.ToByteArray();
            StringBuilder sb = new();
            for (int i = 0; i < bytes.Length - 1; i += 2)
                sb.Append(BitConverter.ToChar(bytes, i));
            return sb.ToString();
        }
    }
}
