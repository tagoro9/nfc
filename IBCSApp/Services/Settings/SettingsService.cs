using System;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBCSApp.Services.Settings
{
    public class SettingsService : ISettingsService
    {
        private static IsolatedStorageSettings settings = IsolatedStorageSettings.ApplicationSettings;
         
        public bool Contains(string key)
        {
            return settings.Contains(key);
        }

        public object Get(string key)
        {
            return settings[key];
        }

        public void Set(string key, object val, bool save = true)
        {
            if (settings.Contains(key))
            {
                settings[key] = val;
            }
            else
            {
                settings.Add(key, val);
            }
            if (save)
            {
                settings.Save();
            }
        }
    }
}
