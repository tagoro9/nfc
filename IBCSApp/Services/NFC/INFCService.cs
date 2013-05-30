using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Networking.Proximity;
using NdefLibrary.Ndef;

namespace IBCSApp.Services.NFC
{
    public delegate void MessageReceivedArgs(ProximityMessage message);
    public delegate void MessagePublishedArgs();
    public delegate void DeviceArrivedArgs();
    public delegate void DeviceLeftArgs();

    public interface INFCService
    {
        /// <summary>
        /// Connect to the default NFC device if it exists
        /// </summary>
        /// <returns></returns>
        bool ConnectDefaultProximityDevice();

        /// <summary>
        /// Publish a text message of a given type to another NFC device
        /// </summary>
        /// <param name="type"></param>
        /// <param name="message"></param>
        void PublishTextMessage(string type, string message);

        /// <summary>
        /// Publish a binary message of a given type to another NFC device
        /// </summary>
        /// <param name="type"></param>
        /// <param name="message"></param>
        void PublishBinaryMessage(string type, byte[] message);

        /// <summary>
        /// Write a binary message of a given type to a NFC tag
        /// </summary>
        /// <param name="type"></param>
        /// <param name="message"></param>
        void WriteBinaryMessageToTag(string type, byte[] message);

        /// <summary>
        /// Write a text message to a tag
        /// </summary>
        /// <param name="type"></param>
        /// <param name="message"></param>
        void WriteMessageToTag(string type, string message);

        /// <summary>
        /// Write a NDEF formatted message to a tag
        /// </summary>
        /// <param name="type"></param>
        /// <param name="message"></param>
        void WriteNdefMessageToTag(NdefMessage message);

        /// <summary>
        /// Publish a NDEF formatted message to another NFC device
        /// </summary>
        /// <param name="type"></param>
        /// <param name="message"></param>
        void PublishNdefMessage(NdefMessage message);

        /// <summary>
        /// Get the information about the NFC device which we are connected to
        /// </summary>
        /// <returns></returns>
        string GetDeviceInfo();

        /// <summary>
        /// Stop sending the message that is being published
        /// </summary>
        void StopSendMessage(long messageId);

        /// <summary>
        /// Subscribe to read certain types of NFC messages
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        bool SubscribeToMessage(string type);

        /// <summary>
        /// Unsubsctibe from reading certain types of NFC messages
        /// </summary>
        /// <param name="type"></param>
        void UnsubscribeForMessage(string type);

        /// <summary>
        /// Notify that a NFC message we are subscribed to has been received
        /// </summary>
        event MessageReceivedArgs MessageReceivedCompleted;

        /// <summary>
        /// Notify that a NFC message has been published / written
        /// </summary>
        event MessagePublishedArgs MessagePublishedCompleted;

        /// <summary>
        /// Notify when a NFC device or tag is close
        /// </summary>
        event DeviceArrivedArgs DeviceArrived;
        /// <summary>
        /// Notify when a NFC device or tag left
        /// </summary>
        event DeviceLeftArgs DeviceLeft;
    }
}
