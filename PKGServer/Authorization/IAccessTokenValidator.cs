using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PKGServer.Authorization
{
    interface IAccessTokenValidator
    {
        bool ValidateToken(string token, string[] scope);
    }
}
