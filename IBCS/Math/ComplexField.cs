using System;
using IBCS.Interfaces;

namespace IBCS.Math
{
    public class ComplexField : Field
    {
        private Fp field;

        public ComplexField(Fp baseField)
        {
            this.field = baseField;
        }

        public FieldElement Add(FieldElement val1, FieldElement val2)
        {
            if (!(val1 is Complex) || (!(val2 is Complex)))
                throw new ArgumentException("The inputs must be Complex numbers");
            return ((Complex)val1).Add((Complex)val2);
        }

        public FieldElement Divide(FieldElement val1, FieldElement val2)
        {
            if (!(val1 is Complex) || (!(val2 is Complex)))
                throw new ArgumentException("The inputs must be Complex numbers");
            if (((Complex)val2).IsZero())
                throw new ArgumentException("Divide by Zero");
            return ((Complex)val1).Divide((Complex)val2);
        }

        public FieldElement GetOne()
        {
            return new Complex(this.field, BigInt.ONE);
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
            return new Complex(this.field, BigInt.ZERO);
        }

        public FieldElement Inverse(FieldElement val)
        {
            if (!(val is Complex))
                throw new ArgumentException("The input must be a Complex number");
            if (((Complex)val).IsZero())
                throw new ArgumentException("Zero cannot be inversed");
            return ((Complex)val).Inverse();
        }

        public bool IsOne(FieldElement val)
        {
            if (val is Complex)
                return ((Complex)val).IsOne();
            else
                return false;
        }

        public bool IsValidElement(FieldElement val)
        {
            if (val is Complex)
                return ((Complex)val).Field.Equals(this.field);
            else
                return false;
        }

        public bool IsZero(FieldElement val)
        {
            if (val is Complex)
                return ((Complex)val).IsZero();
            else
                return false;
        }

        public FieldElement Multiply(FieldElement val1, FieldElement val2)
        {
            if (!(val1 is Complex) || (!(val2 is Complex)))
                throw new ArgumentException("The inputs must be Complex numbers");
            return ((Complex)val1).Multiply((Complex)val2);
        }

        public FieldElement Multiply(FieldElement val, int val2)
        {
            if (!(val is Complex))
                throw new ArgumentException("The first input must be a Complex number");
            return ((Complex)val).Multiply(val2);
        }

        public FieldElement Negate(FieldElement val)
        {
            if (!(val is Complex))
                throw new ArgumentException("The inputsmust be a Complex number");
            return ((Complex)val).Negate();
        }

        public FieldElement Pow(FieldElement val, BigInt exp)
        {
            if (!(val is Complex))
                throw new ArgumentException("The first input must be a Complex number");
            return ((Complex)val).Pow((BigInt)exp);
        }

        public FieldElement RandomElement(Random rnd)
        {
            return new Complex(this.field, (BigInt)this.field.RandomElement(rnd), (BigInt)this.field.RandomElement(rnd));
        }

        public FieldElement Square(FieldElement val)
        {
            if (!(val is Complex))
                throw new ArgumentException("The inputsmust be a Complex number");
            return ((Complex)val).Square();
        }

        public FieldElement SquareRoot(FieldElement val)
        {
            if (!(val is Complex))
                throw new ArgumentException("The inputsmust be a Complex number");
            throw new NotImplementedException();
        }

        public FieldElement Subtract(FieldElement val1, FieldElement val2)
        {
            if (!(val1 is Complex) || (!(val2 is Complex)))
                throw new ArgumentException("The inputs must be Complex numbers");
            return ((Complex)val1).Subtract((Complex)val2);
        }
    }
}
