using System;
using System.Linq;

namespace IBCS.Util
{
    public class ByteArrayUtil
    {
        private static byte[] bitiIs1 = new byte[] { 0x80, 0x40, 0x20, 0x10, 0x8, 0x04, 0x02, 0x01 };
        public static int DegreeOf(byte[] ba)
        {
            // TODO Auto-generated method stub
            for (int i = 0; i < ba.Length; i++)
            {
                //first non-zero byte
                if (ba[i] != 0)
                {
                    return (ba.Length - i) * 8 - NumberOfLeadingZeroBits(ba[i]) - 1;
                }
            }
            //all zero.
            return -1;
        }

        public static int DegreeOf(sbyte[] ba)
        {
            // TODO Auto-generated method stub
            for (int i = 0; i < ba.Length; i++)
            {
                //first non-zero byte
                if (ba[i] != 0)
                {
                    return (ba.Length - i) * 8 - NumberOfLeadingZeroBits(ba[i]) - 1;
                }
            }
            //all zero.
            return -1;
        }

        /**
         * Returns the number of leading zero bits in a byte.
         */

        public static int NumberOfLeadingZeroBits(byte b)
        {
            int result;
            if (b < 0)
                result = 0;
            else if (b >= 64)
                result = 1;
            else if (b >= 32)
                result = 2;
            else if (b >= 16)
                result = 3;
            else if (b >= 8)
                result = 4;
            else if (b >= 4)
                result = 5;
            else if (b >= 2)
                result = 6;
            else if (b == 1)
                result = 7;
            else
                result = 8;
            return result;
        }


        public static int NumberOfLeadingZeroBits(sbyte b)
        {
            int result;
            if (b < 0)
                result = 0;
            else if (b >= 64)
                result = 1;
            else if (b >= 32)
                result = 2;
            else if (b >= 16)
                result = 3;
            else if (b >= 8)
                result = 4;
            else if (b >= 4)
                result = 5;
            else if (b >= 2)
                result = 6;
            else if (b == 1)
                result = 7;
            else
                result = 8;
            return result;
        }
        /**
         * Returns the bit as specified degree. if the input degree is out of range or negative;
         * return 0
         * @param  deg the degree
         * @return the bit as a boolean
         */
        public static bool GetBitByDegree(int deg, byte[] ba)
        {
            int bits = ba.Length * 8;
            if (deg < 0 || deg > bits)
                return false;

            int index = bits - deg - 1;
            int byteIndex = index / 8;
            int bitIndex = index % 8;

            byte b = ba[byteIndex];
            byte b2 = (byte)(b << bitIndex);

            return b2 < 0;
        }

        public static bool GetBitByDegree(int deg, sbyte[] ba)
        {
            int bits = ba.Length * 8;
            if (deg < 0 || deg > bits)
                return false;

            int index = bits - deg - 1;
            int byteIndex = index / 8;
            int bitIndex = index % 8;

            sbyte b = ba[byteIndex];
            sbyte b2 = (sbyte)(b << bitIndex);

            return b2 < 0;
        }

        /// <summary>
        /// Check if two byte arrays are equal
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool AreEqual(byte[] a, byte[] b)
        {
            return a.SequenceEqual(b);
        }

        public static bool AreEqual(sbyte[] a, sbyte[] b)
        {
            return a.SequenceEqual(b);
        }

    }
}
