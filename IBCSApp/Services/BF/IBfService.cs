using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IBCS.BF.Key;

namespace IBCSApp.Services.BF
{
    public delegate void BFCipherText(string ct);
    public delegate void BFPlainText(string pt);

    public interface IBfService
    {
        event BFCipherText CipherTextCompleted;
        event BFPlainText DecipherTextCompleted;
        void CipherText(string message, string identity, SerializedPrivateKey sKey);
        void DecipherText(string ct, SerializedPrivateKey sKey);
    }
}
