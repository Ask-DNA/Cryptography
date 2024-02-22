using System.Collections;

namespace Cryptography.Encoders.Symmetric
{
    public class DataEncryptionStandard
    {
        private readonly int[] _initialPermutationTable = [
            58, 50, 42, 34, 26, 18, 10, 2, 60, 52, 44, 36, 28, 20, 12, 4,
            62, 54, 46, 38, 30, 22, 14, 6, 64, 56, 48, 40, 32, 24, 16, 8,
            57, 49, 41, 33, 25, 17, 9, 1, 59, 51, 43, 35, 27, 19, 11, 3,
            61, 53, 45, 37, 29, 21, 13, 5, 63, 55, 47, 39, 31, 23, 15, 7
            ];

        private readonly int[] _keyPermutationTableC = [
            57, 49, 41, 33, 25, 17, 9, 1, 58, 50, 42, 34, 26, 18,
            10, 2, 59, 51, 43, 35, 27, 19, 11, 3, 60, 52, 44, 36
            ];

        private readonly int[] _keyPermutationTableD = [
            63, 55, 47, 39, 31, 23, 15, 7, 62, 54, 46, 38, 30, 22,
            14, 6, 61, 53, 45, 37, 29, 21, 13, 5, 28, 20, 12, 4
            ];

        private readonly int[] _keyPermutationTableCD = [
            14, 17, 11, 24, 1, 5, 3, 28, 15, 6, 21, 10, 23, 19, 12, 4,
            26, 8, 16, 7, 27, 20, 13, 2, 41, 52, 31, 37, 47, 55, 30, 40,
            51, 45, 33, 48, 44, 49, 39, 56, 34, 53, 46, 42, 50, 36, 29, 32
            ];

        private readonly int[] _keyShiftTable = [1, 1, 2, 2, 2, 2, 2, 2, 1, 2, 2, 2, 2, 2, 2, 1];

        private readonly int[] _permutationTableE = [
            32, 1, 2, 3, 4, 5,
            4, 5, 6, 7, 8, 9,
            8, 9, 10, 11, 12, 13,
            12, 13, 14, 15, 16, 17,
            16, 17, 18, 19, 20, 21,
            20, 21, 22, 23, 24, 25,
            24, 25, 26, 27, 28, 29,
            28, 29, 30, 31, 32, 1
            ];

        private readonly int[] _permutationTableP = [
            16, 7, 20, 21, 29, 12, 28, 17,
            1, 15, 23, 26, 5, 18, 31, 10,
            2, 8, 24, 14, 32, 27, 3, 9,
            19, 13, 30, 6, 22, 11, 4, 25
            ];

        private readonly int[][,] _permutationTablesS = [
            new int[,]
            {
                { 14, 4, 13, 1, 2, 15, 11, 8, 3, 10, 6, 12, 5, 9, 0, 7 },
                { 0, 15, 7, 4, 14, 2, 13, 1, 10, 6, 12, 11, 9, 5, 3, 8 },
                { 4, 1, 14, 8, 13, 6, 2, 11, 15, 12, 9, 7, 3, 10, 5, 0 },
                { 15, 12, 8, 2, 4, 9, 1, 7, 5, 11, 3, 14, 10, 0, 6, 13 }
            },
            new int[,]
            {
                { 15, 1, 8, 14, 6, 11, 3, 4, 9, 7, 2, 13, 12, 0, 5, 10 },
                { 3, 13, 4, 7, 15, 2, 8, 14, 12, 0, 1, 10, 6, 9, 11, 5 },
                { 0, 14, 7, 11, 10, 4, 13, 1, 5, 8, 12, 6, 9, 3, 2, 15 },
                { 13, 8, 10, 1, 3, 15, 4, 2, 11, 6, 7, 12, 0, 5, 14, 9 }
            },
            new int[,]
            {
                { 10, 0, 9, 14, 6, 3, 15, 5, 1, 13, 12, 7, 11, 4, 2, 8 },
                { 13, 7, 0, 9, 3, 4, 6, 10, 2, 8, 5, 14, 12, 11, 15, 1 },
                { 13, 6, 4, 9, 8, 15, 3, 0, 11, 1, 2, 12, 5, 10, 14, 7 },
                { 1, 10, 13, 0, 6, 9, 8, 7, 4, 15, 14, 3, 11, 5, 2, 12 }
            },
            new int[,]
            {
                { 7, 13, 14, 3, 0, 6, 9, 10, 1, 2, 8, 5, 11, 12, 4, 15 },
                { 13, 8, 11, 5, 6, 15, 0, 3, 4, 7, 2, 12, 1, 10, 14, 9 },
                { 10, 6, 9, 0, 12, 11, 7, 13, 15, 1, 3, 14, 5, 2, 8, 4 },
                { 3, 15, 0, 6, 10, 1, 13, 8, 9, 4, 5, 11, 12, 7, 2, 14 }
            },
            new int[,]
            {
                { 2, 12, 4, 1, 7, 10, 11, 6, 8, 5, 3, 15, 13, 0, 14, 9 },
                { 14, 11, 2, 12, 4, 7, 13, 1, 5, 0, 15, 10, 3, 9, 8, 6 },
                { 4, 2, 1, 11, 10, 13, 7, 8, 15, 9, 12, 5, 6, 3, 0, 14 },
                { 11, 8, 12, 7, 1, 14, 2, 13, 6, 15, 0, 9, 10, 4, 5, 3 }
            },
            new int[,]
            {
                { 12, 1, 10, 15, 9, 2, 6, 8, 0, 13, 3, 4, 14, 7, 5, 11 },
                { 10, 15, 4, 2, 7, 12, 9, 5, 6, 1, 13, 14, 0, 11, 3, 8 },
                { 9, 14, 15, 5, 2, 8, 12, 3, 7, 0, 4, 10, 1, 13, 11, 6 },
                { 4, 3, 2, 12, 9, 5, 15, 10, 11, 14, 1, 7, 6, 0, 8, 13 }
            },
            new int[,]
            {
                { 4, 11, 2, 14, 15, 0, 8, 13, 3, 12, 9, 7, 5, 10, 6, 1 },
                { 13, 0, 11, 7, 4, 9, 1, 10, 14, 3, 5, 12, 2, 15, 8, 6 },
                { 1, 4, 11, 13, 12, 3, 7, 14, 10, 15, 6, 8, 0, 5, 9, 2 },
                { 6, 11, 13, 8, 1, 4, 10, 7, 9, 5, 0, 15, 14, 2, 3, 12 }
            },
            new int[,]
            {
                { 13, 2, 8, 4, 6, 15, 11, 1, 10, 9, 3, 14, 5, 0, 12, 7 },
                { 1, 15, 13, 8, 10, 3, 7, 4, 12, 5, 6, 11, 0, 14, 9, 2 },
                { 7, 11, 4, 1, 9, 12, 14, 2, 0, 6, 10, 13, 15, 3, 5, 8 },
                { 2, 1, 14, 7, 4, 10, 8, 13, 15, 12, 9, 0, 3, 5, 6, 11 }
            }
        ];

        public DataEncryptionStandard() { }

        public DataEncryptionStandard(
            int[] initialPermutationTable, int[] keyPermutationTableC, int[] keyPermutationTableD, int[] keyPermutationTableCD,
            int[] keyShiftTable, int[] permutationTableE, int[] permutationTableP, int[][,] permutationTablesS)
        {

            if (!ValidateTable(initialPermutationTable, out string message, 64, 1, 64))
                throw new ArgumentException(message + $"({nameof(initialPermutationTable)}).");

            if (!ValidateTable(keyPermutationTableC, out message, 28, 1, 64, false, [8, 16, 24, 32, 40, 48, 56, 64]))
                throw new ArgumentException(message + $"({nameof(keyPermutationTableC)}).");

            if (!ValidateTable(keyPermutationTableD, out message, 28, 1, 64, false, [8, 16, 24, 32, 40, 48, 56, 64]))
                throw new ArgumentException(message + $"({nameof(keyPermutationTableD)}).");

            if (!ValidateTable([.. keyPermutationTableC, .. keyPermutationTableD], out string _, 56))
                throw new ArgumentException($"Duplicates not allowed for concatination of tables '{nameof(keyPermutationTableC)}' and '{nameof(keyPermutationTableD)}'.");

            if (!ValidateTable(keyPermutationTableCD, out message, 48, 1, 56))
                throw new ArgumentException(message + $"({nameof(keyPermutationTableCD)}).");

            if (!ValidateTable(keyShiftTable, out message, 16, 1, 2, true))
                throw new ArgumentException(message + $"({nameof(keyShiftTable)}).");

            if (!ValidateTable(permutationTableE, out message, 48, 1, 32, true))
                throw new ArgumentException(message + $"({nameof(permutationTableE)}).");

            if (!ValidateTable(permutationTableP, out message, 32, 1, 32))
                throw new ArgumentException(message + $"({nameof(permutationTableP)}).");

            if (permutationTablesS.Length != 8)
                throw new ArgumentException($"Size must be 8. ({nameof(permutationTablesS)}).");

            foreach (int[,] table in permutationTablesS)
            {
                if (table.GetLength(0) != 4 || table.GetLength(1) != 16)
                    throw new ArgumentException($"Size of nested arrays must be [4, 16]. ({nameof(permutationTablesS)}).");
                for (int i = 0; i < 4; i++)
                    for (int j = 0; j < 16; j++)
                        if (table[i, j] < 0 || table[i, j] > 15)
                            throw new ArgumentException($"Values must be in range [0;15]. ({nameof(permutationTablesS)}).");
            }

            initialPermutationTable.CopyTo(_initialPermutationTable, 0);
            keyPermutationTableC.CopyTo(_keyPermutationTableC, 0);
            keyPermutationTableD.CopyTo(_keyPermutationTableD, 0);
            keyPermutationTableCD.CopyTo(_keyPermutationTableCD, 0);
            keyShiftTable.CopyTo(_keyShiftTable, 0);
            permutationTableE.CopyTo(_permutationTableE, 0);
            permutationTableP.CopyTo(_permutationTableP, 0);

            _permutationTablesS =
            [
                (int[,])permutationTablesS[0].Clone(),
                (int[,])permutationTablesS[1].Clone(),
                (int[,])permutationTablesS[2].Clone(),
                (int[,])permutationTablesS[3].Clone(),
                (int[,])permutationTablesS[4].Clone(),
                (int[,])permutationTablesS[5].Clone(),
                (int[,])permutationTablesS[6].Clone(),
                (int[,])permutationTablesS[7].Clone()
            ];
        }

        static bool ValidateTable(
            int[] table, out string message, int requiredSize, int min = int.MinValue, int max = int.MaxValue,
            bool allowDuplicates = false, int[]? forbiddenValues = null)
        {
            if (table.Length != requiredSize)
            {
                message = $"Size must be {requiredSize}.";
                return false;
            }
            for (int i = 0; i < table.Length; i++)
            {
                if (table[i] < min || table[i] > max)
                {
                    message = $"Values must be in range [{min};{max}].";
                    return false;
                }
                if (forbiddenValues != null && forbiddenValues.Contains(table[i]))
                {
                    message = $"Value {table[i]} is forbidden.";
                    return false;
                }
            }
            if (!allowDuplicates && table.Distinct().ToArray().Length != table.Length)
            {
                message = "Duplicates not allowed.";
                return false;
            }
            message = "";
            return true;
        }

        public BitArray Encode(BitArray input, BitArray key)
        {
            if (input.Length == 0)
                throw new ArgumentException("Size must be above zero.", nameof(input));
            if (key.Length != 56)
                throw new ArgumentException("Size must be 56.", nameof(key));

            BitArray[] splittedInput = input.Split(64);
            BitArray result = new(0);
            BitArray tmp;

            for (int i = 0; i < splittedInput.Length; i++)
            {
                tmp = Permutation(new(splittedInput[i]), _initialPermutationTable);
                tmp = FeistelPermutation(tmp, key);
                tmp = Permutation(tmp, _initialPermutationTable, true);
                result = result.Append(tmp);
            }

            return result;
        }

        public BitArray Decode(BitArray input, BitArray key)
        {
            if (input.Length == 0)
                throw new ArgumentException("Size must be above zero.", nameof(input));
            if (key.Length != 56)
                throw new ArgumentException("Size must be 56.", nameof(key));

            BitArray[] splittedInput = input.Split(64);
            BitArray result = new(0);
            BitArray tmp;

            for (int i = 0; i < splittedInput.Length; i++)
            {
                tmp = Permutation(new(splittedInput[i]), _initialPermutationTable);
                tmp = FeistelPermutation(tmp, key, true);
                tmp = Permutation(tmp, _initialPermutationTable, true);
                result = result.Append(tmp);
            }

            return result;
        }

        private static BitArray Permutation(BitArray input, int[] table, bool inverse = false)
        {
            BitArray result = new(table.Length, false);
            for (int i = 0; i < result.Length; i++)
                if (inverse)
                    result[table[i] - 1] = input[i];
                else
                    result[i] = input[table[i] - 1];
            return result;
        }

        private BitArray FeistelPermutation(BitArray input, BitArray key, bool inverse = false)
        {
            BitArray L = new(32, false);
            for (int i = 0; i < 32; i++)
                L[i] = input[i];
            BitArray R = new(32, false);
            for (int i = 32, j = 0; i < 64 && j < 32; i++, j++)
                R[j] = input[i];

            BitArray[] keys = GetKeys(key);

            if (inverse)
                for (int i = 15; i >= 0; i--)
                {
                    (L, R) = (R, L);
                    L = L.Xor(FeistelFunction(R, keys[i]));
                }
            else
                for (int i = 0; i < 16; i++)
                {
                    (L, R) = (R, L);
                    R = R.Xor(FeistelFunction(L, keys[i]));
                }

            return L.Append(R);
        }

        private BitArray[] GetKeys(BitArray key)
        {
            BitArray extendedKey = GetExtendedKey(key);
            BitArray[] result = new BitArray[16];
            BitArray C = Permutation(extendedKey, _keyPermutationTableC);
            BitArray D = Permutation(extendedKey, _keyPermutationTableD);
            for (int i = 0; i < 16; i++)
            {
                C = C.LeftCircularShift(_keyShiftTable[i]);
                D = D.LeftCircularShift(_keyShiftTable[i]);
                result[i] = new BitArray(Permutation(C.Append(D), _keyPermutationTableCD));
            }
            return result;
        }

        private static BitArray GetExtendedKey(BitArray key)
        {
            BitArray extendedKey = new(64, false);

            int trueCount = 0;
            for (int i = 0, j = 0; i < 64 && j < 56; i++, j++)
            {
                if (i % 8 != 0)
                {
                    extendedKey[i] = key[j];
                    if (extendedKey[i])
                        trueCount++;
                }
                else
                {
                    extendedKey[i] = (trueCount % 2 == 0);
                    trueCount = 0;
                    j--;
                }
            }

            return extendedKey;
        }

        private BitArray FeistelFunction(BitArray input, BitArray key)
        {
            input = Permutation(input, _permutationTableE);
            input = input.Xor(key);
            BitArray[] B = input.Split(6);
            BitArray[] Bnew = new BitArray[8];
            BitArray result;

            for (int i = 0; i < 8; i++)
                Bnew[i] = PermutationS(B[i], _permutationTablesS[i]);

            result = new(Bnew[0]);
            for (int i = 1; i < 8; i++)
                result = result.Append(Bnew[i]);

            result = Permutation(result, _permutationTableP);
            return result;
        }

        private static BitArray PermutationS(BitArray input, int[,] table)
        {
            int a, b, c;
            BitArray tmpArr;
            string tmpStr;

            tmpArr = new(2, false);
            tmpArr[0] = input[0];
            tmpArr[1] = input[5];
            a = Convert.ToInt32(tmpArr.ToBinaryString(), 2);

            tmpArr = new(4, false);
            tmpArr[0] = input[1];
            tmpArr[1] = input[2];
            tmpArr[2] = input[3];
            tmpArr[3] = input[4];
            b = Convert.ToInt32(tmpArr.ToBinaryString(), 2);

            c = table[a, b];
            tmpStr = Convert.ToString(c, 2);

            BitArray result = new(4, false);
            for (int i = 3, j = tmpStr.Length - 1; j >= 0; i--, j--)
            {
                result[i] = tmpStr[j] == '1';
            }

            return result;
        }
    }
}
