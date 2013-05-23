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

        public BFUserPrivateKey(SerializedPrivateKey key)
        {
            this.key = new Point(new BigInt(key.KeyX), new BigInt(key.KeyY));
            Fp field = new Fp(new BigInt(key.CurveField));
            EllipticCurve curve = new EllipticCurve(field, new BigInt(key.CurveA), new BigInt(key.CurveB));
            Pairing e = new TatePairing(curve, new BigInt(key.PairingGroupOrder), new BigInt(key.PairingCofactor));
            Point p = new Point(new BigInt(key.PX), new BigInt(key.PY));
            Point pPub = new Point(new BigInt(key.PPubX), new BigInt(key.PPubY));
            this.param = new BFMasterPublicKey(e, p, pPub);
        }

        public SerializedPrivateKey Serialize()
        {
            SerializedPrivateKey sKey = new SerializedPrivateKey();
            sKey.KeyX = key.X.ToString(16);
            sKey.KeyY = key.Y.ToString(16);
            sKey.CurveField = param.Pairing.Curve.Field.GetP().ToString(16);
            sKey.CurveA = param.Pairing.Curve.A.ToString(16);
            sKey.CurveB = param.Pairing.Curve.B.ToString(16);
            sKey.PairingCofactor = param.Pairing.Cofactor.ToString(16);
            sKey.PairingGroupOrder = param.Pairing.GroupOrder.ToString(16);
            sKey.PX = param.P.X.ToString(16);
            sKey.PY = param.P.Y.ToString(16);
            sKey.PPubX = param.Ppub.X.ToString(16);
            sKey.PPubY = param.Ppub.Y.ToString(16);
            return sKey;
        }

        public BFUserPrivateKey()
        {
            this.key = null;
            this.param = null;
        }

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
