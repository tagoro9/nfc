using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using IBCS.BF.Key;
using IBCSApp.Models;
using IBCSApp.Converters;
using Newtonsoft.Json;

namespace IBCSApp.Services.API
{
    public class KeysService : IkeysService
    {

        public event BFMasterPublicKeyArgs GetMasterPublicKeyCompleted;
        public event BFUserKeyArgs GetUserKeyCompleted;

        public void GetMasterPublicKey()
        {
            HttpWebRequest request = HttpWebRequest.CreateHttp(APIInfo.APIURL + "keys");
            request.BeginGetResponse(new AsyncCallback(GetMasterPublicKeyCallback), request);
        }

        private void GetMasterPublicKeyCallback(IAsyncResult asyncResult)
        {
            BFMasterPublicKey key = null;
            if (asyncResult.IsCompleted)
            {
                HttpWebRequest request = (HttpWebRequest)asyncResult.AsyncState;
                try
                {
                    var response = request.EndGetResponse(asyncResult);

                    if (response != null)
                    {
                        string result;
                        using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                        {
                            result = reader.ReadToEnd();
                        }

                        if (!string.IsNullOrEmpty(result))
                        {
                        }
                    }

                }
                catch (WebException ex)
                {
                    var response = ex.Response as HttpWebResponse;
                }
                if (GetMasterPublicKeyCompleted != null)
                    GetMasterPublicKeyCompleted(key);
            }
        }

        public void GetUserKey(string identity, LoginToken token)
        {
            HttpWebRequest request = HttpWebRequest.CreateHttp(APIInfo.APIURL + "keys/private/" + identity + "?accessToken=" + token.Token);
            request.Accept = "application/json";
            request.BeginGetResponse(new AsyncCallback(GetUserKeyCallback), request);
        }

        private void GetUserKeyCallback(IAsyncResult asyncResult)
        {
            //BFUserPrivateKey key = null;
            SerializedPrivateKey key = new SerializedPrivateKey();
            if (asyncResult.IsCompleted)
            {
                HttpWebRequest request = (HttpWebRequest)asyncResult.AsyncState;
                try
                {
                    var response = request.EndGetResponse(asyncResult);

                    if (response != null)
                    {
                        string result;
                        using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                        {
                            result = reader.ReadToEnd();
                        }

                        if (!string.IsNullOrEmpty(result))
                        {
                            //key = JsonConvert.DeserializeObject<BFUserPrivateKey>(result, new KeyConverter());
                            JsonSerializer ser = new JsonSerializer();
                            key = JsonConvert.DeserializeObject<SerializedPrivateKey>(result);
                        }
                    }

                }
                catch (WebException ex)
                {
                    var response = ex.Response as HttpWebResponse;
                }
                if (GetUserKeyCompleted != null)
                    GetUserKeyCompleted(key);
            }
        }
    }
}
