using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using IBCS.BF.Key;
using IBCS.BF;
using IBCS.Math;
using IBCS.EC;
using PKGServer.Authorization;

namespace PKGServer.Controllers
{
    public class KeysController : ApiController
    {
        KeyPair master;

        public KeysController()
        {

            TatePairing e = Predefined.ssTate();
            BigInt s = new BigInt("505589879806357574715819689796588537146433291440", 10);
            BFMasterPrivateKey masterPrivate = new BFMasterPrivateKey(s);
            BigInt xP = new BigInt("4291662186182105785020055031256275922735182686564035226466395653970413755358824903482223509624549595190077490704367257937172972896431165061643256253841639", 10);
            BigInt yP = new BigInt("4734655302033019638724717069550998600270840147458147624263679086394750166097273830456518097912007100893265204858608788227968249694875877762941090121639110", 10);
            BigInt xPpub = new BigInt("3321521420324942690122656396767763795495870232445101779865981146134484022298665464121724898190544088107820667943606314446170142017782563402275554873870099", 10);
            BigInt yPpub = new BigInt("5811060153287472925206265749794945978469077668029247862550354611572579518279518356217273453987945241447730233617290283194068671621572534510817890943208269", 10);
            Point P = new Point(xP, yP);
            Point Ppub = new Point(xPpub, yPpub);
            BFMasterPublicKey masterPublic = new BFMasterPublicKey(e, P, Ppub);
            master = new KeyPair(masterPublic, masterPrivate);
        }

        [HttpGet]
        public BFMasterPublicKey Public()
        {
            return (BFMasterPublicKey) master.Public;
        }

        [HttpGet]
        public BFUserPublicKey Public(string id)
        {
            BFUserPublicKey userKey = (BFUserPublicKey)BFCipher.ExtractPublic(master, id);
            return userKey;
        }

        [HttpGet, RequireAuthorization]
        public SerializedPrivateKey Private(string id)
        {
            KeyPair user = BFCipher.Extract(master, id, new Random());
            return ((BFUserPrivateKey)user.Private).Serialize();
        }

    }
}
