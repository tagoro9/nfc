using System;
using IBCS.Math;
using IBCS.Interfaces;
using IBCS.EC;
using System.Runtime.Serialization;

namespace IBCS.BF.Key
{
    [DataContract]
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

        [DataMember]
        public Pairing Pairing
        {
            get { return this.e; }
            set
            {
                e = value;
            }
        }

        [DataMember]
        public Point P
        {
            get { return this.p; }
            set { p = value; }
        }

        [DataMember]
        public Point Ppub
        {
            get { return this.pPub; }
            set { pPub = value; }
        }

    }
}
