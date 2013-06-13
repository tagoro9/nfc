using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using IBCS.Util;
using IBCS.Math;
using IBCS.EC;
using IBCS.Interfaces;

namespace IBCSTest.EC
{

    [TestClass]
    public class TatePairingTest
    {

        [TestMethod]
        public void BillinearTest()
        {
            //using a predefined pairing
            Pairing e = Predefined.nssTate();

            //get P, which is a random point in group G1
            BigInt xP = new BigInt("6489939838247988945871981900040296813593014269345452667904622838482534881503698483366430292883248221510827517646942621360563883893977498988822397615847035", 10);
            BigInt yP = new BigInt("4183869719038127850866054323652097154062917818644838834002147757841525900395398794914246812190615424669832349278263049648914095684913634131502517423626958", 10);
            Point P = new Point(xP, yP);

            //get Q, which is a random point in group G2
            BigInt xQ = new BigInt("2954273822533893703877488768696651085799471510788466547935020721095428933759242911655447662730930250257332528194261492443088328780492269310876627460775540", 10);
            BigInt yQ = new BigInt("5618108911398484778214611016375606480759218944436047801309324726445485710683489022960503194997792712985027904009701406998265308597942813750269945637462064", 10);
            Point Q = new Point(xQ, yQ);

            //compute e(P,Q)
            FieldElement epq = e.Compute(P, Q);

            //the curve on which G1 is defined
            EllipticCurve g1 = e.Curve;
            //a is a 160-bit random integer
            BigInt a = new BigInt(160, new Random());
            //Point aP is computed over G1
            Point aP = g1.Multiply(P, a);

            //The curve on which G2 is defined
            EllipticCurve g2 = e.Curve2;
            //b is a 160-bit random integer
            BigInt b = new BigInt(160, new Random());
            //bQ is computed over G2
            Point bQ = g2.Multiply(Q, b);

            //compute e(aP,bQ)
            FieldElement res = e.Compute(aP, bQ);

            //compute e(P,Q)^ab, this is done in group Gt
            BigInt ab = a.Multiply(b);
            //get the field on which Gt is defined
            Field gt = e.Gt;
            FieldElement res2 = gt.Pow(epq, ab);

            //compare these two
            Assert.AreEqual(res, res2);
        }

        [TestMethod]
        public void ComputeTest()
        {
            BigInt p = new BigInt("bffffffffffffffffffffffffffcffff3", 16);
            BigInt q = new BigInt("fffffffffffffffffffffffffffbffff", 16);
            BigInt cofactor = p.Add(BigInt.ONE);
            Fp field = new Fp(p);
            EllipticCurve curve = new EllipticCurve(field, BigInt.ZERO, BigInt.ONE);
            Pairing pairing = new TatePairing(curve, q, cofactor);
            BigInt xA = new BigInt("489a03c58dcf7fcfc97e99ffef0bb4634", 16);
            BigInt yA = new BigInt("510c6972d795ec0c2b081b81de767f808", 16);
            BigInt xB = new BigInt("40e98b9382e0b1fa6747dcb1655f54f75", 16);
            BigInt yB = new BigInt("b497a6a02e7611511d0db2ff133b32a3f", 16);
            Point a = new Point(xA, yA);
            Point b = new Point(xB, yB);
            FieldElement result = pairing.Compute(a, b);
            BigInt xE = new BigInt("18901869995136711856104482098716151434", 10);
            BigInt yE = new BigInt("3528981068833319305513374283552333670736", 10);
            FieldElement expected = new Complex(field, xE, yE);
            Assert.AreEqual(expected, result);
        }
    }
}
