using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PKGServer.Authorization
{
    public class AccessTokenValidator : IAccessTokenValidator
    {
        public bool ValidateToken(string token, string[] scope)
        {
            // replace this logic with dataBase access to table with tokens
            if (AesConfig.DecryptStringFromBytes_Aes(Convert.FromBase64String(token)) != scope[0])
            {
                return false;
            }
            return true;
        }
    }
}