using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using IBCS.BF;
using IBCS.BF.Key;
using IBCS.EC;
using IBCS.Math;

namespace TestsApp.Tests.BF
{
    [TestClass]
    public class BFCipherTest
    {
        private BFUserPrivateKey key;
        private BFUserPublicKey pKey;

        [TestInitialize]
        public void InitializePrivateKey()
        {
            SerializedPrivateKey sKey = new SerializedPrivateKey();
            sKey.KeyX = "45042c22425461e759bab12d1d77626800f3a9f432944165a870fe3e9b8dea56023c19a7e0e512eb397b749c8d840625ab89e1ae2ec7d20ccefc3c50a3407269";
            sKey.KeyY = "1c27658c204900d2073806f46d4fd9ce865ca5699c6d1b06866afee8c475fb930ecc6816fb8961f49383885ec5d61acf6e3c904a54caab238684547ab24bc79c";
            sKey.PairingCofactor = "117454A4537B38AF9F9159D8EDBFB7E7C7C2E48760E930A461D5F451F9D9210DC70095F4B241FF57F1BB0549C";
            sKey.PairingGroupOrder = "8000000000000000000000000000000000020001";
            sKey.CurveField = "8BA2A5229BD9C57CFC8ACEC76DFDBF3E3E1952C6B3193ECF5C571FB502FC5DF410F9267E9F2A605BB0F76F52A79E8043BF4AF0EF2E9FA78B0F1E2CDFC4E8549B";
            sKey.CurveA = "1";
            sKey.CurveB = "0";
            sKey.PX = "51F135321C5575E297646AF71ED686E56C16CB32EE78E6DE5794139645D4A9107E9552BF33128FAF0ECAD1DD0A57E14BBAE1ECB1D1C08249011E7AA51AA300E7";
            sKey.PY = "5A668356ACD310BCB60AD334E6F0D4BFFFEDD2BA2E2CE19743CE4C11DC739D8B660AAA69D37CB321BDFB8144D57C87C98800AF3AB82FBF5648A2AFF591B3E4C6";
            sKey.PPubX = "3F6B422DBF545B49D04CDFC08B5DF434129D6560D99D4C3F97165A6CB178DDBF9A3082BD03265E1CAE56539D83EA21365C448C14A1033D817B62BA2E6CC08713";
            sKey.PPubY = "6EF3DECFADA0CEF8F27CC8FF00A636E0DE4CB388802EAEC5A908FC7136D2F0A19CCDC667950BCE4F2278ACAB739F342D260FBA29FA9A0DA5B1F28E9793AD8B4D";
            key = new BFUserPrivateKey(sKey);
            pKey = new BFUserPublicKey("foo@bar.com", key.Param);
        }

        [TestMethod]
        [Description("It should encrypt and decrypt a message correctly")]
        public void CipherTest()
        {
            Random rnd = new Random();
            byte[] message = new byte[511];
            rnd.NextBytes(message);
            BFCText encypted = BFCipher.encrypt(pKey, message, rnd);
            byte[] result = BFCipher.decrypt(encypted, key);
            Assert.AreEqual(Encoding.UTF8.GetString(message, 0, message.Length), Encoding.UTF8.GetString(result, 0, message.Length));
        }

        [TestMethod]
        [Description("It should encrypt messages in a reasonable time")]
        public void CipherInReasonableTimeTest()
        {
            int times = 50;
            int size = 511;
            long[] measures = new long[50];
            Random rnd = new Random();
            System.Diagnostics.Stopwatch m_watch = new System.Diagnostics.Stopwatch();
            m_watch.Start();
            for (int i = 0; i < times; i++)
            {
                byte[] message = new byte[size];
                rnd.NextBytes(message);
                m_watch.Reset();
                m_watch.Start();
                BFCipher.encrypt(pKey, message, rnd);
                measures[i] = m_watch.ElapsedMilliseconds;
                m_watch.Stop();
            }
            string prompt = "Time spent to cipher " + times + " messages of " + size + " bytes:";
            System.Diagnostics.Debug.WriteLine("{0}  {1} msec", prompt, m_watch.ElapsedMilliseconds);
        }
    }
}
