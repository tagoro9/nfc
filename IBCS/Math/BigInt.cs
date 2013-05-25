using System;
using IBCS.Interfaces;
using System.Runtime.Serialization;
using Org.BouncyCastle.Math;

namespace IBCS.Math
{
    /// <summary>
    /// Wrapper for the BigInteger class
    /// </summary>
    [DataContract]
    public class BigInt : FieldElement
    {
        private BigInteger value;
        public BigInteger Value
        {
            get { return value; }
            set { this.value = value; }
        }

        [DataMember(Name="Value")]
        public string HexValue
        {
            get { return this.ToString(16); }
            set
            {
                this.Value = new BigInteger(value, 16);
            }
        }


        public BigInt(BigInteger val)
        {
            this.value = val;
        }

        public BigInt(int numBits, Random rnd)
        {
            this.value = new BigInteger(numBits, rnd);
        }

        public BigInt(byte[] val)
        {
            this.value = new BigInteger(val);
        }

        public BigInt(String val, int radix = 16)
        {
            this.value = new BigInteger(val, radix);
        }

        public BigInt(long val)
        {
            this.Value = BigInteger.ValueOf(val);
        }

        public BigInt Abs()
        {
            return new BigInt(this.value.Abs());
        }

        public BigInt Add(BigInt val)
        {
            return new BigInt(Value.Add(val.Value));
        }

        public BigInt And(BigInt val)
        {
            return new BigInt(Value.And(val.Value));
        }

        public BigInt AndNot(BigInt val)
        {
            return new BigInt(this.Value.AndNot(val.Value));
        }

        public int BitCount()
        {
            return this.Value.BitCount;
        }

        public int BitLength()
        {
            return this.Value.BitLength;
        }

        public BigInt ClearBit(int n)
        {
            return new BigInt(this.Value.ClearBit(n));
        }

        public int CompareTo(BigInt val)
        {
            return this.Value.CompareTo(val.Value);
        }

        public BigInt Divide(BigInt val)
        {
            return new BigInt(this.Value.Divide(val.Value));
        }

        public BigInt[] DivideAndRemainder(BigInt val)
        {
            BigInteger[] res = this.Value.DivideAndRemainder(val.Value);
            return new BigInt[] { new BigInt(res[0]), new BigInt(res[1])};
        }

        //public double DoubleValue()

        public override bool Equals(Object x)
        {
            return this.Value.Equals(((BigInt)x).Value);
        }

        //public BigInt FlipBit(int n)

        //public float FloatValue();

        public BigInt Gcd(BigInt val)
        {
            return new BigInt(Value.Gcd(val.Value));
        }

        //public int GetLowestSetBit();

        public override int GetHashCode()
        {
            return this.Value.GetHashCode();
        }

        public int IntValue()
        {
            return Value.IntValue;
        }

        public bool IsProbablePrime(int certainty)
        {
            return Value.IsProbablePrime(certainty);
        }

        public long LongValue()
        {
            return Value.LongValue;
        }

        public BigInt Max(BigInt val)
        {
            return new BigInt(Value.Max(val.Value));
        }

        public BigInt Min(BigInt val)
        {
            return new BigInt(Value.Min(val.Value));
        }

        public BigInt Mod(BigInt m)
        {
            return new BigInt(Value.Mod(m.Value));
        }

        public BigInt ModInverse(BigInt m)
        {
            return new BigInt(Value.ModInverse(m.Value));
        }

        public BigInt ModPow(BigInt exponent, BigInt m)
        {
            return new BigInt(Value.ModPow(exponent.Value, m.Value));
        }

        public BigInt Multiply(BigInt val)
        {
            return new BigInt(Value.Multiply(val.Value));
        }

        public BigInt Negate()
        {
            return new BigInt(Value.Negate());
        }

        //public BigInt NextProbablePrime()

        public BigInt Not()
        {
            return new BigInt(Value.Not());
        }

        public BigInt Or(BigInt val)
        {
            return new BigInt(Value.Or(val.Value));
        }

        public BigInt Pow(int exponent) //This could fail
        {
            return new BigInt(Value.Pow(exponent));
        }

        public static BigInt ProbablePrime(int bitLength, Random rnd)
        {
            return new BigInt(BigInteger.ProbablePrime(bitLength, rnd));
        }

        public BigInt Remainder(BigInt val)
        {
            return new BigInt(Value.Remainder(val.Value));
        }

        public BigInt SetBit(int n)
        {
            return new BigInt(Value.SetBit(n));
        }

        public BigInt ShiftLeft(int n)
        {
            return new BigInt(Value.ShiftLeft(n));
        }

        public BigInt ShiftRight(int n)
        {
            return new BigInt(Value.ShiftRight(n));
        }

        public int Signum() //This could fail
        {
            return Value.SignValue;
        }

        public BigInt Subtract(BigInt val)
        {
            return new BigInt(Value.Subtract(val.Value));
        }

        public bool TestBit(int n)
        {
            return Value.TestBit(n);
        }

        public sbyte[] ToByteArray()
        {
            return (sbyte[])(Array)Value.ToByteArray();
        }

        public byte[] ToUByteArray()
        {
            return Value.ToByteArrayUnsigned();
        }

        public override String ToString()
        {
            return Value.ToString();
        }

        public String ToString(int radix)
        {
            return Value.ToString(radix);
        }

        public static BigInt ValueOf(long val)
        {
            return new BigInt(BigInteger.ValueOf(val));
        }

        public BigInt Xor(BigInt val)
        {
            return new BigInt(Value.Xor(val.Value));
        }

        public static BigInt ONE = new BigInt(BigInteger.One);

        public static BigInt TEN = new BigInt(BigInteger.Ten);

        public static BigInt ZERO = new BigInt(BigInteger.Zero);
    }
}
