using System.Collections;
using System.Text;

namespace Cryptography
{
    internal static class BitArrayExtensions
    {
        public static BitArray Append(this BitArray a, BitArray b)
        {
            bool[] boolArray = new bool[a.Count + b.Count];
            a.CopyTo(boolArray, 0);
            b.CopyTo(boolArray, a.Count);
            return new BitArray(boolArray);
        }

        public static BitArray LeftCircularShift(this BitArray a, int count)
        {
            BitArray result = new(a);
            bool tmp;
            for (int i = 0; i < count; i++)
            {
                tmp = result[result.Count - 1];
                result = result.LeftShift(1);
                result[0] = tmp;
            }
            return result;
        }

        public static BitArray[] Split(this BitArray a, int partLen)
        {
            int count = a.Length / partLen;
            if (a.Length % partLen != 0)
                count++;

            BitArray[] result = new BitArray[count];
            bool[] tmp;
            int k = 0;
            for (int i = 0; i < count; i++)
            {
                tmp = new bool[partLen];
                for (int j = 0; j < partLen; j++, k++)
                {
                    if (k < a.Length)
                        tmp[j] = a[k];
                    else
                        tmp[j] = false;
                }
                result[i] = new(tmp);
            }
            return result;
        }

        //public static byte[] ToByteArray(this BitArray a)
        //{
        //    if (a.Length == 0)
        //        throw new InvalidOperationException();
        //    if (a.Length % 8 != 0)
        //        throw new InvalidOperationException();

        //    byte[] result = new byte[a.Length / 8];
        //    a.CopyTo(result, 0);
        //    return result;
        //}

        public static string ToBinaryString(this BitArray a)
        {
            StringBuilder sb = new(a.Length);
            for (int i = 0; i < a.Length; i++)
                sb.Append(a[i] ? '1' : '0');
            return sb.ToString();
        }

        //public static BitArray DropLast(this BitArray a)
        //{
        //    BitArray result = new(a.Length - 1);
        //    for (int i = 0; i < result.Length; i++)
        //        result[i] = a[i];
        //    return result;
        //}
    }
}
