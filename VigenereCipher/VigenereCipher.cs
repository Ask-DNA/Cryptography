using System.Text;

namespace VigenereCipher
{
    internal class VigenereCipher
    {
        public readonly string Alphabet;

        public VigenereCipher(string alphabet)
        {
            ArgumentException.ThrowIfNullOrEmpty(alphabet);
            Alphabet = alphabet;
        }

        private void ValidateInput(string s, string key)
        {
            ArgumentException.ThrowIfNullOrEmpty(s, nameof(s));
            ArgumentException.ThrowIfNullOrEmpty(key, nameof(key));

            foreach (char c in s)
                if (!Alphabet.Contains(c))
                    throw new ArgumentException("Invalid character.", nameof(s));

            foreach (char c in key)
                if (!Alphabet.Contains(c))
                    throw new ArgumentException("Invalid character.", nameof(key));
        }

        public string Encode(string s, string key)
        {
            ValidateInput(s, key);

            StringBuilder result = new(s);
            key = ResizeKey(key, s.Length);

            for (int i = 0; i < result.Length; i++)
            {
                result[i] = Alphabet[(Alphabet.IndexOf(s[i]) + Alphabet.IndexOf(key[i])) % Alphabet.Length];
            }
            return result.ToString();
        }

        public string Decode(string s, string key)
        {
            ValidateInput(s, key);

            StringBuilder result = new(s);
            key = ResizeKey(key, s.Length);

            int idx;
            for (int i = 0; i < result.Length; i++)
            {
                idx = (Alphabet.IndexOf(s[i]) - Alphabet.IndexOf(key[i])) % Alphabet.Length;
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
