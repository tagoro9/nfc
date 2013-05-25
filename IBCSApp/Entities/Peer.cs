using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Networking.Proximity;

namespace IBCSApp.Entities
{
    public class Peer
    {
        public string Name { get; set; }
        public PeerInformation Information { get; set; }
    }
}
