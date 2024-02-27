namespace Cryptography
{
    internal class SieveOfEratosthenes
    {
        private readonly bool[] _sieve;
        private List<int>? _primes = null;

        public bool[] Sieve
        {
            get
            {
                bool[] result = new bool[_sieve.Length];
                _sieve.CopyTo(result, 0);
                return result;
            }
        }

        public List<int> Primes
        {
            get
            {
                if (_primes == null)
                {
                    _primes = [];
                    for (int i = 0; i < _sieve.Length; i++)
                        if (_sieve[i])
                            _primes.Add(i);
                }
                return new(_primes);
            }
        }

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

        //public List<int> GetPrimes()
        //{
        //    List<int> primes = [];
        //    for (int i = 0; i < _sieve.Length; i++)
        //        if (_sieve[i])
        //            primes.Add(i);
        //    return primes;
        //}

        //public List<int> GetPrimes(int maxValue)
        //{
        //    ArgumentOutOfRangeException.ThrowIfLessThan(maxValue, 0);

        //    List<int> primes = [];
        //    for (int i = 0; i < maxValue && i < _sieve.Length; i++)
        //        if (_sieve[i])
        //            primes.Add(i);
        //    return primes;
        //}

        //public List<int> GetPrimes(int minValue, int maxValue)
        //{
        //    ArgumentOutOfRangeException.ThrowIfNegative(minValue);
        //    if (minValue > maxValue)
        //        throw new ArgumentException($"'{nameof(minValue)}' must be greater than or equal to '{nameof(maxValue)}'.");
        //    if (maxValue >= _sieve.Length)
        //        throw new ArgumentException($"'{nameof(maxValue)}' must be less than size of '{nameof(Sieve)}'.");
        //    ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(maxValue, _sieve.Length);

        //    List<int> primes = [];
        //    for (int i = minValue; i < maxValue && i < _sieve.Length; i++)
        //        if (_sieve[i])
        //            primes.Add(i);
        //    return primes;
        //}
    }
}
