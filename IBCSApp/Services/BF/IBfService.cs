using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IBCS.BF.Key;

namespace IBCSApp.Services.BF
{
    public interface IBfService
    {
        Task<string> CipherText(string message, string identity, SerializedPrivateKey sKey);
        Task<string> DecipherText(string ct, SerializedPrivateKey sKey);
        Task<string> CipherMessage(byte[] message, string identity, SerializedPrivateKey sKey);
        Task<byte[]> DecipherMessage(string ct, SerializedPrivateKey sKey);
    }
}
