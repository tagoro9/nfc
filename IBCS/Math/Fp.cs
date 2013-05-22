using System;
using IBCS.Interfaces;
using System.Runtime.Serialization;

namespace IBCS.Math
{
    [DataContract]
    public class Fp : Field
    {
        private BigInt r;
        private BigInt inverse2;

        [DataMember]
        public BigInt P
        {
            get { return r; }
        }

        public BigInt Inverse2
        {
            get { return inverse2; }
        }

        /**
         * Creates a finite field
         * @param p the characteristics of this field.
         * @exception IllegalArgumentException if <code>p</code> is not prime.
         */
        public Fp(BigInt p)
        {
            if (!p.IsProbablePrime(40))
                throw new ArgumentException("The modulus must be a prime number.");
            this.r = p;
            this.inverse2 = BigInt.ValueOf(2).ModInverse(p);
        }

        public FieldElement Add(FieldElement val1, FieldElement val2)
        {
            if (!(val1 is BigInt) || (!(val2 is BigInt)))
                throw new ArgumentException("The inputs must be BigInts");
            if (val1.Equals(BigInt.ZERO))
                return val2;
            if (val2.Equals(BigInt.ZERO))
                return val1;
            BigInt result = ((BigInt)val1).Add((BigInt)val2);
            //if (result.bitLength()<this.r.bitLength())
            //	return result;
            //else
            return (result).Mod(r);
        }

        public FieldElement Inverse(FieldElement val)
        {
            if (!(val is BigInt))
                throw new ArgumentException("The input must be a BigInt");
            if (val.Equals(BigInt.ZERO))
                throw new ArgumentException("Zero cannot be inversed");
            if (val.Equals(BigInt.ONE))
                return val;
            return ((BigInt)val).ModInverse(r);
        }

        public FieldElement Multiply(FieldElement val1, FieldElement val2)
        {
            if (!(val1 is BigInt) || (!(val2 is BigInt)))
                throw new ArgumentException("The inputs must be BigInts");
            if (((BigInt)val1).Signum() == 0 || ((BigInt)val2).Signum() == 0)
                return BigInt.ZERO;
            if (val1.Equals(BigInt.ONE))
                return val2;
            if (val2.Equals(BigInt.ONE))
                return val1;

            BigInt result = ((BigInt)val1).Multiply((BigInt)val2);
            return (result).Mod(r);
        }

        public FieldElement Divide(FieldElement val1, FieldElement val2)
        {
            // TODO Auto-generated method stub
            if (!(val1 is BigInt) || (!(val2 is BigInt)))
                throw new ArgumentException("The inputs must be BigInts");
            return (((BigInt)val1).Multiply((BigInt)this.Inverse(val2))).Mod(r);
        }

        public FieldElement Subtract(FieldElement val1, FieldElement val2)
        {
            if (!(val1 is BigInt) || (!(val2 is BigInt)))
                throw new ArgumentException("The inputs must be BigInts");
            // TODO Auto-generated method stub
            if (val2.Equals(BigInt.ZERO))
                return ((BigInt)val1).Mod(r);
            if (val1.Equals(BigInt.ZERO))
                return this.Negate(val2);

            BigInt result = ((BigInt)val1).Subtract((BigInt)val2);
            //if(result.bitLength()<this.r.bitLength())
            //	return result;
            //else
            return result.Mod(r);
        }

        public FieldElement RandomElement(Random rnd)
        {
            BigInt rd = new BigInt(this.r.BitLength(), rnd);
            while (rd.CompareTo(r) >= 0)
                rd = rd.ShiftRight(1);
            return rd;
        }

        public bool IsValidElement(FieldElement val)
        {
            if (val is BigInt)
            {
                if (r.CompareTo((BigInt)val) > 0)
                    return true;
                else
                    return false;
            }
            else
            {
                return false;
            }
        }

        private BigInt Reduce(BigInt val)
        {
            if (val.Signum() < 0)
            {
                return val.Mod(this.r);
            }
            if (r.CompareTo(val) > 0)
                return val;
            else
                return val.Mod(r);
        }

        public FieldElement Multiply(FieldElement val, int val2)
        {
            if (!(val is BigInt))
                throw new ArgumentException("The first input must be a BigInt");
            if (val2 < 0)
                throw new ArgumentException("oprand must not be negative");
            BigInt result = ((BigInt)val).Multiply(BigInt.ValueOf(val2));
            return (result).Mod(r);
        }

        public FieldElement Negate(FieldElement val)
        {
            if (!(val is BigInt))
                throw new ArgumentException("The input must be a BigInt");
            //return val.negate();
            if (((BigInt)val).Signum() == 0)
                return val;
            if (((BigInt)val).Signum() == -1)
                return ((BigInt)val).Negate();
            val = this.Reduce((BigInt)val);
            return this.Subtract(this.r, val);
        }

        public FieldElement GetOne()
        {
            return BigInt.ONE;
        }

        public FieldElement GetZero()
        {
            return BigInt.ZERO;
        }

        public bool IsOne(FieldElement val)
        {
            if (val is BigInt)
                return val.Equals(BigInt.ONE);
            else
                return false;
        }

        public bool IsZero(FieldElement val)
        {
            if (val is BigInt)
                return val.Equals(BigInt.ZERO);
            else
                return false;
        }

        public FieldElement Square(FieldElement val)
        {
            if (!(val is BigInt))
                throw new ArgumentException("The input must be a BigInt");
            // TODO Auto-generated method stub
            if (((BigInt)val).Signum() == 0)
                return val;
            if (val.Equals(BigInt.ONE))
                return val;
            BigInt result = ((BigInt)val).Multiply((BigInt)val);
            return (result).Mod(r);
        }

        public FieldElement SquareRoot(FieldElement val)
        {
            if (!(val is BigInt))
                throw new ArgumentException("The input must be a BigInt");
            BigInt p = this.r;
            BigInt k;


            BigInt g = (BigInt)val;

            //g==0 || g==1
            if (g.Equals(BigInt.ZERO) || g.Equals(BigInt.ONE))
                return g;


            BigInt[] result = p.DivideAndRemainder(BigInt.ValueOf(4));
            //p=3 mod 4, i.e. p=4k+3, output val^{k+1} mod p
            if (result[1].Equals(BigInt.ValueOf(3)))
            {
                //System.out.println("case1");
                k = result[0];
                //System.out.println("k:");
                //System.out.println(k.toString());
                BigInt z = g.ModPow(k.Add(BigInt.ONE), p);
                if (this.Square(z).Equals(g))
                    return z;
                else
                    return null;

            }


            result = p.DivideAndRemainder(BigInt.ValueOf(8));
            k = result[0];
            BigInt remainder = result[1];
            //p=5 mod 8 i.e. p=8k+5
            if (remainder.Equals(BigInt.ValueOf(5)))
            {
                //System.out.println("case2");
                //gamma=(2g)^k mod p
                BigInt g2 = g.Multiply(BigInt.ValueOf(2));
                BigInt gamma = g2.ModPow(k, p);
                //i=2g*gamma^2 mod p
                BigInt i = (g2.Multiply(gamma.ModPow(BigInt.ValueOf(2), p))).Mod(p);
                //output g*gamma(i-1) mod p
                BigInt z = (g.Multiply(gamma.Multiply(i.Subtract(BigInt.ONE))).Mod(p));
                if (this.Square(z).Equals(g))
                    return z;
                else
                    return null;

            }
            else if (remainder.Equals(BigInt.ONE))
            {
                //System.out.println("case3");
                BigInt q = g;
                do
                {
                    BigInt P = (BigInt)this.RandomElement(new Random());
                    BigInt K = (p.Add(BigInt.ONE)).Divide(BigInt.ValueOf(2));
                    BigInt[] re = LucasSequence(P, q, K);

                    BigInt V = re[0];
                    BigInt Q0 = re[1];

                    BigInt z = (V.Divide(BigInt.ValueOf(2))).Mod(p);

                    if ((this.Square(z)).Equals(g))
                        return z;
                    if (Q0.CompareTo(BigInt.ONE) > 0)
                    {
                        if (Q0.CompareTo(p.Subtract(BigInt.ONE)) < 0)
                            return null;
                    }
                } while (true);

            }
            return null;
        }

        private BigInt[] LucasSequence(BigInt p, BigInt q, BigInt k)
        {
            if (k.Signum() < 0)
                throw new ArgumentException("k must be a positive integer");

            //BigInt n=this.r;
            BigInt v0 = BigInt.ValueOf(2);
            BigInt v1 = p;
            BigInt q0 = BigInt.ONE;
            BigInt q1 = BigInt.ONE;



            //byte [] kByte =k.toByteArray();
            int r = k.BitLength() - 1;

            for (int i = r; i >= 0; i--)
            {
                //q0=q0q1 mod n
                q0 = (BigInt)this.Multiply(q0, q1);

                if (k.TestBit(i))
                {
                    //q1=q0q mod n
                    q1 = (BigInt)this.Multiply(q0, q);
                    //v0=v0v1-pq0 mod n
                    v0 = (BigInt)this.Multiply(v0, v1);
                    v0 = (BigInt)this.Subtract(v0, this.Multiply(p, q0));
                    //v1=v1^2-2q1 mod n

                    v1 = (BigInt)this.Square(v1);
                    v1 = (BigInt)this.Subtract(v1, this.Multiply(q1, 2));

                }
                else
                {
                    //q1=q0
                    q1 = q0;
                    //v1=v0v1-pq0 mod n
                    v1 = (BigInt)this.Multiply(v0, v1);
                    v1 = (BigInt)this.Subtract(v1, this.Multiply(p, q0));
                    //v0=v0^2-2q0 mod n
                    v0 = (BigInt)this.Square(v0);
                    v0 = (BigInt)this.Subtract(v0, this.Multiply(q0, 2));

                }

            }

            BigInt[] result = { v0, q0 };
            return result;
        }

        public BigInt GetOrder()
        {
            return this.r;
        }

        public BigInt GetP()
        {
            return this.r;
        }

        public override int GetHashCode()
        {
            const int prime = 31;
            int result = 1;
            result = prime * result + ((r == null) ? 0 : r.GetHashCode());
            return result;
        }

        public override bool Equals(Object obj) //This could fail
        {
            if (this == obj)
                return true;
            if (obj == null)
                return false;
            if (!(obj is Fp))
                return false;
            Fp other = (Fp)obj;
            if (r == null)
            {
                if (other.r != null)
                    return false;
            }
            else if (!r.Equals(other.r))
                return false;
            return true;
        }

        public FieldElement Pow(FieldElement val, BigInt exp)
        {
            if (!(val is BigInt))
                throw new ArgumentException("The input FieldElement must be a BigInt");
            return ((BigInt)val).ModPow((BigInt)exp, this.r);
        }

    }
}
