using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Networking.Proximity;
using IBCSApp.Entities;

namespace IBCSApp.Services.Bluetooth
{
    public interface IBluetoothService
    {
        PeerDiscoveryTypes StartBluetooth();

        Task<List<Peer>> FindPeers();
        //Task<List<Peer>> FindHardwarePeers();

        //void ConnectToDevice(PeerInformation peer);
    }
}
