using System;
using IBCS.Interfaces;

namespace IBCS.Math
{
    public class HalfComplex : FieldElement
    {
        private BigInt real;
        private Fp field;
        public Fp Field
        {
            get { return field; }
        }
        public BigInt Real
        {
            get { return real; }
        }

        public HalfComplex(Fp field, BigInt real)
        {
            this.field = field;
            this.real = real;
        }

        public HalfComplex Add(HalfComplex p)
        {
            return new HalfComplex(this.field, (BigInt)this.field.Add(this.real, p.real));
        }

        public HalfComplex Conjugate()
        {
            return this;
        }

        public HalfComplex Multiply(BigInt p)
        {
            return new HalfComplex(this.field, (BigInt)this.field.Multiply(this.real, p));
        }


        public HalfComplex Multiply(int val)
        {
            return new HalfComplex(this.field, (BigInt)this.field.Multiply(this.real, val));
        }


        public HalfComplex Negate()
        {
            return new HalfComplex(this.field, (BigInt)this.field.Negate(this.real));
        }

        public HalfComplex Pow(BigInt n)
        {
            BigInt p = this.real.ShiftLeft(1);
            BigInt two = BigInt.ValueOf(2);

            FieldElement v0 = two;
            FieldElement v1 = p;

            int t = n.BitLength() - 1;

            for (int j = t; j >= 0; j--)
            {
                if (n.TestBit(j))
                {
                    v0 = this.field.Subtract(this.field.Multiply(v0, v1), p);
                    v1 = this.field.Subtract(this.field.Square(v1), two);
                }
                else
                {
                    v1 = this.field.Subtract(this.field.Multiply(v0, v1), p);
                    v0 = this.field.Subtract(this.field.Square(v0), two);
                }
            }
            if (((BigInt)v0).TestBit(0))
            {
                return new HalfComplex(this.field, (BigInt)this.field.Multiply(v0, this.field.Inverse2));
            }
            else
            {
                return new HalfComplex(this.field, ((BigInt)v0).ShiftRight(1));
            }
        }

        public HalfComplex Square()
        {
            return this.Pow(BigInt.ValueOf(2));
        }

        public HalfComplex Subtract(HalfComplex p)
        {
            return new HalfComplex(this.field, (BigInt)this.field.Subtract(this.real, p.real));
        }

        public override String ToString()
        {
            return "[" + this.real.ToString() + "]";
        }

        public sbyte[] ToByteArray()
        {
            return this.real.ToByteArray();
        }

        public byte[] ToUByteArray()
        {
            return this.real.ToUByteArray();
        }

        public String ToString(int radix)
        {
            return "[" + this.real.ToString(radix) + "]";
        }

        public override int GetHashCode()
        {
            const int prime = 31;
            int result = 1;
            result = prime * result + ((field == null) ? 0 : field.GetHashCode());
            result = prime * result + ((real == null) ? 0 : real.GetHashCode());
            return result;
        }

        public override bool Equals(Object obj)
        {
            if (this == obj)
                return true;
            if (obj == null)
                return false;
            if (GetType() != obj.GetType())
                return false;
            HalfComplex other = (HalfComplex)obj;
            if (field == null)
            {
                if (other.field != null)
                    return false;
            }
            else if (!field.Equals(other.field))
                return false;
            if (real == null)
            {
                if (other.real != null)
                    return false;
            }
            else if (!real.Equals(other.real))
                return false;
            return true;
        }
    }
}
