using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBCSApp.Services.Settings
{
    public interface ISettingsService
    {
        bool Contains(string key);
        object Get(string key);
        void Set(string key, object val, bool save = true);    
    }
}
