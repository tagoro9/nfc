using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Networking.Proximity;

namespace IBCSApp.Services.NFC
{
    public class NFCService : INFCService
    {
        public ProximityDevice pDevice;
        private long textMessageId;
        private Dictionary<string, long> subscriptionIds = new Dictionary<string,long>();

        /// <summary>
        /// Connect to the default NFC device if it exists
        /// </summary>
        /// <returns></returns>
        public bool ConnectDefaultProximityDevice()
        {
            pDevice = Windows.Networking.Proximity.ProximityDevice.GetDefault();
            if (pDevice != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public string GetDeviceInfo()
        {
            return pDevice.DeviceId;
        }

        public void PublishTextMessage(string type, string message)
        {
            textMessageId = pDevice.PublishMessage(type, message);
        }

        public void StopSendMessage()
        {
            if (textMessageId != null)
            {
                pDevice.StopPublishingMessage(textMessageId);
            }
        }

        public event MessageReceivedArgs MessageReceivedCompleted;

        public void MessageReceived(ProximityDevice device, ProximityMessage message)
        {
            if (MessageReceivedCompleted != null)
            {
                MessageReceivedCompleted(message.DataAsString);
            }
        }

        public bool SubscribeToMessage(string type)
        {
            if (pDevice == null)
            {
                pDevice = Windows.Networking.Proximity.ProximityDevice.GetDefault();

                if (pDevice == null)
                {
                    return false;
                }
            }
            subscriptionIds[type] = pDevice.SubscribeForMessage(type, MessageReceived);
            return true;
        }

        public void UnsubscribeForMessage(string type)
        {
            if ((pDevice != null) && (subscriptionIds.ContainsKey(type)))
            {
                pDevice.StopSubscribingForMessage(subscriptionIds[type]);
            }
        }

    }
}
