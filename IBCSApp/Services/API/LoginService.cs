using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using IBCSApp.Entities;
using Newtonsoft.Json;

namespace IBCSApp.Services.API
{
    public class LoginService : ILoginService
    {

        private string email;

        public async void LoginUser(string email, string password)
        {
            HttpWebRequest request = HttpWebRequest.CreateHttp(APIInfo.APIURL + "users/login");
            request.ContentType = "application/x-www-form-urlencoded";
            request.Method = "POST";

            using (var stream = await Task.Factory.FromAsync<Stream>(request.BeginGetRequestStream,
                                                                     request.EndGetRequestStream, null))
            {
                StringBuilder postData = new StringBuilder();
                postData.Append("Email=" + email);
                postData.Append("&");
                postData.Append("Password=" + password);
                string a = postData.ToString();
                byte[] byteArray = Encoding.UTF8.GetBytes(postData.ToString());

                // Write the bytes to the stream
                await stream.WriteAsync(byteArray, 0, postData.Length);
            }
            this.email = email;

            //request.BeginGetRequestStream(new AsyncCallback(GetRequestStreamCallback), request);
            request.BeginGetResponse(new AsyncCallback(HttpWebRequestCallBack), request);
        }

        public event LoginUserArgs LoginUserCompleted;

        private void HttpWebRequestCallBack(IAsyncResult asyncResult)
        {
            LoginToken token = new LoginToken();
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
                            token = JsonConvert.DeserializeObject<LoginToken>(result);
                        }
                    }
                    
                }
                catch (WebException ex)
                {
                    var response = ex.Response as HttpWebResponse;
                }
                if (LoginUserCompleted != null)
                    LoginUserCompleted(token, email);
            }
        }
    }
}
