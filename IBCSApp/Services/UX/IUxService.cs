using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBCSApp.Services.UX
{
    public interface IUxService
    {
        void ShowToastNotification(string title, string message);
        void ShowMessageBox(string title, string message);
    }
}
