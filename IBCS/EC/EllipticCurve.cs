using System;
using IBCS.Math;
using IBCS.Interfaces;
using IBCS.Util;
using System.Runtime.Serialization;

namespace IBCS.EC
{
    [DataContract]
    public class EllipticCurve
    {
        private Field field;
        private FieldElement a1;
        private FieldElement a3;
        private FieldElement a2;
        private FieldElement a4;
        private FieldElement a6;
        private bool opt;

        public bool Opt
        {
            get { return opt; }
        }

        [DataMember]
        public Field Field
        {
            get { return field; }
        }

        /**
         *  Get the coefficient a of the simplified Weierstrass equation 
         * Y^2=X^3+aX+b;
         * @return the coefficient a.
         */

        [DataMember]
        public FieldElement A
        {
            get
            {
                if (this.field is Fp)
                {
                    return this.a4;

                }
                else
                {
                    return this.a2;
                }
            }
        }

        /**
         *  Get the coefficient b of the simplified Weierstrass equation 
         * Y^2=X^3+aX+b;
         * @return the coefficient b.
         */
        [DataMember]
        public FieldElement B
        {

            get
            {
                return this.a6;
            }

        }

        /**
         *  Get the coefficient A1 of the simplified Weierstrass equation 
         * Y^2+a_1XY+a_3Y=X^3+a_2X^2+a_4X+a_6.
         * @return the coefficient A1.
         */

        public FieldElement getA1()
        {
            return this.a1;
        }

        /**
         *  Get the coefficient a2 of the simplified Weierstrass equation 
         * Y^2+a_1XY+a_3Y=X^3+a_2X^2+a_4X+a_6.
         * @return the coefficient a2.
         */

        public FieldElement A2
        {
            get
            {
                return this.a2;
            }
        }
        /**
         *  Get the coefficient a3 of the simplified Weierstrass equation 
         * Y^2+a_1XY+a_3Y=X^3+a_2X^2+a_4X+a_6.
         * @return the coefficient a3.
         */
        public FieldElement A3
        {
            get
            {
                return this.a3;
            }
        }

        /**
         *  Get the coefficient a4 of the simplified Weierstrass equation 
         * Y^2+a_1XY+a_3Y=X^3+a_2X^2+a_4X+a_6.
         * @return the coefficient a4.
         */

        public FieldElement A4
        {
            get
            {
                return this.a4;
            }
        }
        /**
         *  Get the coefficient a6 of the simplified Weierstrass equation 
         * Y^2+a_1XY+a_3Y=X^3+a_2X^2+a_4X+a_6.
         * @return the coefficient a6.
         */
        public FieldElement A6
        {
            get
            {
                return this.a6;
            }
        }

        /**
        * Creates an elliptic curve with the specified finite field
        * <code>field</code> and the coefficients <code>a1,a3,a2,a4,a6</code>.
        * @param field the finite field that this elliptic curve is over.
        * @param a1 the coefficient of term XY.
        * @param a3 the coefficient of term Y.
        * @param a2 the coefficient of term X^2.
        * @param a4 the coefficient of term X.
        * @param a6 the constant term.
        * @exception NullPointerException if <code>field</code>,
        * or any one of<code>a1,a3,a2,a4,a6</code> is null.
        * @exception IllegalArgumentException if any one of 
        * <code>a1,a3,a2,a4,a6</code> is not in <code>field</code>.
        */
        public EllipticCurve(Field field, FieldElement a1, FieldElement a2, FieldElement a3, FieldElement a4, FieldElement a6)
        {
            if (field == null)
                throw new NullReferenceException("Field is null");
            if (a1 == null)
                throw new NullReferenceException("a1 is null");
            if (a3 == null)
                throw new NullReferenceException("a3 is null");
            if (a2 == null)
                throw new NullReferenceException("a2 is null");
            if (a4 == null)
                throw new NullReferenceException("a4 is null");
            if (a6 == null)
                throw new NullReferenceException("a6 is null");

            if (!field.IsValidElement(a1))
                throw new ArgumentException("a1 is not an element in the field");
            if (!field.IsValidElement(a3))
                throw new ArgumentException("a3 is not an element in the field");
            if (!field.IsValidElement(a2))
                throw new ArgumentException("a2 is not an element in the field");
            if (!field.IsValidElement(a4))
                throw new ArgumentException("a4 is not an element in the field");
            if (!field.IsValidElement(a6))
                throw new ArgumentException("a6 is not an element in the field");

            Initialise(field, a1, a3, a2, a4, a6);
            if (a4.Equals(this.field.Negate(BigInt.ValueOf(3))))
                this.opt = true;
            else
                this.opt = false;
        }


        /**
         * Creates an elliptic curve with the specified finite field
         * <code>field</code> and the coefficients <code>a,b</code>.
         * <code>a,b</code> are coefficients for the simplified Weierstrass equations.
         * the simplified Weierstrass equation is
         * Y^2=X^3+aX+b;
         * @param field the finite field that this elliptic curve is over.
         * @param a the first coefficient.
         * @param b the second coefficient.
         * @exception NullPointerException if <code>field</code>,
         * or any one of<code>a,b</code> is null.
         * @exception IllegalArgumentException if any one of 
         * <code>a,b</code> is not in <code>field</code>.
         */

        public EllipticCurve(Field field, FieldElement a, FieldElement b)
        {
            if (field == null)
                throw new NullReferenceException("field is null");
            if (a == null)
                throw new NullReferenceException("a is null");
            if (b == null)
                throw new NullReferenceException("b is null");

            if (!(field).IsValidElement(a))
                throw new ArgumentException("a is not an element in the field");
            if (!field.IsValidElement(b))
                throw new ArgumentException("b is not an element in the field");

            if (field is Fp)
            {
                Initialise(field, BigInt.ZERO, BigInt.ZERO, BigInt.ZERO, a, b);
            }

            if (a4.Equals(this.field.Negate(BigInt.ValueOf(3))))
                this.opt = true;
            else
                this.opt = false;
        }

        private void Initialise(Field field, FieldElement a1, FieldElement a3, FieldElement a2, FieldElement a4, FieldElement a6)
        {
            this.field = field;
            this.a1 = a1;
            this.a3 = a3;
            this.a2 = a2;
            this.a4 = a4;
            this.a6 = a6;
        }

        /**
         * Point addition. Returns a new Point whose value is {@code p1 + p2}.  
         *
         * @param p1 first point.
         * @param p2 second point.
         * @return {@code p1 + p2}
         * @exception IllegalArgumentException if the x- or y-coordinate of p1 or p2 is not valid field element.
         */
        public Point Add(Point p1, Point p2)
        {
            return BasicAdd(p1, p2);
        }

        private Point BasicAdd(Point p1, Point p2)
        {
            //if either one is INFINITY, return the other point
            if (p1.IsInfinity())
                return p2;
            else if (p2.IsInfinity())
                return p1;


            //check inputs
            if (!(this.field.IsValidElement(p1.X)) || !(this.field.IsValidElement(p1.Y)) || !(this.field.IsValidElement(p2.X)) || !(this.field.IsValidElement(p2.Y)))
                throw new ArgumentException("The input points must be taken over the field.");

            //x2=x1
            if (p1.X.Equals(p2.X))
            {
                //p1 = p2
                if (p1.Y.Equals(p2.Y))
                    return Dbl(p1);
                //p1 != p2
                else
                    return Point.INFINITY;
            }

            //lamda=(y2-y1)/(x2-x1)
            FieldElement top = this.field.Subtract(p2.Y, p1.Y);
            FieldElement bottom = this.field.Subtract(p2.X, p1.X);
            FieldElement lamda = this.field.Multiply(top, this.field.Inverse(bottom));


            //x3=lamda^2+a1lamda-a2-x1-x2

            FieldElement x3 = this.field.Square(lamda);
            //x3=this.field.add(x3, this.field.multiply(a1, lamda));
            //x3=this.field.substract(x3, a2);
            x3 = this.field.Subtract(x3, p1.X);
            x3 = this.field.Subtract(x3, p2.X);

            //y3=lamdax1-y1-a1x3-a3-lamdax3
            //y3=lamdax1-y1-a1x3-a3-lamdax3
            FieldElement y3 = this.field.Multiply(lamda, p1.X);
            y3 = this.field.Subtract(y3, p1.Y);
            //y3=this.field.substract(y3, this.field.multiply(a1, x3));
            //y3=this.field.substract(y3, a3);
            y3 = this.field.Subtract(y3, this.field.Multiply(lamda, x3));

            return new Point(x3, y3);
        }

        public Point Dbl(Point p1)
        {
            return BasicDbl(p1);
        }

        private Point BasicDbl(Point p1)
        {
            // TODO Auto-generated method stub
            if (p1.IsInfinity())
                return p1;

            if (!(this.field.IsValidElement(p1.X)) || !(this.field.IsValidElement(p1.Y))
                )
                throw new ArgumentException("The input point must be taken over the field.");
            //lamda = (3x^2+2a2x+a4-a1y)/(2y1+a1x+a3)
            FieldElement bottom = this.field.Multiply(p1.Y, 2);
            //bottom=this.field.add(bottom, this.field.multiply(a1, p1.getX()));
            //bottom=this.field.add(bottom, a3);

            if (this.field.IsZero(bottom))
                return Point.INFINITY;
            FieldElement top = this.field.Multiply(this.field.Square(p1.X), 3);
            //top=this.field.add(top, this.field.multiply(this.field.multiply(a2, p1.getX()), 2));
            top = this.field.Add(top, a4);
            //top=this.field.substract(top, this.field.multiply(a1, p1.getY()));

            FieldElement lamda = this.field.Multiply(top, this.field.Inverse(bottom));

            //x3= lamda^2+lamdaa1-a2-2x
            FieldElement x3 = this.field.Square(lamda);
            x3 = this.field.Add(x3, this.field.Multiply(lamda, a1));
            //x3=this.field.substract(x3, a2);
            x3 = this.field.Subtract(x3, this.field.Multiply(p1.X, 2));

            //y3=lamdax-y-a1x3-a3-lamdax3
            FieldElement y3 = this.field.Multiply(lamda, p1.X);
            y3 = this.field.Subtract(y3, p1.Y);
            //y3=this.field.substract(y3, this.field.multiply(a1, x3));
            //y3=this.field.substract(y3, a3);
            y3 = this.field.Subtract(y3, this.field.Multiply(lamda, x3));

            return new Point(x3, y3);
        }

        public Point Negate(Point p1)
        {
            return BasicNegate(p1);
        }

        private Point BasicNegate(Point p1)
        {
            // TODO Auto-generated method stub
            if (p1.IsInfinity())
                return p1;

            if (!(this.field.IsValidElement(p1.X)) || !(this.field.IsValidElement(p1.Y))
                )
                throw new ArgumentException("The input point must be taken over the field.");
            //x2=x1;
            //y2=-(a1x1+a3+y1)
            //FieldElement y2=this.field.add(this.field.multiply(a1, p1.getX()), a3);
            //y2=this.field.add(y2, p1.getY());
            FieldElement y2 = this.field.Negate(p1.Y);

            return new Point(p1.X, y2);
        }

        public Point Subtract(Point p1, Point p2)
        {
            if (p1.IsInfinity())
                return Negate(p2);
            if (p2.IsInfinity())
                return p1;
            return Add(p1, Negate(p2));
        }

        public Point Multiply(Point p, BigInt k)
        {
            return this.JToA(JMultiplyMut(p, k));
        }

        private Point SimpleMultiply(Point p, BigInt k)
        {

            if (!(this.field.IsValidElement(p.X)) || !(this.field.IsValidElement(p.Y))
            )
                throw new ArgumentException("The input point must be taken over the field.");

            if (p.IsInfinity())
                return p;
            if (k.Equals(BigInt.ZERO))
                return Point.INFINITY;
            if (k.Equals(BigInt.ONE))
                return p;


            if (k.Signum() == -1)
            {
                k = k.Abs();
                p = this.Negate(p);
            }

            sbyte[] ba = k.ToByteArray();

            int degree = ByteArrayUtil.DegreeOf(ba) - 1;

            Point x = p;

            for (int i = degree; i >= 0; i--)
            {
                x = this.Dbl(x);
                if (ByteArrayUtil.GetBitByDegree(i, ba))
                    x = this.Add(p, x);
            }
            return x;
        }

        public Point GetPoint(FieldElement x)
        {
            //
            //FieldElement temp= this.field.pow(x, BigInt.valueOf(3));
            FieldElement temp = this.field.Multiply(x, x);
            temp = this.field.Multiply(temp, x);
            temp = this.field.Add(temp, this.field.Multiply(this.A, x));
            temp = this.field.Add(temp, this.B);

            FieldElement sqr = this.field.SquareRoot(temp);

            if (sqr != null)
            {

                return new Point(x, sqr);
            }
            return null;
        }

        private JacobPoint Negate(JacobPoint p)
        {
            return new JacobPoint(p.X, (BigInt)this.field.Negate(p.Y), p.Z);
        }

        //multiplication using Jacobian coordinates
        public JacobPoint JMultiplyMut(Point p, BigInt k)
        {
            if (!(this.field.IsValidElement(p.X)) || !(this.field.IsValidElement(p.Y)))
                throw new ArgumentException("The input point must be taken over the field.");

            if (p.IsInfinity())
                return this.AToJ(p);
            if (k.Equals(BigInt.ZERO))
                return JacobPoint.INFINITY;
            if (k.Equals(BigInt.ONE))
                return this.AToJ(p);


            if (k.Signum() == -1)
            {
                k = k.Abs();
                p = this.Negate(p);
            }

            //byte [] ba =k.toByteArray();

            int degree = k.BitLength() - 2;

            JacobPoint result = this.AToJ(p);

            for (int i = degree; i >= 0; i--)
            {
                this.JDblMut(result);
                if (k.TestBit(i)) ///AQUI TE QUEDASTE IMPLEMENTAR TESTBIT
                    this.JAddMut(result, p);
            }
            return result;

        }

        /**
         * Test whether the input point is on this curve.   
         *
         * @param p the point to be tested.
         * @return {@code true} if the point is on the curve, {@code false} otherwise
         */

        public bool IsOnCurve(Point p)
        {
            if (p.IsInfinity())
                return true;


            FieldElement x = p.X;
            FieldElement y = p.Y;
            if (!(this.field.IsValidElement(x)) || !(this.field.IsValidElement(y))
            )
                return false;
            //left hand side y^2+a_1xy+a_3y

            FieldElement lhs = this.field.Square(y);
            lhs = this.field.Add(lhs, this.field.Multiply(this.field.Multiply(a1, x), y));
            lhs = this.field.Add(lhs, this.field.Multiply(a3, y));



            //right hand side x^3+a_2x^2+a_4x+a_6
            FieldElement x2 = this.field.Square(x);
            FieldElement rhs = this.field.Multiply(x, x2);
            rhs = this.field.Add(rhs, this.field.Multiply(a2, x2));
            rhs = this.field.Add(rhs, this.field.Multiply(a4, x));
            rhs = this.field.Add(rhs, a6);

            return lhs.Equals(rhs);
        }

        public Point RandomPoint(Random r)
        {
            return RandomPointP(r);
        }

        //y^2=x^3+ax+b
        private Point RandomPointP(Random r)
        {
            FieldElement x;
            FieldElement y;

            do
            {
                x = this.field.RandomElement(r);


                //temp =x^3+ax+b
                //FieldElement temp= this.field.pow(x, BigInt.valueOf(3));
                FieldElement temp = this.field.Multiply(x, x);
                temp = this.field.Multiply(x, temp);
                temp = this.field.Add(temp, this.field.Multiply(this.A, x));
                temp = this.field.Add(temp, this.B);

                FieldElement sqr = this.field.SquareRoot(temp);

                if (sqr != null)
                {
                    //BigInteger negB=this.field.negate(b);
                    if (r.Next(0, 2) == 1)
                    {
                        y = sqr;
                    }
                    else
                    {
                        y = this.field.Negate(sqr);
                    }

                    return new Point(x, y);

                }
            } while (true);
        }


        //point doubling in Jacobian coordinates, result is saving in the input point
        public void JDblMut(JacobPoint P)
        {
            if (P.IsInfinity())
                return;

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
            if (opt)
            {
                t5 = this.field.Multiply(this.field.Subtract(x, t4), this.field.Add(x, t4));
                t5 = this.field.Add(t5, this.field.Add(t5, t5));
            }
            else
            {
                //t5=3x^2+aZ^4	=3x^2+at4^2	
                t5 = this.field.Square(x);
                t5 = this.field.Add(t5, this.field.Add(t5, t5));
                t5 = this.field.Add(t5, this.field.Multiply(this.A4, this.field.Square(t4)));
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
            return;

        }

        //add two point, save result in the first argument
        public void JAddMut(JacobPoint P, Point Q)
        {
            if (P.IsInfinity())
            {
                P.X = (BigInt)Q.X;
                P.Y = (BigInt)Q.Y;
                P.Z = (BigInt)this.field.GetOne();
                return;
            }
            if (Q.IsInfinity())
                return;


            FieldElement x1 = P.X;
            FieldElement y1 = P.Y;
            FieldElement z1 = P.Z;

            FieldElement x = Q.X;
            FieldElement y = Q.Y;

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

            P.X = (BigInt)x3;
            P.Y = (BigInt)y3;
            P.Z = (BigInt)z3;
            return;
        }

        // convert Jacobian to Affine	
        public Point JToA(JacobPoint P)
        {
            if (P.IsInfinity())
                return Point.INFINITY;
            FieldElement bi = P.Z;
            FieldElement zInverse = this.field.Inverse(P.Z);
            FieldElement square = this.field.Square(zInverse);
            //x =X/Z^2
            FieldElement x = this.field.Multiply(P.X, square);
            //y=Y/Z^3
            FieldElement y = this.field.Multiply(P.Y, this.field.Multiply(square, zInverse));

            return new Point(x, y);
        }
        //convert Affine to Jacobian
        public JacobPoint AToJ(Point P)
        {
            return new JacobPoint((BigInt)P.X, (BigInt)P.Y, BigInt.ONE);
        }

        /**
         * Return a random base point (generator of an order-q subgroup). cof*q must equal to the number of points on the curve. Otherwise it will loop
         * forever.
         * @param rnd the source of randomness
         * @param q the order of the subgroup (the order of the base point)
         * @param cof the cofactor
         * @return a base point 
         */

        public Point GetBasePoint(Random rnd, BigInt q, BigInt cof)
        {
            Point p;
            do
            {
                p = this.RandomPoint(rnd);
                p = this.Multiply(p, cof);
            } while (!p.Equals(Point.INFINITY) && !this.Multiply(p, q).Equals(Point.INFINITY));

            return p;
        }

    }
}
