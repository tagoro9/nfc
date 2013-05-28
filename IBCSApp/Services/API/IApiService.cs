using IBCS.BF.Key;
using IBCS.Interfaces;
using IBCSApp.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBCSApp.Services.API
{
    public delegate void BFMasterPublicKeyArgs(PublicKey key);
    public delegate void BFUserKeyArgs(SerializedPrivateKey key);
    public delegate void LoginUserArgs(LoginToken token, string email);
    public delegate void CreateAccountArgs();

    public interface IApiService
    {
        void LoginUser(string email, string password);
        void GetMasterPublicKey();
        void GetUserKey(string identity, LoginToken token);
        void CreateAccount(string email, string password, string name);

        //Events
        event BFMasterPublicKeyArgs GetMasterPublicKeyCompleted;
        event BFUserKeyArgs GetUserKeyCompleted;
        event LoginUserArgs LoginUserCompleted;
        event CreateAccountArgs CreateAccountCompleted;
    }
}
