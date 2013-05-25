using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IBCSApp.Entities;
using System.Threading.Tasks;

namespace IBCSApp.Services.API
{

    public delegate void LoginUserArgs(LoginToken token, string email);

    public interface ILoginService
    {
        void LoginUser(string email, string password);
        event LoginUserArgs LoginUserCompleted;
    }
}
