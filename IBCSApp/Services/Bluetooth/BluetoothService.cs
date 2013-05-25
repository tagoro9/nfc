using IBCSApp.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Windows.Networking.Proximity;
using IBCSApp.Resources;

namespace IBCSApp.Services.Bluetooth
{
    public class BluetoothService : IBluetoothService
    {
        public PeerDiscoveryTypes StartBluetooth()
        {
            PeerDiscoveryTypes type = PeerDiscoveryTypes.None;

            PeerFinder.Start();

            type = PeerFinder.SupportedDiscoveryTypes;

            return type;
        }

        public async Task<List<Peer>> FindPeers()
        {
            List<Peer> data = null;

            try
            {
                var peer = await PeerFinder.FindAllPeersAsync();

                data = (from p in peer
                        select new Peer()
                        {
                            Name = p.DisplayName,
                            Information = p
                        }).ToList();
            }
            catch (Exception ex)
            {
                if ((uint)ex.HResult == 0x8007048F)
                {
                    if (MessageBox.Show(AppResources.BluetoothActivateMessage, AppResources.BluetoothActivateCaption, MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                    {
                        Microsoft.Phone.Tasks.ConnectionSettingsTask cst = new Microsoft.Phone.Tasks.ConnectionSettingsTask();
                        cst.ConnectionSettingsType = Microsoft.Phone.Tasks.ConnectionSettingsType.Bluetooth;
                        cst.Show();
                    }
                }
            }

            return data;
        }
    }
}
