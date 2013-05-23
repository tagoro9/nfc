using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBCSApp.Models
{
    public class LoginToken
    {
        private string token;
        public string Token
        {
            get { return token; }
            set
            {
                token = value;
            }
        }
    }
}
