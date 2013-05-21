using System;
using IBCS.Math;
using IBCS.Interfaces;
using IBCS.EC;

namespace IBCS.BF.Key
{
    public class BFMasterPublicKey : PublicKey
    {
        private Pairing e;
        private Point p;
        private Point pPub;

        public BFMasterPublicKey(Pairing e, Point P, Point Ppub)
        {
            this.e = e;
            this.p = P;
            this.pPub = Ppub;
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

        public Pairing Pairing
        {
            get { return this.e; }
        }

        public Point P
        {
            get { return this.p; }
        }

        public Point Ppub
        {
            get { return this.pPub; }
        }

    }
}
