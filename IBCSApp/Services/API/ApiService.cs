using IBCS.BF.Key;
using IBCSApp.Entities;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBCSApp.Services.API
{
    public class ApiService : IApiService
    {

        public static string APIURL = "https://pkg.apphb.com/api/";
        public static string SHORTEN_URL = "http://tinyurl.com/api-create.php";
        private RestClient client = new RestClient(APIURL);
        private RestClient shortenClient = new RestClient(SHORTEN_URL);

        //Events
        public event BFMasterPublicKeyArgs GetMasterPublicKeyCompleted;
        public event BFUserKeyArgs GetUserKeyCompleted;
        public event LoginUserArgs LoginUserCompleted;
        public event CreateAccountArgs CreateAccountCompleted;


        public void LoginUser(string email, string password)
        {
            RestRequest request = new RestRequest("users/login", Method.POST);
            request.AddParameter("Email", email);
            request.AddParameter("Password", password);
            client.ExecuteAsync<LoginToken>(request, response =>
            {
                if (LoginUserCompleted != null)
                    LoginUserCompleted(response.Data, email);
            });
        }

        public void GetMasterPublicKey()
        {
            throw new NotImplementedException();
        }

        public void GetUserKey(string identity, Entities.LoginToken token)
        {
            RestRequest request = new RestRequest("keys/private/{id}", Method.GET);
            request.AddUrlSegment("id", identity);
            request.AddParameter("accessToken", token.Token);
            request.AddHeader("Accept", "application/json");
            client.ExecuteAsync<SerializedPrivateKey>(request, response =>
            {
                if (GetUserKeyCompleted != null)
                    GetUserKeyCompleted(response.Data);
            });
        }


        public void CreateAccount(string email, string password, string name)
        {
            RestRequest request = new RestRequest("users/create", Method.POST);
            request.AddParameter("Nombre", name);
            request.AddParameter("Email", email);
            request.AddParameter("Password", password);
            client.ExecuteAsync(request, response =>
            {
                if (CreateAccountCompleted != null)
                    CreateAccountCompleted();
            });
        }


        public async Task<string> ShortenUrl(string url)
        {
            RestRequest request = new RestRequest();
            request.AddParameter("url", url);
            var task = new TaskCompletionSource<string>();
            shortenClient.ExecuteAsync(request, response =>
            {
                task.SetResult(response.Content);
            });
            string result = await task.Task;
            return result;
        }
    }
}
