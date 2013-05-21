using System;
using IBCS.Interfaces;
using IBCS.Util;

namespace IBCS.Math
{
    public class Complex : FieldElement
    {
        private Fp field;
        private BigInt real;
        private BigInt imag;

        public Fp Field
        {
            get { return field; }
        }

        public BigInt Real
        {
            get { return real; }
        }

        public BigInt Imag
        {
            get { return imag; }
        }

        public Complex(Fp field, BigInt real)
        {
            this.field = field;
            if (!this.field.IsValidElement(real))
                throw new ArgumentException("The input values are not in the field.");
            this.real = real;
            this.imag = BigInt.ZERO;
        }

        public Complex(Fp field, BigInt real, BigInt imag)
        {
            this.field = field;
            this.real = real;
            this.imag = imag;
        }

        public bool IsZero()
        {
            return this.field.IsZero(imag) & this.field.IsZero(this.real);
        }

        public bool IsOne()
        {
            return this.field.IsZero(imag) & this.field.IsOne(this.real);
        }

        public Complex Add(Complex p)
        {
            if (!((Complex)p).Field.Equals(this.field))
                throw new ArgumentException("The input polynomial is over another field.");

            if (this.IsZero())
                return p;
            else if (((Complex)p).IsZero())
                return this;

            BigInt newReal = (BigInt)this.field.Add(this.real, ((Complex)p).real);
            BigInt newImag = (BigInt)this.field.Add(this.imag, ((Complex)p).imag);

            return new Complex(this.field, newReal, newImag);

        }

        public Complex Subtract(Complex p)
        {
            if (!((Complex)p).Field.Equals(this.field))
                throw new ArgumentException("The input polynomial is over another field.");

            if (((Complex)p).IsZero())
                return this;
            BigInt newReal = (BigInt)this.field.Subtract(this.real, ((Complex)p).real);
            BigInt newImag = (BigInt)this.field.Subtract(this.imag, ((Complex)p).imag);

            return new Complex(this.field, newReal, newImag);
        }

        public Complex Negate()
        {
            if (this.IsZero())
                return this;
            return new Complex(this.field, (BigInt)this.field.Negate(this.real), (BigInt)this.field.Negate(this.imag));
        }

        public Complex Multiply(Complex p)
        {
            if (!((Complex)p).Field.Equals(this.field))
                throw new ArgumentException("The input polynomial is over another field.");

            // shortcuts
            if (this.IsZero())
                return this;

            if (((Complex)p).IsZero())
                return p;

            if (this.IsOne())
                return p;

            if (((Complex)p).IsOne())
                return this;

            if (p.Equals(this))
                return (Complex)this.Square();

            //(a+bi)*(c+di)=ac+adi+bci-bd
            //(a + ib)(c + id) = ac - bd + i[(a + b)(c + d) - ac - bd)]

            //BigInt newReal =(BigInt) this.field.substract(this.field.multiply(this.real, p.real), this.field.multiply(this.imag, p.imag));
            //BigInt newImag = (BigInt) this.field.add(this.field.multiply(this.imag, p.real), this.field.multiply(this.real, p.imag));
            BigInt ac = this.real.Multiply(p.Real);
            BigInt bd = this.imag.Multiply(p.Imag);
            BigInt newReal = (BigInt)this.field.Subtract(ac, bd);

            BigInt newImag = this.real.Add(this.imag);
            newImag = newImag.Multiply(p.Real.Add(p.Imag));
            newImag = newImag.Subtract(ac);
            newImag = (BigInt)this.field.Subtract(newImag, bd);
            return new Complex(this.field, newReal, newImag);
        }

        public Complex Square()
        {
            // TODO Auto-generated method stub
            //0 or 1
            if (this.IsOne() || this.IsZero())
                return this;
            //real number
            BigInt newReal;
            if (this.imag.Equals(BigInt.ZERO))
            {
                newReal = (BigInt)this.field.Multiply(this.real, this.real);
                return new Complex(this.field, newReal, BigInt.ZERO);
            }
            //imaginary
            if (this.real.Equals(BigInt.ZERO))
            {
                newReal = (BigInt)this.field.Multiply(this.imag, this.imag);
                newReal = (BigInt)this.field.Negate(newReal);
                return new Complex(this.field, newReal, BigInt.ZERO);
            }

            //(a+bi)^2=(a+b)(a-b)+2abi
            newReal = (BigInt)this.field.Multiply(this.real.Add(this.imag), this.real.Subtract(this.imag));
            //BigInt newImag =(BigInt) this.field.multiply(this.real.multiply(BigInt.valueOf(2)), this.imag);
            BigInt newImag = (BigInt)this.field.Multiply(this.field.Add(this.real, this.real), this.imag);
            return new Complex(this.field, newReal, newImag);
        }

        public Complex Divide(Complex p)
        {
            if (!((Complex)p).Field.Equals(this.field))
                throw new ArgumentException("The input polynomial is over another field.");
            if (((Complex)p).IsZero())
            {
                throw new ArithmeticException("Divided by Zero.");
            }
            if (this.IsZero())
            {
                return new Complex(this.field, BigInt.ZERO, BigInt.ZERO);
            }

            if (((Complex)p).IsOne())
            {
                return this;
            }

            Complex conj = ((Complex)p).Conjugate();

            Complex top = (Complex)this.Multiply(conj);
            BigInt bottom = (BigInt)this.field.Add(this.field.Square(((Complex)p).real), this.field.Square(((Complex)p).imag));

            return new Complex(this.field, (BigInt)this.field.Divide(top.real, bottom), (BigInt)this.field.Divide(top.imag, bottom));

        }

        public Complex Conjugate()
        {
            return new Complex(this.field, this.real, (BigInt)this.field.Negate(this.imag));
        }

        public Complex Pow(BigInt k)
        {

            if (k.Equals(BigInt.ZERO))
                return new Complex(this.field, (BigInt)this.field.GetOne(), (BigInt)this.field.GetZero());
            if (k.Equals(BigInt.ONE))
                return this;
            if (this.IsOne() || this.IsZero())
                return this;


            bool inverse = false;
            if (k.Signum() == -1)
            {
                k = k.Abs();
                inverse = true;
            }


            Complex u = this;
            int windowSize = 5;
            int tLength = (int)System.Math.Pow(2, windowSize - 1);
            //prepare table for windowing
            FieldElement u2 = u.Square();
            Complex[] t = new Complex[tLength];
            t[0] = u;

            for (int i = 1; i < tLength; i++)
            {
                t[i] = ((Complex)u2).Multiply(t[i - 1]);
            }
            //left to right method -with windows

            int nb = k.BitLength();

            int[] nbw = new int[1];
            int[] nzs = new int[1];
            int n;
            if (nb > 1)
                for (int i = nb - 2; i >= 0; )
                {
                    n = Window(k, i, nbw, nzs, windowSize);
                    for (int j = 0; j < nbw[0]; j++)
                        u = u.Square();
                    if (n > 0)
                        u = u.Multiply(t[n / 2]);
                    i -= nbw[0];
                    if (nzs[0] != 0)
                    {
                        for (int j = 0; j < nzs[0]; j++)
                            u = u.Square();
                        i -= nzs[0];

                    }
                }

            if (inverse)
            {
                return u.Inverse();
            }
            return u;
        }

        private int Window(BigInt x, int i, int[] nbs, int[] nzs, int w)
        {
            // TODO Auto-generated method stub
            int j, r;
            nbs[0] = 1;
            nzs[0] = 0;
            sbyte[] xa = x.ToByteArray();

            //check for leading 0 bit
            if (!ByteArrayUtil.GetBitByDegree(i, xa))
                return 0;
            //adjust window if not enough bits left
            if (i - w + 1 < 0)
                w = i + 1;

            r = 1;
            for (j = i - 1; j > i - w; j--)
            {
                nbs[0]++;
                r *= 2;
                if (ByteArrayUtil.GetBitByDegree(j, xa))
                    r += 1;
                if (r % 4 == 0)
                {
                    r /= 4;
                    nbs[0] -= 2;
                    nzs[0] = 2;
                    break;
                }
            }

            if (r % 2 == 0)
            {
                r /= 2;
                nzs[0] = 1;
                nbs[0]--;
            }
            return r;
        }

        public Complex Inverse()
        {
            return new Complex(this.field, BigInt.ONE, BigInt.ZERO).Divide(this);
        }

        public Complex Multiply(int val)
        {
            if (val < 0)
                throw new ArgumentException("oprand must not be negative");
            if (val == 0)
                return new Complex(this.field, BigInt.ZERO, BigInt.ZERO);
            if (val == 1)
                return this;
            return new Complex(this.field, (BigInt)this.field.Multiply(this.real, val), (BigInt)this.field.Multiply(this.imag, val));
        }

        public override String ToString()
        {
            return "[" + this.real.ToString() + "," + this.imag.ToString() + "]";
        }

        public String ToString(int radix)
        {
            return "[" + this.real.ToString(radix) + "," + this.imag.ToString(radix) + "]";
        }

        public override int GetHashCode()
        {
            const int prime = 31;
            int result = 1;
            result = prime * result + ((field == null) ? 0 : field.GetHashCode());
            result = prime * result + ((imag == null) ? 0 : imag.GetHashCode());
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
            Complex other = (Complex)obj;
            if (field == null)
            {
                if (other.field != null)
                    return false;
            }
            else if (!field.Equals(other.field))
                return false;
            if (imag == null)
            {
                if (other.imag != null)
                    return false;
            }
            else if (!imag.Equals(other.imag))
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

        public sbyte[] ToByteArray()
        {
            sbyte[] r = this.real.ToByteArray();
            sbyte[] i = this.imag.ToByteArray();

            sbyte[] all = new sbyte[r.Length + i.Length];
            Array.Copy(r, 0, all, 0, r.Length);
            Array.Copy(i, 0, all, r.Length, i.Length);
            return all;
        }

        public byte[] ToUByteArray()
        {
            byte[] r = this.real.ToUByteArray();
            byte[] i = this.imag.ToUByteArray();

            byte[] all = new byte[r.Length + i.Length];
            Array.Copy(r, 0, all, 0, r.Length);
            Array.Copy(i, 0, all, r.Length, i.Length);
            return all;
        }

        public HalfComplex ToHalfComplex()
        {
            return new HalfComplex(this.field, this.real);
        }

    }
}
