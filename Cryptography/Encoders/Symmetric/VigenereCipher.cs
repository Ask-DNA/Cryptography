using System.Text;

namespace Cryptography.Encoders.Symmetric
{
    public class VigenereCipher
    {
        public readonly string Alphabet;

        public VigenereCipher(string alphabet)
        {
            ArgumentException.ThrowIfNullOrEmpty(alphabet);
            if (alphabet.Length != alphabet.Distinct().Count())
                throw new ArgumentException("Duplicates not allowed.", nameof(alphabet));
            Alphabet = alphabet;
        }

        private void ValidateInput(string input, string key)
        {
            ArgumentException.ThrowIfNullOrEmpty(input, nameof(input));
            ArgumentException.ThrowIfNullOrEmpty(key, nameof(key));

            foreach (char c in input)
                if (!Alphabet.Contains(c))
                    throw new ArgumentException("Invalid character.", nameof(input));

            foreach (char c in key)
                if (!Alphabet.Contains(c))
                    throw new ArgumentException("Invalid character.", nameof(key));
        }

        public string Encode(string input, string key)
        {
            ValidateInput(input, key);

            StringBuilder result = new(input);
            key = ResizeKey(key, input.Length);

            for (int i = 0; i < result.Length; i++)
            {
                result[i] = Alphabet[(Alphabet.IndexOf(input[i]) + Alphabet.IndexOf(key[i])) % Alphabet.Length];
            }
            return result.ToString();
        }

        public string Decode(string input, string key)
        {
            ValidateInput(input, key);

            StringBuilder result = new(input);
            key = ResizeKey(key, input.Length);

            int idx;
            for (int i = 0; i < result.Length; i++)
            {
                idx = (Alphabet.IndexOf(input[i]) - Alphabet.IndexOf(key[i])) % Alphabet.Length;
                idx = (idx < 0) ? Alphabet.Length + idx : idx;
                result[i] = Alphabet[idx];
            }
            return result.ToString();
        }

        private static string ResizeKey(string key, int length)
        {
            StringBuilder result = new(length);

            if (key.Length <= length)
            {
                result.Insert(0, key, length / key.Length);
                length %= key.Length;
            }

            if (length > 0)
            {
                result.Append(key[..length]);
            }

            return result.ToString();
        }
    }
}
