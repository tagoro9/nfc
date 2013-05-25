using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IBCS.BF.Key;
using IBCS.BF;
using Newtonsoft.Json;
using System.Net;

namespace IBCSApp.Services.BF
{
    public class BfService : IBfService
    {
        public event BFCipherText CipherTextCompleted;

        public event BFPlainText DecipherTextCompleted;

        public async void CipherText(string message, string identity, SerializedPrivateKey sKey)
        {
            byte[] bMessage = Encoding.UTF8.GetBytes(message);
            var cipherTask = Task<BFCText>.Factory.StartNew(() => {
                BFUserPrivateKey key = new BFUserPrivateKey(sKey);
                BFUserPublicKey pKey = new BFUserPublicKey(identity, key.Param);
                return BFCipher.encrypt(pKey, bMessage, new Random());
            });
            await cipherTask;
            BFCText cipher = (BFCText) cipherTask.Result;
            string cipherText = HttpUtility.UrlEncode(JsonConvert.SerializeObject(cipher.Serialize()));
            if (CipherTextCompleted != null)
            {
                CipherTextCompleted(cipherText);
            }
        }

        public void DecipherText(string ct, SerializedPrivateKey sKey)
        {
            var decipherTask = Task<byte[]>.Factory.StartNew(() =>
            {
                BFUserPrivateKey key = new BFUserPrivateKey(sKey);
                SerializedBFCText ciphered = (SerializedBFCText)JsonConvert.DeserializeObject(HttpUtility.UrlDecode(ct), typeof(SerializedBFCText));
                BFCText cText = new BFCText(ciphered);
                return BFCipher.decrypt(cText, key);
            });
            byte[] bMessage = (byte[]) decipherTask.Result;
            string message = Encoding.UTF8.GetString(bMessage,0, bMessage.Length);
            if (DecipherTextCompleted != null)
            {
                DecipherTextCompleted(message);
            }
        }
    }
}
