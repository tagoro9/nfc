using System;
using System.Collections.Generic;
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

            if (tempUri.Contains("ibcs:decrypt"))
            {
                return new Uri("/Views/DecryptMessage.xaml", UriKind.Relative);
            }

            return uri;
        }
    }
}
