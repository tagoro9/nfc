using IBCSApp.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Windows.Networking.Proximity;
using IBCSApp.Resources;
using IBCSApp.Services.Settings;
using Windows.Networking.Sockets;
using IBCSApp.Services.Dispatcher;

namespace IBCSApp.Services.Bluetooth
{
    public class BluetoothService : IBluetoothService
    {

        private ISettingsService settingsService;
        private IDispatcherService dispatcherService;

        public BluetoothService(ISettingsService settingsService, IDispatcherService dispatcherService)
        {
            this.settingsService = settingsService;
            this.dispatcherService = dispatcherService;
        }

        public PeerDiscoveryTypes StartBluetooth()
        {
            PeerDiscoveryTypes type = PeerDiscoveryTypes.None;
            PeerFinder.ConnectionRequested += PeerFinder_ConnectionRequested;
            PeerFinder.DisplayName = (string) settingsService.Get("email");
            PeerFinder.Start();
            type = PeerFinder.SupportedDiscoveryTypes;

            return type;
        }

        private void PeerFinder_ConnectionRequested(object sender, ConnectionRequestedEventArgs args)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Peer>> FindPeers()
        {
            List<Peer> data = null;

            var peer = await PeerFinder.FindAllPeersAsync();

            data = (from p in peer
                    select new Peer()
                    {
                        Name = p.DisplayName,
                        Information = p
                    }).ToList();

            return data;

            //List<Peer> data = null;

            //try
            //{
            //    var peer = await PeerFinder.FindAllPeersAsync();

            //    data = (from p in peer
            //            select new Peer()
            //            {
            //                Name = p.DisplayName,
            //                Information = p
            //            }).ToList();
            //}
            //catch (Exception ex)
            //{
            //    if ((uint)ex.HResult == 0x8007048F)
            //    {
            //        dispatcherService.CallDispatcher(() => {
            //            if (MessageBox.Show(AppResources.BluetoothActivateMessage, AppResources.BluetoothActivateCaption, MessageBoxButton.OKCancel) == MessageBoxResult.OK)
            //            {
            //                Microsoft.Phone.Tasks.ConnectionSettingsTask cst = new Microsoft.Phone.Tasks.ConnectionSettingsTask();
            //                cst.ConnectionSettingsType = Microsoft.Phone.Tasks.ConnectionSettingsType.Bluetooth;
            //                cst.Show();
            //            }
            //        });
            //    }
            //    return null;
            //}
            //return data;
        }


        public async Task<StreamSocket> ConnectToDevice(PeerInformation peer)
        {
            return await PeerFinder.ConnectAsync(peer);
            //Peer myPeer = peers.First(s => s.Name == identity);
            //return await PeerFinder.ConnectAsync(myPeer.Information);
        }
    }
}
