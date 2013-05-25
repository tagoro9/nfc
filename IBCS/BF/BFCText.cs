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

        public BFCText(SerializedBFCText ser)
        {
            BigInt UX = new BigInt(ser.UX, 16);
            BigInt UY = new BigInt(ser.UY, 16);
            Point uPoint = new Point(UX, UY);
            this.u = uPoint;
            this.v = ser.V;
            this.w = ser.W;
        }

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

        public SerializedBFCText Serialize()
        {
            SerializedBFCText ser = new SerializedBFCText();
            ser.UX = u.X.ToString(16);
            ser.UY = u.Y.ToString(16);
            ser.V = v;
            ser.W = w;
            return ser;
        }
    }
}
