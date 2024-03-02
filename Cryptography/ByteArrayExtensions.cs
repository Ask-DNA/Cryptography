using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cryptography
{
    internal static class ByteArrayExtensions
    {
        public static byte[][] Split(this byte[] arr, int partLength, bool padding = false)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(partLength, nameof(partLength));
            if (arr.Length == 0)
                throw new ArgumentException($"Input array '{nameof(arr)}' must be non-empty.");
            if (!padding && arr.Length % partLength == 0)
                throw new ArgumentException($"Input array '{nameof(arr)}' could not be splitted without padding.");

            int countParts = arr.Length / partLength;
            if (arr.Length % partLength != 0)
                countParts++;

            byte[][] result = new byte[countParts][];

            int k = 0;
            for (int i = 0; i < countParts; i++)
            {
                result[i] = new byte[partLength];
                for (int j = 0; j < partLength; j++)
                {
                    if (k < arr.Length)
                    {
                        result[i][j] = arr[k];
                        k++;
                    }
                    else
                        result[i][j] = 0;
                }
            }

            return result;
        }

        public static byte[,] ReshapeTo2D(this byte[] arr, int rows, int columns, int majorDim = 0)
        {
            if (arr.Length == 0)
                throw new ArgumentException($"Input array '{nameof(arr)}' must be non-empty.");
            ArgumentOutOfRangeException.ThrowIfNegative(rows, nameof(rows));
            ArgumentOutOfRangeException.ThrowIfNegative(columns, nameof(columns));
            ArgumentOutOfRangeException.ThrowIfNegative(majorDim, nameof(majorDim));
            ArgumentOutOfRangeException.ThrowIfGreaterThan(majorDim, 1, nameof(majorDim));
            if (arr.Length != rows * columns)
                throw new ArgumentException($"Input array '{nameof(arr)}' of length = '{arr.Length}' could not be reshaped to 2D array of length = '{rows * columns}'.");

            byte[,] result = new byte[rows, columns];

            for (int i = 0; i < rows; i++)
                for (int j = 0; j < columns; j++)
                {
                    if (majorDim == 0)
                        result[i, j] = arr[j + columns * i];
                    else
                        result[i, j] = arr[i + rows * j];
                }

            return result;
        }

        public static byte[] Flatten(this byte[,] arr, int majorDim = 0)
        {
            if (arr.Length == 0)
                throw new ArgumentException($"Input array '{nameof(arr)}' must be non-empty.");
            ArgumentOutOfRangeException.ThrowIfNegative(majorDim, nameof(majorDim));
            ArgumentOutOfRangeException.ThrowIfGreaterThan(majorDim, 1, nameof(majorDim));

            byte[] result = new byte[arr.Length];

            int minorDim = (majorDim == 0) ? 1 : 0;
            int k = 0;
            for (int i = 0; i < arr.GetLength(majorDim); i++)
                for (int j = 0; j < arr.GetLength(minorDim); j++)
                {
                    if (majorDim == 0)
                        result[k] = arr[i, j];
                    else
                        result[k] = arr[j, i];
                    k++;
                }

            return result;
        }
    }
}
