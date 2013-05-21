using System;
using IBCS.Math;
using IBCS.EC;

namespace IBCS.Interfaces
{
    public interface Pairing
    {
        /**
         * Compute pairing.
         * @param P a point from group G1.
         * @param Q a point from group G2.
         * @return an element from Gt which represent the result of e(P,Q).
         */
        FieldElement Compute(Point P, Point Q);

        /**
         * Get the cofactor of the Elliptic curve on which G1 is defined. If the order of G1 is {@code q} and the order of the curve is {@code #E}, then the cofactor = {@code #E/q}.
         * @return the cofactor
         */
        BigInt Cofactor { get; }

        /**
         * Get the elliptic curve on which G1 is defined.
         * @return the Elliptic curve
         */
        EllipticCurve Curve { get; }

        /**
         * Get the elliptic curve on which G2 is defined.
         * @return the Elliptic curve
         */
        EllipticCurve Curve2 { get; }

        /**
         * Get a random element in G1. 
         * @return a random element in G1
         */
        Point RandomPointInG1(Random rnd);

        /**
         * Get a random element in G2. 
         * @return a random element in G2
         */
        Point RandomPointInG2(Random rnd);

        /**
         * Get the order of G1.Usually it is a large prime number. 
         * @return the order of G1.
         */
        BigInt GroupOrder { get; }
        /**
         * Get the extension field where the group Gt is taken.
         * @return the extension field
         */
        Field Gt { get; }
    }
}
