using System;
using IBCS.Interfaces;

namespace IBCS.BF.Key
{
    public class KeyPair
    {
        private PrivateKey privateKey;
        private PublicKey publicKey;

        public PublicKey Public
        {
            get { return publicKey; }
        }

        public PrivateKey Private
        {
            get { return privateKey; }
        }

        public KeyPair(PublicKey publicKey, PrivateKey privateKey)
        {
            this.publicKey = publicKey;
            this.privateKey = privateKey;
        }
    }
}

