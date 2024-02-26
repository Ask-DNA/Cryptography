namespace Cryptography
{
    internal class SieveOfEratosthenes
    {
        private readonly bool[] _sieve;

        public SieveOfEratosthenes(int n)
        {
            ArgumentOutOfRangeException.ThrowIfLessThan(n, 2);

            _sieve = Enumerable.Repeat(true, n + 1).ToArray();
            _sieve[0] = false;
            _sieve[1] = false;
            for (int i = 2; i * i <= n; i++)
                if (_sieve[i])
                    for (int j = i * i, k = 0; j <= n; j = (i * i) + (k * i), k++)
                        _sieve[j] = false;
        }

        public List<int> GetPrimes()
        {
            List<int> primes = [];
            for (int i = 0; i < _sieve.Length; i++)
                if (_sieve[i])
                    primes.Add(i);
            return primes;
        }

        public List<int> GetPrimes(int rightBorder)
        {
            ArgumentOutOfRangeException.ThrowIfLessThan(rightBorder, 0);

            List<int> primes = [];
            for (int i = 0; i < rightBorder && i < _sieve.Length; i++)
                if (_sieve[i])
                    primes.Add(i);
            return primes;
        }

        public List<int> GetPrimes(int leftBorder, int rightBorder)
        {
            ArgumentOutOfRangeException.ThrowIfLessThan(leftBorder, 0);
            ArgumentOutOfRangeException.ThrowIfLessThan(rightBorder, leftBorder);
            ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(rightBorder, _sieve.Length);

            List<int> primes = [];
            for (int i = leftBorder; i < rightBorder && i < _sieve.Length; i++)
                if (_sieve[i])
                    primes.Add(i);
            return primes;
        }
    }
}
