using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IBCS.BF.Key;
using IBCS.Interfaces;
using IBCSApp.Models;

namespace IBCSApp.Services.API
{

    public delegate void BFMasterPublicKeyArgs(PublicKey key);
    public delegate void BFUserKeyArgs(SerializedPrivateKey key);

    public interface IkeysService
    {
        void GetMasterPublicKey();
        void GetUserKey(string identity, LoginToken token);
        event BFMasterPublicKeyArgs GetMasterPublicKeyCompleted;
        event BFUserKeyArgs GetUserKeyCompleted;
    }
}
