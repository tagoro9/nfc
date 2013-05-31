using System;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;

namespace IBCSApp.Mappers
{
    public class ProtocolUriMapper : UriMapperBase
    {
        public override Uri MapUri(Uri uri)
        {
            string tempUri = HttpUtility.UrlDecode(uri.ToString());
            string destination = tempUri;
            if (tempUri.Contains("ibcs:decrypt"))
            {
                destination = "/Views/DecryptMessage.xaml";
                string query = "";
                if (tempUri.IndexOf("{") == -1)
                {
                    query = tempUri.Substring(tempUri.IndexOf("?message="));
                }
                else
                {
                    query = "?message=" + HttpUtility.UrlEncode(tempUri.Substring(tempUri.IndexOf("{"), tempUri.IndexOf("&id") - tempUri.IndexOf("={") - 1));
                    query += "&id=" + HttpUtility.UrlEncode(tempUri.Substring(tempUri.IndexOf("&id=") + 4));
                }
                destination += query;
                if (IsolatedStorageSettings.ApplicationSettings.Contains("email"))
                {
                    return new Uri(destination, UriKind.Relative);
                }
                else
                {
                    return new Uri("/Views/LoginPage.xaml?destination=" + destination, UriKind.Relative);  
                }
            }
            return uri;
        }
    }
}
