using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using IBCS.Math;

namespace TestsApp.Tests.Math
{
    [TestClass]
    public class ComplexTest
    {

        Complex a, b;
        Fp field;

        [TestInitialize]
        public void InitializePoints()
        {
            BigInt xA = new BigInt("79e7128ff9ea31d83fcad7522eeaf8ef5dcddf2026dd8b61f31cb42c99f0b53e36f6879ea556d459f551b7ac87827b64a172cc3f5170acba502d3d4334ce6ae0", 16);
            BigInt yA = new BigInt("2dfab43956cfa22ab888f29f3f0de15117476e7ebe56ec0f56344cb848dc60b0d27960ec82fb217d6bc17e873f265139fb6d88080ce15d642f0b6c88f6eb5b93", 16);
            BigInt xB = new BigInt("488af43b1f7896f448eb9b2b0d8d08609fae80378331a2f46eff498ff74587c9faee194750fa83dab39c249af427ee35eca11c85ae040c251b0c3e2d42867a6c", 16);
            BigInt yB = new BigInt("2fc1857562f9d8a35157eadecfd990157f8a064038d7d7c9d721c57202de9d8f1456295983f0cb61332084f3df37e9d6d24aba24e563df2a82b6a851038dbf2e", 16);
            BigInt p = new BigInt("7401141187874108427630978940572992096905023977763437457544729409920516808700177140037470732982461341936680397619662697052215095603718060674210397061385939", 10);
            field = new Fp(p);
            a = new Complex(field, xA, yA);
            b = new Complex(field, xB, yB);
        }

        [TestMethod]
        public void IsZeroTest()
        {
            Assert.IsFalse(a.IsZero());
            Complex zero = new Complex(field, BigInt.ZERO);
            Assert.IsTrue(zero.IsZero());

        }

        [TestMethod]
        public void IsOneTest()
        {
            Assert.IsFalse(a.IsOne());
            Complex one = new Complex(field, BigInt.ONE);
            Assert.IsTrue(one.IsOne());
        }

        [TestMethod]
        public void AddTest()
        {
            BigInt xE = new BigInt("35220081ee207cc2b5b7b40bbd947eaa7ebe25227a4baa74b5a20b9eb5f2cc9b36cb7db24e6788d062b7a918f7d15f7c37242ddc8e1a1866e7f13bee0f6cd679", 16);
            BigInt yE = new BigInt("5dbc39aeb9c97ace09e0dd7e0ee7716696d174bef72ec3d92d56122a4bbafe3fe6cf8a4606ebecde9ee2037b1e5e3b10cdb8422cf2453c8eb1c214d9fa791ac1", 16);
            Complex e = new Complex(field, xE, yE);
            Assert.AreEqual(e, a.Add(b));
        }

        [TestMethod]
        public void SubtractTest()
        {
            BigInt xE = new BigInt("315c1e54da719ae3f6df3c27215df08ebe1f5ee8a3abe86d841d6a9ca2ab2d743c086e57545c507f41b59311935a8d2eb4d1afb9a36ca0953520ff15f247f074", 16);
            BigInt yE = new BigInt("8b89350d1f1815913a2fc631ee17d3e1167ba273b54298272b8c79642141338eb93c5ac6a6f425807ed72cc1e3c77181801288cb98d81eb22f9d03ba5b45ab38", 16);
            Complex e = new Complex(field, xE, yE);
            Assert.AreEqual(e, a.Subtract(b));
        }

        [TestMethod]
        public void MultiplyTest()
        {
            BigInt xE = new BigInt("2440e582ff0eae9f3b6414ef0212c66642e9fcf6e68e3f0fd382e974e2e7227ef8ac964212d91f9a692fc1cdd8ffa79f40ffe70c230df360b22ca682292be98d", 16);
            BigInt yE = new BigInt("3b90f24fe172b0055d20edc686698f1a9825febc4ddf20e78d0e458b8fd08d9cb5af67247829a9f6426a4d0c29a0436e029b34845fd0145ce3e82a39319106c8", 16);
            Complex e = new Complex(field, xE, yE);
            Assert.AreEqual(e, a.Multiply(b));
        }

        [TestMethod]
        public void SquareTest()
        {
            BigInt xE = new BigInt("5a5013d8e99d5c74923a6255574dca16ce8a3baf6c12d075dc614c97d5b826efc29bd6ddf97612d8b5e247b1270579442ad9f50b94c2d0a026e135bc9517f41c", 16);
            BigInt yE = new BigInt("564766dd46b2b90e3eae4bbe183847a5862b838a2f42632d00f83cb079224dc4d666f0bd28d909936599fcde942cb358d47ca2f845d15ea427acd99908cd30ac", 16);
            Complex e = new Complex(field, xE, yE);
            Assert.AreEqual(e, a.Square());
            BigInt xU = new BigInt("1073412479682535018862284160576533390990420205073469103221186306215666995384751707328974787227703747969787181665536583631045558504604168504487407864499395", 10);
            BigInt yU = new BigInt("5856068547941270829217587377151399939486630434710800994676558520495401961675031771127309528464398215021343483616673868706179845767294209256606067396126508", 10);
            Complex u = new Complex(field, xU, yU);
            BigInt xEU = new BigInt("3170542692065887201357852972613253074058265787401805831692373488880493379981702597566323780180259893295724278468998259319539822651757650262903855249898980", 10);
            BigInt yEU = new BigInt("824084649556602505165671612756507148824944413961082968743716478804694503360774726530142150424870155634623871804020104186511071820272234365803681034026876", 10);
            Complex eU = new Complex(field, xEU, yEU);
            Assert.AreEqual(eU, u.Square());
        }

        [TestMethod]
        public void DivideTest()
        {
            BigInt xE = new BigInt("1d31053bec1cd46482c60b73d1545a16535db619e2756daaba4c4cde4c05c61e1bf1320c219fe219f9feaa4ae0d839474b0c84e5f9d7d0a660e364ea816bc2e9", 16);
            BigInt yE = new BigInt("3f53e8b877b79da0a56bbfed63c41e40acd97fb104d493d861c19f99458577751ce4491980e0ffc95f088573cc6a2c62097907d21c4bd32d3d08f0efb84dadf3", 16);
            Complex e = new Complex(field, xE, yE);
            Assert.AreEqual(e, a.Divide(b));
        }

        [TestMethod]
        public void ConjugateTest()
        {
            BigInt xE = new BigInt("79e7128ff9ea31d83fcad7522eeaf8ef5dcddf2026dd8b61f31cb42c99f0b53e36f6879ea556d459f551b7ac87827b64a172cc3f5170acba502d3d4334ce6ae0", 16);
            BigInt yE = new BigInt("5f55520fd472a9df1a75cbd23fd5a1546776cbb6716c97d25645a56592670fbc289fc24724eeade6da74b4a744b2b8e45b8232e064794314543cd2f970fcb340", 16);
            Complex e = new Complex(field, xE, yE);
            Assert.AreEqual(e, a.Conjugate());
        }

        [TestMethod]
        public void PowTest()
        {
            BigInt exp = new BigInt("2fc1857562f9d8a35157eadecfd990157f8a064038d7d7c9d721c57202de9d8f1456295983f0cb61332084f3df37e9d6d24aba24e563df2a82b6a851038dbf2e", 16);
            BigInt xE = new BigInt("7db8e520dcfa3c86add98d75b92962140ef6aebef3431835b3928231654af3ee32588953bd43bf658692fcd406f0cb05736c9789e69e01070981b546fc7432bb", 16);
            BigInt yE = new BigInt("32b90c633edf32174fe6c4e54399ee5586fc682db6a383c5a5ef065570e5e3e730e0c90abf4bd7be2e03e6a52d4403a24f3d66db7042da1478f47e4a32d85c57", 16);
            Complex e = new Complex(field, xE, yE);
            Assert.AreEqual(e, a.Pow(exp));
        }

        [TestMethod]
        public void InverseTest()
        {
            BigInt xE = new BigInt("4256100717783961070459408848666180089270296384666980665872863779755347492050561550250645896155026025851222135934422367229632839949569170493559818175803841", 10);
            BigInt yE = new BigInt("5970088382622522341807469483870903253958283675578458139880466596276861911789790058271606365385339535806708463757789699179829968857004602489189877157462174", 10);
            Complex e = new Complex(field, xE, yE);
            Assert.AreEqual(e, a.Inverse());
        }

        [TestMethod]
        public void EqualsTest()
        {
            Assert.IsTrue(a.Equals(a));
            Assert.IsTrue(b.Equals(b));
            Assert.IsFalse(a.Equals(b));
        }

        [TestMethod]
        public void NegateTest()
        {
            BigInt xE = new BigInt("1016582559610770774704813290780669822093413500599701513912949119342892800060923898453912754807067993374784376261725453177279902172111468871168069276902387", 10);
            BigInt yE = new BigInt("4993009192769040298194898355642872946951032916988733659048817055334834969800387705401808604132068961216703838496458290694339400190222952536194803220394816", 10);
            Complex e = new Complex(field, xE, yE);
            Assert.AreEqual(e, a.Negate());
        }
    }
}
