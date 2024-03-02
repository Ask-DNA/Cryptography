using System.Text;

namespace Cryptography.Encoders.Symmetric
{
    public class VigenereCipher
    {
        public readonly string Alphabet;

        public VigenereCipher(string alphabet)
        {
            if (alphabet.Length == 0)
                throw new ArgumentException($"Input string '{nameof(alphabet)}' must be non-empty.");
            if (alphabet.Length != alphabet.Distinct().Count())
                throw new ArgumentException($"Input string '{nameof(alphabet)}' must not contain duplicates.");

            Alphabet = alphabet;
        }

        public VigenereCipher(char[] alphabet)
        {
            if (alphabet.Length == 0)
                throw new ArgumentException($"Input array '{nameof(alphabet)}' must be non-empty.");
            if (alphabet.Length != alphabet.Distinct().Count())
                throw new ArgumentException($"Input array '{nameof(alphabet)}' must not contain duplicates.");

            Alphabet = new string(alphabet);
        }

        public string Encrypt(string data, string key)
        {
            if (data.Length == 0)
                throw new ArgumentException($"Input string '{nameof(data)}' must be non-empty.");
            if (key.Length == 0)
                throw new ArgumentException($"Input string '{nameof(key)}' must be non-empty.");
            foreach (char c in data)
                if (!Alphabet.Contains(c))
                    throw new ArgumentException($"Input string '{nameof(data)}' must only contain characters contained in '{nameof(Alphabet)}'.");
            foreach (char c in key)
                if (!Alphabet.Contains(c))
                    throw new ArgumentException($"Input string '{nameof(key)}' must only contain characters contained in '{nameof(Alphabet)}'.");

            StringBuilder resultBuilder = new(data);
            key = KeyExpansion(key, data.Length);

            for (int i = 0; i < resultBuilder.Length; i++)
            {
                resultBuilder[i] = Alphabet[(Alphabet.IndexOf(data[i]) + Alphabet.IndexOf(key[i])) % Alphabet.Length];
            }
            return resultBuilder.ToString();
        }

        public string Decrypt(string data, string key)
        {
            if (data.Length == 0)
                throw new ArgumentException($"Input string '{nameof(data)}' must be non-empty.");
            if (key.Length == 0)
                throw new ArgumentException($"Input string '{nameof(key)}' must be non-empty.");
            foreach (char c in data)
                if (!Alphabet.Contains(c))
                    throw new ArgumentException($"Input string '{nameof(data)}' must only contain characters contained in '{nameof(Alphabet)}'.");
            foreach (char c in key)
                if (!Alphabet.Contains(c))
                    throw new ArgumentException($"Input string '{nameof(key)}' must only contain characters contained in '{nameof(Alphabet)}'.");

            StringBuilder resultBuilder = new(data);
            key = KeyExpansion(key, data.Length);

            int idx;
            for (int i = 0; i < resultBuilder.Length; i++)
            {
                idx = (Alphabet.IndexOf(data[i]) - Alphabet.IndexOf(key[i])) % Alphabet.Length;
                idx = (idx < 0) ? Alphabet.Length + idx : idx;
                resultBuilder[i] = Alphabet[idx];
            }
            return resultBuilder.ToString();
        }

        public char[] Encrypt(char[] data, char[] key)
        {
            if (data.Length == 0)
                throw new ArgumentException($"Input array '{nameof(data)}' must be non-empty.");
            if (key.Length == 0)
                throw new ArgumentException($"Input array '{nameof(key)}' must be non-empty.");
            foreach (char c in data)
                if (!Alphabet.Contains(c))
                    throw new ArgumentException($"Input array '{nameof(data)}' must only contain characters contained in '{nameof(Alphabet)}'.");
            foreach (char c in key)
                if (!Alphabet.Contains(c))
                    throw new ArgumentException($"Input array '{nameof(key)}' must only contain characters contained in '{nameof(Alphabet)}'.");

            return Encrypt(new string(data), new string(key)).ToCharArray();
        }

        public char[] Decrypt(char[] data, char[] key)
        {
            if (data.Length == 0)
                throw new ArgumentException($"Input array '{nameof(data)}' must be non-empty.");
            if (key.Length == 0)
                throw new ArgumentException($"Input array '{nameof(key)}' must be non-empty.");
            foreach (char c in data)
                if (!Alphabet.Contains(c))
                    throw new ArgumentException($"Input array '{nameof(data)}' must only contain characters contained in '{nameof(Alphabet)}'.");
            foreach (char c in key)
                if (!Alphabet.Contains(c))
                    throw new ArgumentException($"Input array '{nameof(key)}' must only contain characters contained in '{nameof(Alphabet)}'.");

            return Decrypt(new string(data), new string(key)).ToCharArray();
        }

        private static string KeyExpansion(string key, int length)
        {
            StringBuilder resultBuilder = new(length);

            if (key.Length <= length)
            {
                resultBuilder.Insert(0, key, length / key.Length);
                length %= key.Length;
            }

            if (length > 0)
            {
                resultBuilder.Append(key[..length]);
            }

            return resultBuilder.ToString();
        }
    }
}
