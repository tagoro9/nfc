using System;
using IBCS.Math;

namespace IBCS.Interfaces
{
    public interface Field
    {
        /// <summary>
        /// return val1 + val2
        /// </summary>
        /// <param name="val1"></param>
        /// <param name="val2"></param>
        /// <returns></returns>
        FieldElement Add(FieldElement val1, FieldElement val2);

        /// <summary>
        /// return 1 / val
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        FieldElement Inverse(FieldElement val);

        /// <summary>
        /// return val * val2
        /// </summary>
        /// <param name="val1"></param>
        /// <param name="val2"></param>
        /// <returns></returns>
        FieldElement Multiply(FieldElement val1, FieldElement val2);

        /// <summary>
        /// return val1 / val2
        /// </summary>
        /// <param name="val1"></param>
        /// <param name="val2"></param>
        /// <returns></returns>
        FieldElement Divide(FieldElement val1, FieldElement val2);

        /// <summary>
        /// return val1 - val2
        /// </summary>
        /// <param name="val1"></param>
        /// <param name="val2"></param>
        /// <returns></returns>
        FieldElement Subtract(FieldElement val1, FieldElement val2);

        /// <summary>
        /// return a random element in the field
        /// </summary>
        /// <param name="rnd"></param>
        /// <returns></returns>
        FieldElement RandomElement(Random rnd);

        /// <summary>
        /// Return the characteristics of this field
        /// </summary>
        /// <returns></returns>
        BigInt GetP();

        /// <summary>
        /// return true if valid elment in the field, otherwise, return false
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        bool IsValidElement(FieldElement val);

        /// <summary>
        /// return val1*val2
        /// </summary>
        /// <param name="val"></param>
        /// <param name="val2"></param>
        /// <returns></returns>
        FieldElement Multiply(FieldElement val, int val2);

        /// <summary>
        /// return -val1
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        FieldElement Negate(FieldElement val);

        /// <summary>
        /// return 1
        /// </summary>
        /// <returns></returns>
        FieldElement GetOne();

        /// <summary>
        /// return 0
        /// </summary>
        /// <returns></returns>
        FieldElement GetZero();

        /// <summary>
        /// determine if val == 1
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        bool IsOne(FieldElement val);

        /// <summary>
        /// determine if val == 0
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        bool IsZero(FieldElement val);

        /// <summary>
        /// determine the square of val
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        FieldElement Square(FieldElement val);

        /// <summary>
        /// determine the square root of val
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        FieldElement SquareRoot(FieldElement val);

        /// <summary>
        /// return the order of the field
        /// </summary>
        /// <returns></returns>
        BigInt GetOrder();

        /// <summary>
        /// return val^exp
        /// </summary>
        /// <param name="val"></param>
        /// <param name="exp"></param>
        /// <returns></returns>
        FieldElement Pow(FieldElement val, BigInt exp);

    }
}
