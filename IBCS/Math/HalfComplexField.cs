using System;
using IBCS.Interfaces;

namespace IBCS.Math
{
    public class HalfComplexField : Field
    {
        private Fp field;
        public Fp Field
        {
            get { return field; }
        }

        public HalfComplexField(Fp baseField)
        {
            this.field = baseField;
        }

        public FieldElement Add(FieldElement val1, FieldElement val2)
        {
            if (!(val1 is HalfComplex) || (!(val2 is HalfComplex)))
                throw new ArgumentException("The inputs must be HalfComplex numbers");
            return ((HalfComplex)val1).Add((HalfComplex)val2);
        }

        public FieldElement Divide(FieldElement val1, FieldElement val2)
        {
            throw new ArithmeticException("Division in a HalfCompressedField is not supported!");
        }

        public FieldElement GetOne()
        {
            return new HalfComplex(this.field, (BigInt)this.field.GetOne());
        }

        public BigInt GetOrder()
        {
            return this.field.GetOrder().Multiply(this.field.GetOrder());
        }

        public BigInt GetP()
        {
            return this.field.GetP();
        }

        public FieldElement GetZero()
        {
            return new HalfComplex(this.field, (BigInt)this.field.GetZero());
        }

        public FieldElement Inverse(FieldElement val)
        {
            throw new ArithmeticException("Inversion in a HalfCompressedField is not supported!");
        }

        public bool IsOne(FieldElement val)
        {
            throw new ArithmeticException("isOne in a HalfCompressedField is not supported!");
        }

        public bool IsValidElement(FieldElement val)
        {
            return (val is HalfComplex) && ((HalfComplex)val).Field.Equals(this.field);
        }

        public bool IsZero(FieldElement val)
        {
            throw new ArithmeticException("isZero in a HalfCompressedField is not supported!");
        }

        public FieldElement Multiply(FieldElement val1, FieldElement val2)
        {
            HalfComplex c = null;
            BigInt i = null;
            if (val1 is HalfComplex)
            {
                if (val2 is HalfComplex)
                {
                    throw new ArgumentException("The inputs cannot be both HalfComplex numbers");
                }
                if (val2 is BigInt)
                {
                    c = (HalfComplex)val1;
                    i = (BigInt)val2;
                }
                else
                    throw new ArgumentException("The second input is of a unsupported type");
            }

            if (val2 is HalfComplex)
            {
                if (val1 is HalfComplex)
                {
                    throw new ArgumentException("The inputs cannot be both HalfComplex numbers");
                }
                if (val1 is BigInt)
                {
                    c = (HalfComplex)val2;
                    i = (BigInt)val1;
                }
                else
                    throw new ArgumentException("The first input is of a unsupported type");
            }

            return c.Multiply(i);
        }

        public FieldElement Multiply(FieldElement val, int val2)
        {
            if (!(val is HalfComplex))
                throw new ArgumentException("The first input must be of a HalfComplex");
            return ((HalfComplex)val).Multiply(val2);
        }

        public FieldElement Negate(FieldElement val)
        {
            if (!(val is HalfComplex))
                throw new ArgumentException("The input must be of a HalfComplex");
            return ((HalfComplex)val).Negate();
        }

        public FieldElement Pow(FieldElement val, BigInt exp)
        {
            if (!(val is HalfComplex))
                throw new ArgumentException("The input must be of a HalfComplex");
            return ((HalfComplex)val).Pow(exp);
        }

        public FieldElement RandomElement(Random rnd)
        {
            return new HalfComplex(this.field, (BigInt)this.field.RandomElement(rnd));
        }

        public FieldElement Square(FieldElement val)
        {
            if (!(val is HalfComplex))
                throw new ArgumentException("The input must be of a HalfComplex");
            return ((HalfComplex)val).Square();
        }

        public FieldElement SquareRoot(FieldElement val)
        {
            throw new ArithmeticException("squareRoot in a HalfCompressedField is not supported!");
        }

        public FieldElement Subtract(FieldElement val1, FieldElement val2)
        {
            if (!(val1 is HalfComplex) || (!(val2 is HalfComplex)))
                throw new ArgumentException("The inputs must be HalfComplex numbers");
            return ((HalfComplex)val1).Subtract((HalfComplex)val2);
        }
    }
}

