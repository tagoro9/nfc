using System;
using IBCS.Math;
using IBCS.Interfaces;

namespace IBCS.BF.Key
{
    public class BFUserPublicKey : PublicKey
    {
        private String id;
        private BFMasterPublicKey param;

        public BFUserPublicKey(String id, BFMasterPublicKey param)
        {
            this.id = id;
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

        public String Key
        {
            get { return this.id; }
        }

        public BFMasterPublicKey Param
        {
            get { return this.param; }
        }
    }
}