namespace VigenereCipher
{
    internal class Program
    {
        static string UpperCaseAlphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        static string Alphabet = "AaBbCcDdEeFfGgHhIiJjKkLlMmNnOoPpQqRrSsTtUuVvWwXxYyZz";
        static void Main()
        {
            string alphabet = UpperCaseAlphabet;
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
    }
}
