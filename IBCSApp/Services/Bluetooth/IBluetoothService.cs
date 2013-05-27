using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Networking.Proximity;
using IBCSApp.Entities;
using Windows.Networking.Sockets;

namespace IBCSApp.Services.Bluetooth
{
    public interface IBluetoothService
    {
        PeerDiscoveryTypes StartBluetooth();

        Task<List<Peer>> FindPeers();
        //Task<List<Peer>> FindHardwarePeers();

        Task<StreamSocket> ConnectToDevice(List<Peer> peers, string identity);
    }
}
