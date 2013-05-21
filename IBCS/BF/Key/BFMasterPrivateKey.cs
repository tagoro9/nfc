using System;
using IBCS.Interfaces;
using IBCS.Math;

namespace IBCS.BF.Key
{
    public class BFMasterPrivateKey : PrivateKey
    {
        private BigInt s;

        public BFMasterPrivateKey(BigInt s)
        {
            this.s = s;
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

        public BigInt Key
        {
            get { return this.s; }
        }
    }
}
