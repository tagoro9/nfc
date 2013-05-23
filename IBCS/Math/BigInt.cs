using System;
using IBCS.Interfaces;
using System.Runtime.Serialization;

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
            BigInteger rand = new BigInteger();
            rand.genRandomBits(numBits, rnd);
            this.value = rand;
        }

        public BigInt(byte[] val)
        {
            this.value = new BigInteger(val);
        }

        public BigInt(String val, int radix = 16)
        {
            this.value = new BigInteger(val, radix);
            // TODO Auto-generated constructor stub
        }


        public BigInt Abs()
        {
            return new BigInt(this.value.abs());
        }

        public BigInt Add(BigInt val)
        {
            return new BigInt(Value + val.Value);
        }

        public BigInt And(BigInt val)
        {
            return new BigInt(Value & val.Value);
        }

        public BigInt AndNot(BigInt val)
        {
            return new BigInt(this.Value & ~val.Value);
        }

        public int BitCount() //This could fail
        {
            throw new NotImplementedException();
        }

        public int BitLength()
        {
            return this.Value.bitCount();
        }

        public BigInt ClearBit(int n)
        {
            BigInteger copy = this.Value;
            copy.unsetBit(Convert.ToUInt16(n));
            return new BigInt(copy);
        }

        public int CompareTo(BigInt val)
        {
            if (this.Value == val.Value)
                return 0;
            else if (this.Value > val.Value)
                return 1;
            else
                return -1;
        }

        public BigInt Divide(BigInt val)
        {
            return new BigInt(Value / val.Value);
        }

        public BigInt[] DivideAndRemainder(BigInt val)
        {
            BigInteger modulus = Value % val.Value;
            BigInteger result = Value / val.Value;
            return new BigInt[2] { new BigInt(result), new BigInt(modulus) };
        }

        //public double DoubleValue()

        public override bool Equals(Object x)
        {
            if (GetType() != x.GetType())
                return false;
            return this.Value.Equals(((BigInt)x).Value);
        }

        //public BigInt FlipBit(int n)

        //public float FloatValue();

        public BigInt Gcd(BigInt val)
        {
            return new BigInt(Value.gcd(val.Value));
        }

        //public int GetLowestSetBit();

        public override int GetHashCode()
        {
            return this.Value.GetHashCode();
        }

        public int IntValue()
        {
            return Value.IntValue();
        }

        public bool IsProbablePrime(int certainty)
        {
            return Value.isProbablePrime(certainty);
        }

        public long LongValue()
        {
            return Value.LongValue();
        }

        public BigInt Max(BigInt val)
        {
            return new BigInt(Value.max(val.Value));
        }

        public BigInt Min(BigInt val)
        {
            return new BigInt(Value.min(val.Value));
        }

        public BigInt Mod(BigInt m)
        {
            return new BigInt(Value % m.Value);
        }

        public BigInt ModInverse(BigInt m)
        {
            return new BigInt(Value.modInverse(m.Value));
        }

        public BigInt ModPow(BigInt exponent, BigInt m)
        {
            return new BigInt(Value.modPow(exponent.Value, m.Value));
        }

        public BigInt Multiply(BigInt val)
        {
            return new BigInt(Value * val.Value);
        }

        public BigInt Negate()
        {
            return new BigInt(-Value);
        }

        //public BigInt NextProbablePrime()

        public BigInt Not()
        {
            return new BigInt(~Value);
        }

        public BigInt Or(BigInt val)
        {
            return new BigInt(Value | val.Value);
        }

        public BigInt Pow(int exponent) //This could fail
        {
            return new BigInt(Value.pow(exponent));
        }

        public static BigInt ProbablePrime(int bitLength, Random rnd)
        {
            return new BigInt(BigInteger.genPseudoPrime(bitLength, 10, rnd));
        }

        public BigInt Remainder(BigInt val)
        {
            return new BigInt(Value % val.Value);
        }

        public BigInt SetBit(int n)
        {
            BigInteger copy = Value;
            copy.setBit(Convert.ToUInt16(n));
            return new BigInt(copy);
        }

        public BigInt ShiftLeft(int n)
        {
            return new BigInt(Value << n);
        }

        public BigInt ShiftRight(int n)
        {
            return new BigInt(Value >> n);
        }

        public int Signum() //This could fail
        {
            return Value.Signum();
        }

        public BigInt Subtract(BigInt val)
        {
            return new BigInt(Value - val.Value);
        }

        public bool TestBit(int n)
        {
            return Value.TestBit(Convert.ToUInt16(n));
        }

        public sbyte[] ToByteArray()
        {
            return Value.ToSByteArray();
        }

        public byte[] ToUByteArray()
        {
            return Value.ToByteArray();
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
            return new BigInt(new BigInteger(val));
        }

        public BigInt Xor(BigInt val)
        {
            return new BigInt(Value ^ val.Value);
        }

        public static BigInt ONE = new BigInt(new BigInteger(1));

        public static BigInt TEN = new BigInt(new BigInteger(10));

        public static BigInt ZERO = new BigInt(new BigInteger(0));
    }
}
