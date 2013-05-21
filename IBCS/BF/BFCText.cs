using System;
using IBCS.Math;
using IBCS.EC;

namespace IBCS.BF
{
    public class BFCText
    {
        private Point u;
        private byte[] v;
        private byte[] w;

        public BFCText(Point u, byte[] v, byte[] w)
        {
            this.u = u;
            this.v = v;
            this.w = w;
        }

        public Point U
        {
            get { return this.u; }
        }

        public byte[] V
        {
            get { return this.v; }
        }

        public byte[] W
        {
            get { return this.w; }
        }
    }
}
