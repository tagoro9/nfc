using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using IBCS.Math;
using IBCS.Interfaces;

namespace IBCSTest.Math
{
    [TestClass]
    public class FpTest
    {

        Fp field;
        BigInt a, b, c, d;

        [TestInitialize]
        public void InitializeField()
        {
            BigInt p = new BigInt("bffffffffffffffffffffffffffcffff3", 16);
            field = new Fp(p);
            a = new BigInt("489a03c58dcf7fcfc97e99ffef0bb4634", 16);
            b = new BigInt("510c6972d795ec0c2b081b81de767f808", 16);
            c = new BigInt("417b9d98f934b571bffaed8d2968f6d67", 16);
            d = new BigInt("6bdafdf", 16);
        }

        [TestMethod, TestCategory("Math"), TestCategory("Fp")]
        public void AddTest()
        {
            BigInt e = new BigInt("99a66d3865656bdbf486b581cd8233e3c", 16);
            Assert.AreEqual(e, field.Add(a, b));
        }

        [TestMethod, TestCategory("Math"), TestCategory("Fp")]
        public void InverseTest()
        {
            BigInt e = new BigInt("4068b16ae3212238617c8cb5b0c2b6e7c", 16);
            Assert.AreEqual(e, field.Inverse(a));
        }

        [TestMethod, TestCategory("Math"), TestCategory("Fp")]
        public void MultiplyTest()
        {
            BigInt e = new BigInt("67e8b04a172e4c11cfb35c570551d75dc", 16);
            Assert.AreEqual(e, field.Multiply(a, b));
        }

        [TestMethod, TestCategory("Math"), TestCategory("Fp")]
        public void DivideTest()
        {
            BigInt e = new BigInt("bd1d368cf71d8a54390d2038b230cee3c", 16);
            Assert.AreEqual(e, field.Divide(a, d));
        }

        [TestMethod, TestCategory("Math"), TestCategory("Fp")]
        public void SubtractTest()
        {
            BigInt e = new BigInt("87265ad49c66c3c61898181ef6acb1d4", 16);
            Assert.AreEqual(e, field.Subtract(b, a));
        }

        [TestMethod, TestCategory("Math"), TestCategory("Fp")]
        public void RandomElementTest()
        {
            FieldElement r = field.RandomElement(new Random());
            Assert.IsTrue(field.IsValidElement(r));
        }

        [TestMethod, TestCategory("Math"), TestCategory("Fp")]
        public void IsValidElementTest()
        {
            BigInt w = new BigInt("dffffffffffffffffffffffffffcffff3", 16);
            Assert.IsTrue(field.IsValidElement(a));
            Assert.IsFalse(field.IsValidElement(w));
        }

        [TestMethod, TestCategory("Math"), TestCategory("Fp")]
        public void NegateTest()
        {
            BigInt e = new BigInt("7765fc3a723080303681660010f14b9bf", 16);
            Assert.AreEqual(e, field.Negate(a));
        }

        [TestMethod, TestCategory("Math"), TestCategory("Fp")]
        public void IsOneTest()
        {
            Assert.IsFalse(field.IsOne(a));
            Assert.IsTrue(field.IsOne(BigInt.ONE));

        }

        [TestMethod, TestCategory("Math"), TestCategory("Fp")]
        public void IsZeroTest()
        {
            Assert.IsTrue(field.IsZero(BigInt.ZERO));
            Assert.IsFalse(field.IsZero(a));
        }

        [TestMethod, TestCategory("Math"), TestCategory("Fp")]
        public void SquareTest()
        {
            BigInt e = new BigInt("a6c5a19786dfa9d4cb664cbb3f4c434c8", 16);
            Assert.AreEqual(e, field.Square(a));
        }

        [TestMethod, TestCategory("Math"), TestCategory("Fp")]
        public void SquareRootTest()
        {
            BigInt e = BigInt.ValueOf(5);
            BigInt pT = BigInt.ValueOf(11);
            BigInt aT = BigInt.ValueOf(3);
            Fp fT = new Fp(pT);
            Assert.AreEqual(e, fT.SquareRoot(aT));
        }

        [TestMethod, TestCategory("Math"), TestCategory("Fp")]
        public void EqualsTest()
        {
            Assert.IsTrue(a.Equals(a));
            Assert.IsFalse(a.Equals(b));
        }

        [TestMethod, TestCategory("Math"), TestCategory("Fp")]
        public void PowTest()
        {
            BigInt e = new BigInt("585f8a739443ace112bb166bb04fed079", 16);
            Assert.AreEqual(e, field.Pow(a, BigInt.ValueOf(20)));
        }
    }
}
