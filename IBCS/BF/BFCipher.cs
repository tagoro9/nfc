using System;
using System.Text;
using IBCS.BF.Key;
using IBCS.BF.Util;
using IBCS.EC;
using IBCS.Math;
using IBCS.Interfaces;

namespace IBCS.BF
{
    public class BFCipher
    {
        private static int sigmaBitLength = 512;

        public static KeyPair Setup(Pairing e, Random rnd)
        {
            Point P = e.Curve2.RandomPoint(rnd);
            BigInt s = new BigInt(e.GroupOrder.BitLength(), rnd);

            while (s.CompareTo(e.GroupOrder) >= 0)
                s = s.ShiftRight(1);

            Point Ppub = e.Curve2.Multiply(P, s);

            BFMasterPublicKey pk = new BFMasterPublicKey(e, P, Ppub);

            BFMasterPrivateKey sk = new BFMasterPrivateKey(s);

            //return new KeyPair(pk, sk);
            return new KeyPair(null, null);

        }


        public static PublicKey ExtractPublic(KeyPair masterKey, String ID)
        {
            BFUserPublicKey pk = new BFUserPublicKey(ID, (BFMasterPublicKey)masterKey.Public);
            return pk;
        }


        public static KeyPair Extract(KeyPair masterKey, String ID, Random rnd)
        {
            //user public key is ID+ public parameters

            BFUserPublicKey pk = new BFUserPublicKey(ID, (BFMasterPublicKey)masterKey.Public);


            Pairing e = ((BFMasterPublicKey)masterKey.Public).Pairing;
            //user private key: hash(ID)->point Q
            //sQ, s is the master private key
            byte[] bid = null;
            bid = Encoding.UTF8.GetBytes(ID);

            Point Q = BFUtil.HashToPoint(bid, e.Curve, e.Cofactor);
            BigInt s = ((BFMasterPrivateKey)masterKey.Private).Key;
            Q = e.Curve.Multiply(Q, s);

            BFUserPrivateKey sk = new BFUserPrivateKey(Q, (BFMasterPublicKey)masterKey.Public);

            return new KeyPair(pk, sk);
        }

        public static BFCText encrypt(BFUserPublicKey pk, byte[] m, Random rnd)
        {
            Pairing e = pk.Param.Pairing;
            byte[] sigma = new byte[BFCipher.sigmaBitLength / 8];

            rnd.NextBytes(sigma);

            //sigma||m
            byte[] toHash = new byte[sigma.Length + m.Length];
            Array.Copy(sigma, 0, toHash, 0, sigma.Length);
            Array.Copy(m, 0, toHash, sigma.Length, m.Length);

            //hash(sigma||m) to biginteger r;
            Field field = e.Curve2.Field;
            BigInt r = BFUtil.HashToField(toHash, field);

            //hash(ID) to point
            byte[] bid = null;
            String ID = pk.Key;
            bid = Encoding.UTF8.GetBytes(ID);

            Point Q = BFUtil.HashToPoint(bid, e.Curve, e.Cofactor);

            //gID = e(Q, sP), sP is Ppub
            FieldElement gID = e.Compute(Q, pk.Param.Ppub);

            //U=rP
            Point U = e.Curve2.Multiply(pk.Param.P, r);
            //gID^r
            FieldElement gIDr = e.Gt.Pow(gID, r);

            //V=sigma xor hash(gID^r)
            byte[] hash = BFUtil.HashToLength(gIDr.ToUByteArray(), sigma.Length); //This could fail
            byte[] V = BFUtil.XorTwoByteArrays(sigma, hash);

            //W =m xor hash(sigma)

            hash = BFUtil.HashToLength(sigma, m.Length);
            byte[] W = BFUtil.XorTwoByteArrays(m, hash);

            return new BFCText(U, V, W);

        }

        public static byte[] decrypt(BFCText c, BFUserPrivateKey sk)
        {
            BFMasterPublicKey msk = sk.Param;
            Pairing e = msk.Pairing;

            //e(sQ,U), sQ is the user private key		
            FieldElement temp = e.Compute(sk.Key, c.U);

            //sigma = V xor hash(temp)
            byte[] hash = BFUtil.HashToLength(temp.ToUByteArray(), c.V.Length); //This could fail

            byte[] sigma = BFUtil.XorTwoByteArrays(c.V, hash);

            hash = BFUtil.HashToLength(sigma, c.W.Length);

            byte[] m = BFUtil.XorTwoByteArrays(hash, c.W);

            //sigma||m
            byte[] toHash = new byte[sigma.Length + m.Length];
            Array.Copy(sigma, 0, toHash, 0, sigma.Length);
            Array.Copy(m, 0, toHash, sigma.Length, m.Length);

            //hash(sigma||m) to biginteger r;
            Field field = e.Curve2.Field;
            BigInt r = BFUtil.HashToField(toHash, field);

            if (c.U.Equals(e.Curve2.Multiply(msk.P, r)))
                return m;
            else
                return null;

        }
    }
}
