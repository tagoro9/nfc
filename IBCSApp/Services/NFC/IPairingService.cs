using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Windows.Networking.Sockets;
using System.Threading.Tasks;

namespace IBCSApp.Services.NFC
{
    public delegate void PairingCompletedArgs(string otherIdentity, AesManaged aes, StreamSocket socket);
    public delegate void NfcPairingCompletedArgs();

    public interface IPairingService
    {
        event PairingCompletedArgs PairingCompleted;
        void NfcPairDevices();
    }
}
