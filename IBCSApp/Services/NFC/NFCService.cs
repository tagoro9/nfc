using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Networking.Proximity;
using System.Runtime.InteropServices.WindowsRuntime; 

namespace IBCSApp.Services.NFC
{
    public class NFCService : INFCService
    {
        public ProximityDevice pDevice;
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
                pDevice.DeviceArrived += pDevice_DeviceArrived;
                pDevice.DeviceDeparted += pDevice_DeviceDeparted;
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Device departed handle method
        /// </summary>
        /// <param name="sender"></param>
        private void pDevice_DeviceDeparted(ProximityDevice sender)
        {
            if (DeviceLeft != null)
            {
                DeviceLeft();
            }
        }

        /// <summary>
        /// Device arrived hadle method
        /// </summary>
        /// <param name="sender"></param>
        private void pDevice_DeviceArrived(ProximityDevice sender)
        {
            if (DeviceArrived != null)
            {
                DeviceArrived();
            }
        }

        /// <summary>
        /// Get the information about the NFC device which we are connected to
        /// </summary>
        /// <returns></returns>
        public string GetDeviceInfo()
        {
            return pDevice.DeviceId;
        }

        /// <summary>
        /// Publish a text message of a given type to another NFC device
        /// </summary>
        /// <param name="type"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public void PublishTextMessage(string type, string message)
        {
            DeliverTextMessage(type, message);
        }

        /// <summary>
        /// Stop sending the message that is being published
        /// </summary>
        /// <param name="messageId"></param>
        public void StopSendMessage(long messageId)
        {
            if (messageId != null)
            {
                pDevice.StopPublishingMessage(messageId);
            }
        }

        /// <summary>
        /// Notify that a NFC message we are subscribed to has been received
        /// </summary>
        public event MessageReceivedArgs MessageReceivedCompleted;

        /// <summary>
        /// Callback to run whenever a message is received
        /// </summary>
        /// <param name="device"></param>
        /// <param name="message"></param>
        private void MessageReceived(ProximityDevice device, ProximityMessage message)
        {
            if (MessageReceivedCompleted != null)
            {
                MessageReceivedCompleted(message);
            }
        }

        /// <summary>
        /// Subscribe to read certain types of NFC messages
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Unsubsctibe from reading certain types of NFC messages
        /// </summary>
        /// <param name="type"></param>
        public void UnsubscribeForMessage(string type)
        {
            if ((pDevice != null) && (subscriptionIds.ContainsKey(type)))
            {
                pDevice.StopSubscribingForMessage(subscriptionIds[type]);
            }
        }

        /// <summary>
        /// Write a text message to a tag
        /// </summary>
        /// <param name="type"></param>
        /// <param name="message"></param>
        public void WriteMessageToTag(string type, string message)
        {
            DeliverTextMessage(type + ":WriteTag", message);
        }

        /// <summary>
        /// Write a NDEF formatted message to a tag
        /// </summary>
        /// <param name="type"></param>
        /// <param name="message"></param>
        public void WriteNdefMessageToTag(NdefLibrary.Ndef.NdefMessage message)
        {
            DeliverBinaryMessage("NDEF:WriteTag", message.ToByteArray());
        }

        /// <summary>
        /// Publish a NDEF formatted message to another NFC device
        /// </summary>
        /// <param name="type"></param>
        /// <param name="message"></param>
        public void PublishNdefMessage(NdefLibrary.Ndef.NdefMessage message)
        {
           DeliverBinaryMessage("NDEF", message.ToByteArray());
        }

        /// <summary>
        /// Message published / written hadler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="messageId"></param>
        private void MessageWrittenHandler(ProximityDevice sender, long messageId)
        {
            pDevice.StopPublishingMessage(messageId);
            if (MessagePublishedCompleted != null)
            {
                MessagePublishedCompleted();
            }
        }

        /// <summary>
        /// Notify that a NFC message has been published / written
        /// </summary>
        public event MessagePublishedArgs MessagePublishedCompleted;

        /// <summary>
        /// Notify when a NFC device or tag is close
        /// </summary>
        public event DeviceArrivedArgs DeviceArrived;

        /// <summary>
        /// Notify when a NFC device or tag left
        /// </summary>
        public event DeviceLeftArgs DeviceLeft;

        /// <summary>
        /// Publish a binary message to another NFC device
        /// </summary>
        /// <param name="type"></param>
        /// <param name="message"></param>
        public void PublishBinaryMessage(string type, byte[] message)
        {
            DeliverBinaryMessage(type, message);
        }

        /// <summary>
        /// Write a binary message to a NFC tag
        /// </summary>
        /// <param name="type"></param>
        /// <param name="message"></param>
        public void WriteBinaryMessageToTag(string type, byte[] message)
        {
            DeliverBinaryMessage(type + ":WriteTag", message);
        }

        /// <summary>
        /// Deliver any kind of text message
        /// </summary>
        /// <param name="type"></param>
        /// <param name="message"></param>
        private void DeliverTextMessage(string type, string message)
        {
            pDevice.PublishMessage(type, message, MessageWrittenHandler);
        }

        /// <summary>
        /// Deliver any kind of binary message
        /// </summary>
        /// <param name="type"></param>
        /// <param name="message"></param>
        private void DeliverBinaryMessage(string type, byte[] message) 
        {
            pDevice.PublishBinaryMessage(type, message.AsBuffer(), MessageWrittenHandler);
        }

        /// <summary>
        /// Deliver a Uri type message
        /// </summary>
        /// <param name="message"></param>
        private void DeliverUriMessage(Uri message)
        {
            pDevice.PublishUriMessage(message, MessageWrittenHandler);
        }
    }
}
