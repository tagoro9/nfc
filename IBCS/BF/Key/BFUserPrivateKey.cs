using System;
using IBCS.Math;
using IBCS.Interfaces;
using IBCS.EC;

namespace IBCS.BF.Key
{
    public class BFUserPrivateKey : PrivateKey
    {
        private Point key;
        private BFMasterPublicKey param;

        public BFUserPrivateKey(Point sk, BFMasterPublicKey param)
        {
            this.key = sk;
            this.param = param;
        }

        public String GetAlgorithm()
        {
            return "Boneh-Franklin IBE";
        }

        public byte[] GetEncoded()
        {
            return null;
        }

        public String GetFormat()
        {
            return null;
        }

        public Point Key
        {
            get { return this.key; }
        }

        public BFMasterPublicKey Param
        {
            get { return this.param; }

        }
    }
}
