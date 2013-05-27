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
            if (AesConfig.DecryptStringFromBytes_Aes(StringToByteArray(token)) != scope[0])
            {
                return false;
            }
            return true;
        }

        public static byte[] StringToByteArray(string hex)
        {
            return Enumerable.Range(0, hex.Length)
                             .Where(x => x % 2 == 0)
                             .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                             .ToArray();
        }
    }
}