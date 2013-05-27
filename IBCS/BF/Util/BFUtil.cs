using System;
using Org.BouncyCastle.Crypto.Digests;
using Org.BouncyCastle.Crypto;

using IBCS.Math;
using IBCS.Interfaces;
using IBCS.EC;

namespace IBCS.BF.Util
{
    public class BFUtil
    {

        public static byte[] HashToLength(byte[] toHash, int byteLength)
        {
            if (byteLength <= 0)
                throw new ArgumentException("Invalid hash length");

            int bitLength = byteLength * 8;
            int round;

            if (bitLength <= 512)
            {
                round = 1;
            }
            else
            {
                round = 1 + (bitLength - 1) / 512;
            }

            byte[] result = new byte[byteLength];
            byte[][] temp = new byte[round][];

            IDigest hash = new Sha512Digest();
            for (int i = 0; i < round; i++)
            {
                hash.Reset();
                hash.BlockUpdate(toHash, 0, toHash.Length);
                temp[i] = new byte[hash.GetDigestSize()];
                hash.DoFinal(temp[i], 0);
                toHash = temp[i];
            }

            int startIndex = 0;
            //int length= byteLength;
            for (int i = 0; i < round; i++)
            {
                if (byteLength >= temp[i].Length)
                {
                    Array.Copy(temp[i], 0, result, startIndex, temp[i].Length);
                    startIndex += temp[i].Length;
                    byteLength -= temp[i].Length;
                }
                else
                    Array.Copy(temp[i], 0, result, startIndex, byteLength);
            }

            return result;
        }

        public static string ToHexString(byte[] ba)
        {
            string hex = BitConverter.ToString(ba);
            return hex.Replace("-", "");
        }

        public static BigInt HashToField(byte[] toHash, Field field)
        {
            int byteLength = 1 + (field.GetP().BitLength() - 1) / 8;

            byte[] ba = HashToLength(toHash, byteLength);

            BigInt b = new BigInt(ba); //This could fail

            while (b.CompareTo(field.GetP()) >= 0)
                b = b.ShiftRight(1);

            return b;
        }

        public static Point HashToPoint(byte[] toHash, EllipticCurve ec)
        {

            BigInt b = HashToField(toHash, ec.Field);

            Point P = ec.GetPoint(b);

            while (P == null)
            {
                b = b.Add(BigInt.ONE);
                P = ec.GetPoint(b);
            }
            return P;
        }

        public static Point HashToPoint(byte[] toHash, EllipticCurve ec, BigInt cofactor)
        {

            Point P = HashToPoint(toHash, ec);
            P = ec.Multiply(P, cofactor);
            return P;
        }

        public static byte[] Xor(byte[] a, byte[] b)
        {
            byte[] r = new byte[a.Length];
            for (int i = 0; i < a.Length; i++)
                r[i] = (byte) (a[i] ^ b[i]);
            return r;
        }

        public static byte[] XorTwoByteArrays(byte[] ba1, byte[] ba2)
        {
            byte[] result = new byte[ba1.Length];
            for (int i = 0; i < ba1.Length; i++)
            {
                result[i] = (byte)(ba1[i] ^ ba2[i]);
            }
            return result;
        }

    }
}
