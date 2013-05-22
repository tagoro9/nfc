using System;
using IBCS.Math;
using IBCS.Interfaces;
using System.Runtime.Serialization;

namespace IBCS.EC
{
    [DataContract]
    public class TatePairing : Pairing
    {
        EllipticCurve ec;
        EllipticCurve twisted;
        Field field;
        Field gt;
        BigInt order;
        BigInt finalExponent;
        BigInt cof;

        [DataMember]
        public BigInt Cofactor
        {
            get { return this.cof; }
        }

        /* (non-Javadoc)
         * @see uk.ac.ic.doc.jpbc.Pairing#getCurve()
         */
        [DataMember]
        public EllipticCurve Curve
        {
            get { return this.ec; }
        }

        /**
         * Return the twisted curve.
         */
        public EllipticCurve Curve2
        {
            get { return this.twisted; }
        }

        [DataMember]
        public BigInt GroupOrder
        {
            get { return this.order; }
        }

        public Field Gt
        {
            get { return this.gt; }
        }

        /**
          * Define the parameters for computing Tate paring 
          * @param curve the elliptic curve on which G1 is defined
          * @param groupOrder the order of G1
          * @param coFactor a BigInteger such that {@code coFactor*groupOrder = #E}, where #E is the number of points on the curve.
        */
        public TatePairing(EllipticCurve curve, BigInt groupOrder, BigInt coFactor)
        {
            this.ec = curve;
            this.field = curve.Field;
            this.order = groupOrder;
            //(p+1)/r
            this.finalExponent = (this.field.GetOrder().Add(BigInt.ONE)).Divide(groupOrder);
            this.cof = coFactor;
            this.twisted = Twist(curve);
            this.gt = new ComplexField((Fp)this.field);
        }

        public EllipticCurve Twist(EllipticCurve e)
        {
            Field f = e.Field;
            FieldElement b = f.Negate(e.B);
            return new EllipticCurve(f, e.A, b);
        }

        public FieldElement Compute(Point P, Point Q)
        {
            FieldElement f = new Complex((Fp)this.field, BigInt.ONE);
            JacobPoint V = this.ec.AToJ(P);
            BigInt n = this.order.Subtract(BigInt.ONE);
            Point nP = ec.Negate(P);


            //byte[] ba =n.toByteArray();
            sbyte[] r = Naf(this.order, (byte)2);
            //Point T;
            //BigInt [] lamda= new BigInt[1];;
            FieldElement u;
            for (int i = r.Length - 2; i >= 0; i--)
            {
                u = EncDbl(V, Q);
                //f=f.square().multiply(u);
                f = this.gt.Multiply(this.gt.Square(f), u);
                if (r[i] == 1)
                {
                    u = EncAdd(V, P, Q);
                    f = this.gt.Multiply(f, u);
                }
                if (r[i] == -1) //this is probably going to fail!
                {
                    u = EncAdd(V, nP, Q);
                    f = this.gt.Multiply(f, u);
                }
            }
            f = ((Complex)f).Conjugate().Divide((Complex)f);
            return this.gt.Pow(f, this.finalExponent);
        }

        //used by tate pairing, point doubling in Jacobian coordinates, and return the value of f 
        public Complex EncDbl(JacobPoint P, Point Q)
        {
            //if(P.isInfinity())
            //	return;

            BigInt x = P.X;
            BigInt y = P.Y;
            BigInt z = P.Z;
            //t1=y^2
            FieldElement t1 = this.field.Square(y);
            //t2=4xt1
            FieldElement t2 = this.field.Multiply(x, t1);
            //t2=this.field.multiply(t2, 4);

            t2 = this.field.Add(t2, t2);
            t2 = this.field.Add(t2, t2);

            //t3=8t1^2
            FieldElement t3 = this.field.Square(t1);
            //t3 = this.field.multiply(t3, 8);

            t3 = this.field.Add(t3, t3);
            t3 = this.field.Add(t3, t3);
            t3 = this.field.Add(t3, t3);

            //t4=z^2
            FieldElement t4 = this.field.Square(z);
            FieldElement t5;
            //if a==-3
            if (this.ec.Opt)
            {
                t5 = this.field.Multiply(this.field.Subtract(x, t4), this.field.Add(x, t4));
                t5 = this.field.Add(t5, this.field.Add(t5, t5));
            }
            else
            {
                //t5=3x^2+aZ^4		
                t5 = this.field.Square(x);
                t5 = this.field.Add(t5, this.field.Add(t5, t5));
                t5 = this.field.Add(t5, this.field.Multiply(this.ec.A4, this.field.Square(t4)));

                //		FieldElement temp =this.field.square(this.field.square(z));
                //		temp=this.field.multiply(this.ec.getA4(), temp);
                //		
                //		t5=this.field.add(t5,temp);
            }
            //x3=t5^2-2t2
            FieldElement x3 = this.field.Square(t5);
            x3 = this.field.Subtract(x3, this.field.Add(t2, t2));

            //y3=t5(t2-x3)-t3
            FieldElement y3 = this.field.Multiply(t5, this.field.Subtract(t2, x3));
            y3 = this.field.Subtract(y3, t3);

            //z3=2y1z1
            FieldElement z3 = this.field.Multiply(y, z);
            z3 = this.field.Add(z3, z3);

            P.X = (BigInt)x3;
            P.Y = (BigInt)y3;
            P.Z = (BigInt)z3;

            //Z3t4yQi-(2t1-t5(t4Xq+x1))
            FieldElement real = this.field.Multiply(t4, Q.X);
            real = this.field.Add(real, x);
            real = this.field.Multiply(t5, real);
            real = this.field.Subtract(real, t1);
            real = this.field.Subtract(real, t1);

            FieldElement imag = this.field.Multiply(z3, t4);
            imag = this.field.Multiply(imag, Q.Y);


            return new Complex((Fp)this.field, (BigInt)real, (BigInt)imag);

        }

        public Complex EncAdd(JacobPoint A, Point P, Point Q)
        {
            BigInt x1 = A.X;
            BigInt y1 = A.Y;
            BigInt z1 = A.Z;

            FieldElement x = P.X;
            FieldElement y = P.Y;

            //t1=z1^2
            FieldElement t1 = this.field.Square(z1);
            //t2=z1t1
            FieldElement t2 = this.field.Multiply(z1, t1);
            //t3=xt1
            FieldElement t3 = this.field.Multiply(x, t1);
            //t4=Yt2
            FieldElement t4 = this.field.Multiply(y, t2);
            //t5=t3-x1
            FieldElement t5 = this.field.Subtract(t3, x1);
            //t6=t4-y1
            FieldElement t6 = this.field.Subtract(t4, y1);
            //t7=t5^2
            FieldElement t7 = this.field.Square(t5);
            //t8=t5t7
            FieldElement t8 = this.field.Multiply(t5, t7);
            //t9=x1t7
            FieldElement t9 = this.field.Multiply(x1, t7);

            //x3=t6^2-(t8+2t9)
            FieldElement x3 = this.field.Square(t6);
            x3 = this.field.Subtract(x3, this.field.Add(t8, this.field.Add(t9, t9)));

            //y3=t6(t9-x3)-y1t8
            FieldElement y3 = this.field.Multiply(t6, this.field.Subtract(t9, x3));
            y3 = this.field.Subtract(y3, this.field.Multiply(y1, t8));

            //z3=z1t5
            FieldElement z3 = this.field.Multiply(z1, t5);

            A.X = (BigInt)x3;
            A.Y = (BigInt)y3;
            A.Z = (BigInt)z3;

            //z3yqi -(z3Y-t6(xq+x))
            FieldElement imag = this.field.Multiply(z3, Q.Y);

            FieldElement real = this.field.Add(Q.X, x);
            real = this.field.Multiply(real, t6);
            real = this.field.Subtract(real, this.field.Multiply(z3, y));

            return new Complex((Fp)this.field, (BigInt)real, (BigInt)imag);
        }

        //windowed naf form of BigInt k, w is the window size
        sbyte[] Naf(BigInt k, byte w)
        {

            // The window NAF is at most 1 element longer than the binary
            // representation of the integer k. byte can be used instead of short or
            // int unless the window width is larger than 8. For larger width use
            // short or int. However, a width of more than 8 is not efficient for
            // m = log2(q) smaller than 2305 Bits. Note: Values for m larger than
            // 1000 Bits are currently not used in practice.
            sbyte[] wnaf = new sbyte[k.BitLength() + 1];

            // 2^width as short and BigInteger
            short pow2wB = (short)(1 << w);
            BigInt pow2wBI = BigInt.ValueOf(pow2wB);

            int i = 0;

            // The actual length of the WNAF
            int length = 0;

            // while k >= 1
            while (k.Signum() > 0)
            {
                // if k is odd
                if (k.TestBit(0))
                {
                    // k mod 2^width
                    BigInt remainder = k.Mod(pow2wBI);

                    // if remainder > 2^(width - 1) - 1
                    if (remainder.TestBit(w - 1))
                    {
                        wnaf[i] = (sbyte)(remainder.IntValue() - pow2wB);
                    }
                    else
                    {
                        wnaf[i] = (sbyte)remainder.IntValue();
                    }
                    // wnaf[i] is now in [-2^(width-1), 2^(width-1)-1]

                    k = k.Subtract(BigInt.ValueOf(wnaf[i]));
                    length = i;
                }
                else
                {
                    wnaf[i] = 0;
                }

                // k = k/2
                k = k.ShiftRight(1);
                i++;
            }

            length++;

            // Reduce the WNAF array to its actual length
            sbyte[] wnafShort = new sbyte[length];
            Array.Copy(wnaf, 0, wnafShort, 0, length);
            return wnafShort;

        }

        public Point RandomPointInG1(Random rnd)
        {
            Point P;
            do
            {
                P = ec.RandomPoint(rnd);

                P = ec.Multiply(P, this.cof);

            } while (!ec.Multiply(P, this.order).Equals(Point.INFINITY));

            return P;

        }

        public Point RandomPointInG2(Random rnd)
        {
            return this.twisted.RandomPoint(rnd);
        }

    }
}
