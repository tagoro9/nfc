using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using IBCS.Interfaces;
using IBCS.Math;
using IBCS.EC;

namespace TestsApp.Tests.EC
{
    [TestClass]
    public class EllipticCurveTest
    {

        EllipticCurve curve;

        [TestInitialize]
        public void InitialiseCurve()
        {
            BigInt p = new BigInt("bffffffffffffffffffffffffffcffff3", 16);
            Fp field = new Fp(p);
            curve = new EllipticCurve(field, BigInt.ZERO, BigInt.ONE);
        }

        [TestMethod]
        public void PointAddTest()
        {
            BigInt xA = new BigInt("489a03c58dcf7fcfc97e99ffef0bb4634", 16);
            BigInt yA = new BigInt("510c6972d795ec0c2b081b81de767f808", 16);
            BigInt xB = new BigInt("417b9d98f934b571bffaed8d2968f6d67", 16);
            BigInt yB = new BigInt("6339a332da64ca233033eace4d6bdafdf", 16);

            Point a = new Point(xA, yA);
            Point b = new Point(xB, yB);
            Point r = curve.Add(a, b);
            BigInt xE = new BigInt("4e429ac21600f83ae575eda41ccbd72c5", 16);
            BigInt yE = new BigInt("14eb50532fc0f26c9e1328bbfe5747322", 16);
            Point e = new Point(xE, yE);
            Assert.AreEqual(e, r);
        }

        [TestMethod]
        public void PointDoubleTest()
        {
            BigInt xA = new BigInt("489a03c58dcf7fcfc97e99ffef0bb4634", 16);
            BigInt yA = new BigInt("510c6972d795ec0c2b081b81de767f808", 16);
            BigInt xE = new BigInt("417b9d98f934b571bffaed8d2968f6d67", 16);
            BigInt yE = new BigInt("6339a332da64ca233033eace4d6bdafdf", 16);
            Point a = new Point(xA, yA);
            Point e = new Point(xE, yE);
            Assert.AreEqual(e, curve.Dbl(a));
        }

        [TestMethod]
        public void NegateTest()
        {
            BigInt xA = new BigInt("489a03c58dcf7fcfc97e99ffef0bb4634", 16);
            BigInt yA = new BigInt("510c6972d795ec0c2b081b81de767f808", 16);
            BigInt xE = new BigInt("489a03c58dcf7fcfc97e99ffef0bb4634", 16);
            BigInt yE = new BigInt("6ef3968d286a13f3d4f7e47e2186807eb", 16);
            Point a = new Point(xA, yA);
            Point e = new Point(xE, yE);
            Assert.AreEqual(e, curve.Negate(a));
        }

        [TestMethod]
        public void SubtractTest()
        {
            BigInt xA = new BigInt("489a03c58dcf7fcfc97e99ffef0bb4634", 16);
            BigInt yA = new BigInt("510c6972d795ec0c2b081b81de767f808", 16);
            BigInt xB = new BigInt("417b9d98f934b571bffaed8d2968f6d67", 16);
            BigInt yB = new BigInt("6339a332da64ca233033eace4d6bdafdf", 16);
            Point a = new Point(xA, yA);
            Point b = new Point(xB, yB);
            Assert.AreEqual(a, curve.Subtract(b, a));

        }

        [TestMethod]
        public void MultiplyTest()
        {
            BigInt xA = new BigInt("489a03c58dcf7fcfc97e99ffef0bb4634", 16);
            BigInt yA = new BigInt("510c6972d795ec0c2b081b81de767f808", 16);
            BigInt l = new BigInt("b8bbbc0089098f2769b32373ade8f0daf", 16);
            BigInt xE = new BigInt("073734b32a882cc97956b9f7e54a2d326", 16);
            BigInt yE = new BigInt("9c4b891aab199741a44a5b6b632b949f7", 16);
            Point e = new Point(xE, yE);
            Point a = new Point(xA, yA);
            Assert.AreEqual(e, curve.Multiply(a, l));
        }

        [TestMethod]
        public void JMultiplyTest()
        {
            BigInt xA = new BigInt("489a03c58dcf7fcfc97e99ffef0bb4634", 16);
            BigInt yA = new BigInt("510c6972d795ec0c2b081b81de767f808", 16);
            BigInt l = new BigInt("b8bbbc0089098f2769b32373ade8f0daf", 16);
            BigInt xE = new BigInt("073734b32a882cc97956b9f7e54a2d326", 16);
            BigInt yE = new BigInt("9c4b891aab199741a44a5b6b632b949f7", 16);
            Point e = new Point(xE, yE);
            Point a = new Point(xA, yA);
            Assert.AreEqual(e, curve.JToA(curve.JMultiplyMut(a, l)));
        }

        [TestMethod]
        public void IsOnCurveTest()
        {
            BigInt xA = new BigInt("489a03c58dcf7fcfc97e99ffef0bb4634", 16);
            BigInt yA = new BigInt("510c6972d795ec0c2b081b81de767f808", 16);
            Point a = new Point(xA, yA);
            BigInt xB = new BigInt("489a03c58dcf7fcfc91239ffef0bb4621", 16);
            BigInt yB = new BigInt("510c6972d795ec0ca5081b81de767f808", 16);
            Point b = new Point(xB, yB);
            Assert.IsTrue(curve.IsOnCurve(a));
            Assert.IsFalse(curve.IsOnCurve(b));
        }

        [TestMethod]
        public void RandomPointTest()
        {
            Point a = curve.RandomPoint(new Random());
            Assert.IsTrue(curve.IsOnCurve(a));
        }

        [TestMethod]
        public void JDoubleMutTest()
        {
            BigInt x = new BigInt("489a03c58dcf7fcfc97e99ffef0bb4634", 16);
            BigInt y = new BigInt("510c6972d795ec0c2b081b81de767f808", 16);
            BigInt z = new BigInt("1", 16);
            JacobPoint a = new JacobPoint(x, y, z);
            curve.JDblMut(a);
            BigInt xE = new BigInt("417b9d98f934b571bffaed8d2968f6d67", 16);
            BigInt yE = new BigInt("6339a332da64ca233033eace4d6bdafdf", 16);
            Point e = new Point(xE, yE);
            Point r = curve.JToA(a);
            Assert.AreEqual(e, r);
        }

        [TestMethod]
        public void JAddMutTest()
        {
            BigInt x = new BigInt("489a03c58dcf7fcfc97e99ffef0bb4634", 16);
            BigInt y = new BigInt("510c6972d795ec0c2b081b81de767f808", 16);
            BigInt z = new BigInt("1", 16);
            BigInt xB = new BigInt("417b9d98f934b571bffaed8d2968f6d67", 16);
            BigInt yB = new BigInt("6339a332da64ca233033eace4d6bdafdf", 16);
            JacobPoint a = new JacobPoint(x, y, z);
            Point b = new Point(xB, yB);
            curve.JAddMut(a, b);
            BigInt xE = new BigInt("4e429ac21600f83ae575eda41ccbd72c5", 16);
            BigInt yE = new BigInt("14eb50532fc0f26c9e1328bbfe5747322", 16);
            Point e = new Point(xE, yE);
            Point r = curve.JToA(a);
            Assert.AreEqual(e, r);
        }

        [TestMethod]
        public void JToATest()
        {
            BigInt x = new BigInt("683912747928604429365587748083767546177", 10);
            BigInt y = new BigInt("812110666547712189275454409466955570012", 10);
            BigInt z = new BigInt("3931989407626300110045528123007777908518", 10);
            BigInt xE = new BigInt("4e429ac21600f83ae575eda41ccbd72c5", 16);
            BigInt yE = new BigInt("14eb50532fc0f26c9e1328bbfe5747322", 16);
            JacobPoint a = new JacobPoint(x, y, z);
            Point e = new Point(xE, yE);
            Point r = curve.JToA(a);
            Assert.AreEqual(e, r);
        }


    }
}
