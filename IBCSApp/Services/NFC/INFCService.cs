using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Networking.Proximity;

namespace IBCSApp.Services.NFC
{
    public delegate void MessageReceivedArgs(string message);

    public interface INFCService
    {
        bool ConnectDefaultProximityDevice();

        void PublishTextMessage(string type, string message);

        string GetDeviceInfo();

        void StopSendMessage();

        bool SubscribeToMessage(string type);

        void UnsubscribeForMessage(string type);

        event MessageReceivedArgs MessageReceivedCompleted;
    }
}
