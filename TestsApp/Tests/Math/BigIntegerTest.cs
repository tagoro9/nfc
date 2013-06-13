using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using IBCS.Math;
using IBCS.Util;
using System.Runtime.Serialization;

namespace TestsApp.Tests.Math
{
    [TestClass]
    public class BigIntegerTest
    {

        [TestMethod]
        public void AbsTest()
        {
            BigInt a = new BigInt(-123456);
            BigInt e = new BigInt(123456);
            Assert.AreEqual(e, a.Abs());
        }

        [TestMethod]
        public void AddTest()
        {
            BigInt a = new BigInt("489a03c58dcf7fcfc97e99ffef0bb4634", 16);
            BigInt b = new BigInt("510c6972d795ec0c2b081b81de767f808", 16);
            BigInt e = new BigInt("99a66d3865656bdbf486b581cd8233e3c", 16);
            Assert.AreEqual(e, a.Add(b));
        }

        [TestMethod]
        public void AndTest()
        {
            BigInt a = new BigInt(121);
            BigInt b = new BigInt(27);
            BigInt e = new BigInt(25);
            Assert.AreEqual(e, a.And(b));
        }

        [TestMethod]
        public void AndNotTest()
        {
            BigInt a = new BigInt(121);
            BigInt b = new BigInt(27);
            BigInt e = new BigInt(96);
            Assert.AreEqual(e, a.AndNot(b));
        }

        [TestMethod]
        public void BitCountTest()
        {
            BigInt a = new BigInt(123456);
            Assert.AreEqual(6, a.BitCount());
        }

        [TestMethod]
        public void BitLengthTest()
        {
            BigInt a = new BigInt(123456);
            Assert.AreEqual(17, a.BitLength());
        }

        [TestMethod]
        public void ClearBitTest()
        {
            BigInt a = new BigInt(127);
            BigInt e = new BigInt(63);
            Assert.AreEqual(e, a.ClearBit(6));
        }

        [TestMethod]
        public void CompareToTest()
        {
            BigInt a = new BigInt(123);
            BigInt b = new BigInt(9);
            Assert.AreEqual(0, a.CompareTo(a));
            Assert.AreEqual(1, a.CompareTo(b));
            Assert.AreEqual(-1, b.CompareTo(a));
        }

        [TestMethod]
        public void DivideTest()
        {
            BigInt a = new BigInt("489a03c58dcf7fcfc97e99ffef0bb4634", 16);
            BigInt b = new BigInt("510c6972d795ec0c2b081b81de767f808", 16);
            BigInt e = new BigInt(1);
            Assert.AreEqual(e, b.Divide(b));

        }

        [TestMethod]
        public void DivideAndRemainderTest()
        {
            BigInt a = new BigInt("489a03c58dcf7fcfc97e99ffef0bb4634", 16);
            BigInt b = new BigInt("510c6972d795ec0c2b081b81de767f808", 16);
            BigInt e = new BigInt(1);
            BigInt r = new BigInt("87265ad49c66c3c61898181ef6acb1d4", 16);
            BigInt[] result = b.DivideAndRemainder(a);
            Assert.AreEqual(e, result[0]);
            Assert.AreEqual(r, result[1]);
        }

        [TestMethod]
        public void EqualsTest()
        {
            BigInt a = new BigInt("489a03c58dcf7fcfc97e99ffef0bb4634", 16);
            BigInt b = new BigInt("510c6972d795ec0c2b081b81de767f808", 16);
            Assert.IsFalse(a.Equals(b));
            Assert.IsTrue(a.Equals(a));
        }

        [TestMethod]
        public void GcdTest()
        {
            BigInt a = new BigInt("489a03c58dcf7fcfc97e99ffef0bb4634", 16);
            BigInt b = new BigInt("510c6972d795ec0c2b081b81de767f808", 16);
            BigInt r = new BigInt(12);
            Assert.AreEqual(r, a.Gcd(b));
        }

        [TestMethod]
        public void IntValueTest()
        {
            BigInt a = new BigInt(123456);
            int e = 123456;
            Assert.AreEqual(e, a.IntValue());
        }

        [TestMethod]
        public void IsProbablePrimeTest()
        {
            BigInt a = new BigInt("bffffffffffffffffffffffffffcffff3", 16);
            BigInt b = new BigInt("510c6972d795ec0c2b081b81de767f808", 16);
            Assert.IsTrue(a.IsProbablePrime(10));
            Assert.IsFalse(b.IsProbablePrime(10));
        }

        [TestMethod]
        public void LongValueTest()
        {
            long a = 1234567890;
            BigInt b = new BigInt("1234567890", 10);
            Assert.AreEqual(a, b.LongValue());
        }

        [TestMethod]
        public void MaxTest()
        {
            BigInt a = new BigInt("bffffffffffffffffffffffffffcffff3", 16);
            BigInt b = new BigInt("510c6972d795ec0c2b081b81de767f808", 16);
            Assert.AreEqual(a, a.Max(b));
        }

        [TestMethod]
        public void MinTest()
        {
            BigInt a = new BigInt("bffffffffffffffffffffffffffcffff3", 16);
            BigInt b = new BigInt("510c6972d795ec0c2b081b81de767f808", 16);
            Assert.AreEqual(b, a.Min(b));
        }

        [TestMethod]
        public void ModTest()
        {
            BigInt m = new BigInt("bffffffffffffffffffffffffffcffff3", 16);
            BigInt a = new BigInt(12345);
            BigInt b = new BigInt(-123456);
            BigInt c = new BigInt("bffffffffffffffffffffffffffcffffd", 16);
            BigInt r1 = new BigInt("bffffffffffffffffffffffffffce1db3", 16);
            BigInt r2 = new BigInt("a", 16);
            Assert.AreEqual(a, a.Mod(m));
            Assert.AreEqual(r1, b.Mod(m));
            Assert.AreEqual(r2, c.Mod(m));

            BigInt p = new BigInt("7401141187874108427630978940572992096905023977763437457544729409920516808700177140037470732982461341936680397619662697052215095603718060674210397061385939", 10);
            BigInt val = new BigInt("-8805429631650526155836600268159171770648675293342431242561252684743455675458956278839936533508930607472520298823261623691721172019027250972148014155598348733105604819284208971479513407675775814097476803038337847470770299691804138857426678437453236036186849010942113274374881531890439789017226262436184284536574664706786746132934883579663088274750233751279959539065418679", 10);
            BigInt e = new BigInt("4172347906990935952909974862081322867216789152635156965746994396748028565812688217693287691708857986169886365572269519123982258587656695371921544143722320", 10);
            Assert.AreEqual(e, val.Mod(p));
        }

        [TestMethod]
        public void ModInverseTest()
        {
            BigInt m = new BigInt("bffffffffffffffffffffffffffcffff3", 16);
            BigInt a = new BigInt("489a03c58dcf7fcfc97e99ffef0bb4634", 16);
            BigInt e = new BigInt("4068b16ae3212238617c8cb5b0c2b6e7c", 16);
            Assert.AreEqual(e, a.ModInverse(m));
        }

        [TestMethod]
        public void ModPowTest()
        {
            BigInt m = new BigInt("bffffffffffffffffffffffffffcffff3", 16);
            BigInt a = new BigInt("489a03c58dcf7fcfc97e99ffef0bb4634", 16);
            BigInt exp = new BigInt("4068b16ae3212238617c8cb5b0c2b6e7c", 16);
            BigInt expected = new BigInt("67f143a2a79cb0a71def9354df2731ac6", 16);
            Assert.AreEqual(expected, a.ModPow(exp, m));
        }

        [TestMethod]
        public void MultiplyTest()
        {
            BigInt m = new BigInt("bffffffffffffffffffffffffffcffff3", 16);
            BigInt a = new BigInt("489a03c58dcf7fcfc97e99ffef0bb4634", 16);
            int b = 5;
            BigInt e = new BigInt("367382d42a5b9fdbd71ef37ff347ed7c29b23380fd580bab5e4955bcf09cbd6f5c", 16);
            BigInt e2 = new BigInt("3bffffffffffffffffffffffffff0fffbf", 16);
            Assert.AreEqual(e, m.Multiply(a));

        }

        [TestMethod]
        public void NegateTest()
        {
            BigInt a = new BigInt(123);
            BigInt e = new BigInt(-123);
            Assert.AreEqual(e, a.Negate());
        }

        [TestMethod]
        public void NotTest()
        {
            BigInt a = new BigInt(123);
            BigInt e = new BigInt(-124);
            Assert.AreEqual(e, a.Not());
        }

        [TestMethod]
        public void OrTest()
        {
            BigInt m = new BigInt("bffffffffffffffffffffffffffcffff3", 16);
            BigInt a = new BigInt("489a03c58dcf7fcfc97e99ffef0bb4634", 16);
            BigInt e = new BigInt("ffffffffffffffffffffffffffffffff7", 16);
            Assert.AreEqual(e, m.Or(a));
        }

        [TestMethod]
        public void ProbablePrimeTest()
        {
            BigInt a = BigInt.ProbablePrime(128, new Random());
            Assert.IsTrue(a.IsProbablePrime(10));
        }

        [TestMethod]
        public void RemainderTest()
        {
            BigInt m = new BigInt("bffffffffffffffffffffffffffcffff3", 16);
            BigInt a = new BigInt(12345);
            BigInt b = new BigInt(-123456);
            BigInt c = new BigInt("bffffffffffffffffffffffffffcffffd", 16);
            BigInt r1 = new BigInt("bffffffffffffffffffffffffffce1db3", 16);
            BigInt r2 = new BigInt("a", 16);
            Assert.AreEqual(a, a.Mod(m));
            Assert.AreEqual(r1, b.Mod(m));
            Assert.AreEqual(r2, c.Mod(m));
        }

        [TestMethod]
        public void SetBitTest()
        {
            BigInt a = new BigInt(126);
            BigInt e = new BigInt(127);
            Assert.AreEqual(e, a.SetBit(0));
        }

        [TestMethod]
        public void ShiftLeftTest()
        {
            BigInt m = new BigInt("bffffffffffffffffffffffffffcffff3", 16);
            BigInt e = new BigInt("2fffffffffffffffffffffffffff3fffcc00", 16);
            Assert.AreEqual(e, m.ShiftLeft(10));
        }

        [TestMethod]
        public void ShiftRightTest()
        {
            BigInt m = new BigInt("bffffffffffffffffffffffffffcffff3", 16);
            BigInt e = new BigInt("2fffffffffffffffffffffffffff3ff", 16);
            Assert.AreEqual(e, m.ShiftRight(10));
        }

        [TestMethod]
        public void SignumTest()
        {
            BigInt a = new BigInt(10);
            BigInt b = new BigInt(0);
            BigInt c = new BigInt(-10);
            Assert.AreEqual(1, a.Signum());
            Assert.AreEqual(0, b.Signum());
            Assert.AreEqual(-1, c.Signum());
        }

        [TestMethod]
        public void SubtractTest()
        {
            BigInt a = new BigInt("489a03c58dcf7fcfc97e99ffef0bb4634", 16);
            BigInt b = new BigInt("510c6972d795ec0c2b081b81de767f808", 16);
            BigInt e = new BigInt("87265ad49c66c3c61898181ef6acb1d4", 16);
            Assert.AreEqual(e, b.Subtract(a));
        }

        [TestMethod]
        public void TestBitTest()
        {
            BigInt number = new BigInt(5);
            bool bit = number.TestBit(2);
            Assert.AreEqual(true, bit);
        }

        [TestMethod]
        public void TobyteArrayTest()
        {
            sbyte[] e = new sbyte[] { 55 };
            BigInt a = new BigInt(55);
            Assert.IsTrue(ByteArrayUtil.AreEqual(a.ToByteArray(), e));
        }

        [TestMethod]
        public void ToStringTest()
        {
            BigInt a = new BigInt("489a03c58dcf7fcfc97e99ffef0bb4634", 16);
            string r = "1544065694588693922751392396002715190836";
            string r1 = "489a03c58dcf7fcfc97e99ffef0bb4634";
            Assert.AreEqual(r, a.ToString());
            Assert.AreEqual(r1, a.ToString(16));
        }

        [TestMethod]
        public void ValueOfTest()
        {
            BigInt r = BigInt.ValueOf(123456);
            BigInt e = new BigInt(123456);
            Assert.AreEqual(e, r);
        }

        [TestMethod]
        public void XorTest()
        {
            BigInt a = new BigInt("489a03c58dcf7fcfc97e99ffef0bb4634", 16);
            BigInt b = new BigInt("510c6972d795ec0c2b081b81de767f808", 16);
            BigInt e = new BigInt("19966ab75a5a93c3e276827e317dcbe3c", 16);
            Assert.AreEqual(e, a.Xor(b));
        }
    }
}