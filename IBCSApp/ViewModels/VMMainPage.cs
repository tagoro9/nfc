﻿namespace IBCSApp.ViewModels
{
    using System;
    using System.Linq;
    using System.Windows.Input;
    using IBCSApp.Services.Dispatcher;
    using IBCSApp.Services.Navigation;
    using IBCSApp.Services.API;
    using IBCSApp.ViewModels.Base;
    using System.IO.IsolatedStorage;
    using IBCSApp.Services.Settings;
    using IBCS.BF.Key;
    using IBCSApp.Services.NFC;
    using IBCSApp.Entities;
    using IBCSApp.Services.Bluetooth;
    using Windows.Networking.Proximity;
    using System.Collections.Generic;
    using IBCSApp.Services.BF;
    using System.Security.Cryptography;
    using IBCS.BF.Util;
    using Windows.Networking.Sockets;
    using Microsoft.Phone.Tasks;
    using IBCSApp.Services.UX;
    using System.Text;
    using System.Collections.ObjectModel;
    using NdefLibrary.Ndef;
    using System.Runtime.InteropServices.WindowsRuntime;
    using IBCSApp.Resources; 

    /// <summary>
    /// MainPage ViewModel.
    /// </summary>
    public class VMMainPage : VMBase
    {
        //Services variables.
        private INavigationService navService;
        private IDispatcherService dispatcherService;
        private ISettingsService settingsService;
        private IApiService apiService;
        private INFCService nfcService;
        private IBluetoothService bluetoothService;
        private IBfService bfService;
        private IPairingService pairingService;
        private IUxService uxService;

        private SerializedPrivateKey sKey;
        private StreamSocket socket;
        private AesManaged aes;

        //Commands variables.
        private DelegateCommand navigateToSecondPageCommand;
        private DelegateCommand<bool> setAsBusyCommand;
        private DelegateCommand logOutCommand;
        private DelegateCommand navigateToSecureEmailCommand;
        private DelegateCommand navigateToShareSecureMessageCommand;
        private DelegateCommand startNfcPairingCommand;
        private DelegateCommand navigateToAboutCommand;
        private DelegateCommand addressChooserCommand;
        private DelegateCommand sendSecureEmailCommand;
        private DelegateCommand shareSecureMessageCommand;
        private DelegateCommand writeToTagCommand;
        private DelegateCommand publishMessageCommand;
        private DelegateCommand encryptSecureNoteCommand;
        private DelegateCommand clearLogCommand;

        private string identity;
        private string key;
        private string ct;
        private bool keyAvailable;

        private string messageText;
        private string messageIdentity;

        private string destinataryEmail;
        private string destinataryName;
        private string emailSubject;
        private string emailBody;
        private string progressMessage;

        private string secureNoteText;
        private string secureNoteEncryptedText;
        private bool secureNoteEncrypted = false;
        private string shortenedNote;

        private string myIdentity;
        private SerializedPrivateKey myKey;

        private bool devicePresent = false;

        private ObservableCollection<NfcLogItem> nfcLogCollection = new ObservableCollection<NfcLogItem>();


        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="navService">Navigation service instance resolved by DI.</param>
        /// <param name="dispatcherService">Dispatcher service instance resolved by DI.</param>
        public VMMainPage(INavigationService navService, IDispatcherService dispatcherService,
            ISettingsService settingsService, IApiService apiService, INFCService nfcService,
            IBluetoothService bluetoothService, IBfService bfService, IPairingService pairingService, IUxService uxService)
        {
            this.navService = navService;
            this.settingsService = settingsService;
            this.dispatcherService = dispatcherService;
            this.apiService = apiService;
            this.nfcService = nfcService;
            this.bluetoothService = bluetoothService;
            this.bfService = bfService;
            this.pairingService = pairingService;
            this.uxService = uxService;

            this.navigateToSecondPageCommand = new DelegateCommand(NavigateToSecondPageExecute);
            this.setAsBusyCommand = new DelegateCommand<bool>(SetAsBusyExecte);
            this.logOutCommand = new DelegateCommand(LogOutExecute);
            this.navigateToSecureEmailCommand = new DelegateCommand(NavigateToSecureEmailExecute);
            this.navigateToShareSecureMessageCommand = new DelegateCommand(NavigateToShareSecureMessageExecute);
            this.startNfcPairingCommand = new DelegateCommand(StartNfcPairingExecute);
            this.navigateToAboutCommand = new DelegateCommand(NavigateToAboutExecute);
            this.addressChooserCommand = new DelegateCommand(AddressChooserExecute);
            this.sendSecureEmailCommand = new DelegateCommand(SendSecureEmailExecute);
            this.shareSecureMessageCommand = new DelegateCommand(ShareSecureMessageExecute);
            this.publishMessageCommand = new DelegateCommand(PublishMessageExecute, PublishMessageCanExecute);
            this.writeToTagCommand = new DelegateCommand(WriteToTagExecute, WriteToTagCanExecute);
            this.encryptSecureNoteCommand = new DelegateCommand(EncryptSecureNoteExecute);
            this.clearLogCommand = new DelegateCommand(ClearLogExecute);

            if (this.nfcService.ConnectDefaultProximityDevice())
            {
                this.nfcService.DeviceArrived += nfcService_DeviceArrived;
                this.nfcService.DeviceLeft += nfcService_DeviceLeft;
                this.nfcService.MessageReceivedCompleted += nfcService_MessageReceivedCompleted;
                this.nfcService.MessagePublishedCompleted += nfcService_MessagePublishedCompleted;
                this.nfcService.SubscribeToMessage("WriteableTag");
                this.nfcService.SubscribeToMessage("NDEF");
            }

            myIdentity = (string) settingsService.Get("email");
            myKey = (SerializedPrivateKey) settingsService.Get("private");

            Identity = "no identity...";
            Key = "no key...";
        }

        private void nfcService_MessagePublishedCompleted()
        {
            LogMessage(AppResources.NfcMessageWritten, NfcLogItem.CROSS_ICON);
        }

        private void nfcService_MessageReceivedCompleted(ProximityMessage message)
        {
            if (message.MessageType == "WriteableTag")
            {
                Int32 size = System.BitConverter.ToInt32(message.Data.ToArray(), 0);
                LogMessage(AppResources.NfcWriteableTagSize + " " + size + " bytes", NfcLogItem.INFO_ICON);
            }
        }

        private void ClearLogExecute()
        {
            NfcLogCollection.Clear();
        }

        private bool WriteToTagCanExecute()
        {
            return (SecureNoteEncrypted && devicePresent);
        }

        private bool PublishMessageCanExecute()
        {
            return (SecureNoteEncrypted && devicePresent);
        }

        private async void EncryptSecureNoteExecute()
        {
            IsBusy = true;
            ProgressMessage = AppResources.EncryptingMessage;
            secureNoteEncryptedText = await bfService.CipherText(SecureNoteText, myIdentity, myKey);
            string url = String.Format("ibcs:decrypt?message={0}&id={1}", secureNoteEncryptedText, myIdentity);
            shortenedNote = await apiService.ShortenUrl(url);
            dispatcherService.CallDispatcher(() => {
                IsBusy = false;
                ProgressMessage = String.Empty;
            });
            SecureNoteEncrypted = true;
        }

        /// <summary>
        /// Command to be binded in UI, log the user out.
        /// </summary>
        public ICommand LogOutCommand
        {
            get { return this.logOutCommand; }
        }

        /// <summary>
        /// Command to be binded in UI, execute navigation to second page.
        /// </summary>
        public ICommand NavigateToSecondPageCommand
        {
            get { return this.navigateToSecondPageCommand; }
        }

        /// <summary>
        /// Command to be binded in UI, set IsBusy to the passed value.
        /// </summary>
        public ICommand SetAsBusyCommand
        {
            get { return this.setAsBusyCommand; }
        }

        /// <summary>
        /// Command to be binded in UI, write a message to a NFC tag
        /// </summary>
        public ICommand WriteToTagCommand
        {
            get { return writeToTagCommand; }
        }

        /// <summary>
        /// Command to be binded in UI, navigate to page share secure message
        /// </summary>
        public ICommand NavigateToShareSecureMessageCommand
        {
            get { return this.navigateToShareSecureMessageCommand; }
        }

        /// <summary>
        /// Command to be binded in UI, navigate to page secure email
        /// </summary>
        public ICommand NavigateToSecureEmailCommand
        {
            get { return this.navigateToSecureEmailCommand; }
        }

        public ICommand ClearLogCommand
        {
            get { return clearLogCommand; }
        }

        /// <summary>
        /// Command to be binded in UI, publish a message to another NFC device
        /// </summary>
        public ICommand PublishMessageCommand
        {
            get { return publishMessageCommand; }
        }
        
        /// <summary>
        /// Command to be binded in UI, pick an email from the contact list
        /// </summary>
        public ICommand AddressChooserCommand
        {
            get { return this.addressChooserCommand; }
        }

        /// <summary>
        /// Command to be binded in UI, share a secure message in social networks
        /// </summary>
        public ICommand ShareSecureMessageCommand
        {
            get { return shareSecureMessageCommand; }
        }

        /// <summary>
        /// Command to be binded in UI, navigate to page called about
        /// </summary>
        public ICommand NavigateToAboutCommand
        {
            get { return this.navigateToAboutCommand; }
        }

        /// <summary>
        /// Command to be binded in UI, send a secure email
        /// </summary>
        public ICommand SendSecureEmailCommand
        {
            get { return sendSecureEmailCommand; }
        }

        /// <summary>
        /// Command to be binded in UI, encrypt a note
        /// </summary>
        public ICommand EncryptSecureNoteCommand
        {
            get { return encryptSecureNoteCommand; }
        }

        /// <summary>
        /// Handler for the nfcService DeviceLeft event
        /// </summary>
        private void nfcService_DeviceLeft()
        {
            DevicePresent = false;
            LogMessage(AppResources.NotifyDeviceDeparted, NfcLogItem.NFC_ICON);
            dispatcherService.CallDispatcher(() => uxService.ShowToastNotification("NFC", AppResources.NotifyDeviceDeparted));
        }

        /// <summary>
        /// Handler for the nfcService DeviceArrived event
        /// </summary>
        private void nfcService_DeviceArrived()
        {
            DevicePresent = true;
            LogMessage(AppResources.NotifyDeviceArrived, NfcLogItem.NFC_ICON);
            dispatcherService.CallDispatcher(() => uxService.ShowToastNotification("NFC", AppResources.NotifyDeviceArrived));
        }

        private void LogMessage(string message, string icon = "")
        {
            dispatcherService.CallDispatcher(() => NfcLogCollection.Add(new NfcLogItem(message, icon, DateTime.Now)));
        }

        private void WriteToTagExecute()
        {
            var spRecord = new NdefSpRecord
            {
                Uri = shortenedNote,
                NfcAction = NdefSpActRecord.NfcActionType.DoAction
            };
            spRecord.AddTitle(new NdefTextRecord
            {
                Text = "Not",
                LanguageCode = "es"
            });
            var msg = new NdefMessage { spRecord };
            nfcService.WriteNdefMessageToTag(msg);
        }

        private void PublishMessageExecute()
        {
            var spRecord = new NdefSpRecord
            {
                Uri = "http://msdn.microsoft.com/windows",
                NfcAction = NdefSpActRecord.NfcActionType.DoAction
            };
            spRecord.AddTitle(new NdefTextRecord
            {
                Text = "Windows Dev Center",
                LanguageCode = "es"
            });
            // Add record to NDEF message
            var msg = new NdefMessage { spRecord };
            nfcService.PublishNdefMessage(msg);
            //var uri = new Uri(" http://www.nokia.com/");
            ////encode uri to UTF16LE.
            //var buffer = Encoding.Unicode.GetBytes(uri.ToString());
            //nfcService.PublishBinaryMessage("WindowsUri", buffer);
            //NdefUriRecord rec = new NdefUriRecord { Uri = "http://www.nokia.com/" };
            //NdefMessage msg = new NdefMessage { rec };
            //nfcService.PublishNdefMessage(msg);
        }

        public ObservableCollection<NfcLogItem> NfcLogCollection
        {
            get { return nfcLogCollection; }
            set
            {
                nfcLogCollection = value;
                RaisePropertyChanged();
            }
        }

        public bool DevicePresent
        {
            get { return devicePresent; }
            set
            {
                devicePresent = value;
                RaisePropertyChanged();
                dispatcherService.CallDispatcher(() => {
                    publishMessageCommand.RaiseCanExecuteChanged();
                    writeToTagCommand.RaiseCanExecuteChanged();
                });
            }
        }

        public bool SecureNoteEncrypted
        {
            get { return secureNoteEncrypted; }
            set
            {
                secureNoteEncrypted = value;
                RaisePropertyChanged();
                dispatcherService.CallDispatcher(() =>
                {
                    publishMessageCommand.RaiseCanExecuteChanged();
                    writeToTagCommand.RaiseCanExecuteChanged();
                });
            }
        }

        public string SecureNoteText
        {
            get { return secureNoteText; }
            set
            {
                secureNoteText = value;
                SecureNoteEncrypted = false;
                RaisePropertyChanged();
            }
        }

        public string MessageText
        {
            get { return messageText; }
            set
            {
                messageText = value;
                RaisePropertyChanged();
            }
        }

        public string MessageIdentity
        {
            get { return messageIdentity; }
            set
            {
                messageIdentity = value;
                RaisePropertyChanged();
            }
        }

        public string ProgressMessage
        {
            get { return progressMessage; }
            set
            {
                progressMessage = value;
                RaisePropertyChanged();
            }
        }

        public string EmailSubject
        {
            get { return emailSubject; }
            set
            {
                emailSubject = value;
                RaisePropertyChanged();
            }
        }

        public string EmailBody
        {
            get { return emailBody; }
            set
            {
                emailBody = value;
                RaisePropertyChanged();
            }
        }

        public string DestinataryEmail
        {
            get { return destinataryEmail; }
            set
            {
                destinataryEmail = value;
                RaisePropertyChanged();
            }
        }

        public string DestinataryName
        {
            get { return destinataryName; }
            set
            {
                destinataryName = value;
                RaisePropertyChanged();
            }
        }

        private async void ShareSecureMessageExecute()
        {
            ShareLinkTask shareLink = new ShareLinkTask();
            string url = await apiService.ShortenUrl("ibcs:decrypt?message=esto_deberia_ser_el_mensaje_cifrado?id=i_identidad");
            shareLink.LinkUri = new Uri(url);
            //shareLink.LinkUri = new Uri("ibcs:decrypt?message=esto_deberia_ser_el_mensaje_cifrado?id=i_identidad");
            shareLink.Title = "IBCS cencrypted message";
            shareLink.Message = "Message intended to mi_identidad";
            shareLink.Show();
        }

        private void SendSecureEmailExecute()
        {
            //IsBusy = true;
            //ProgressMessage = "Encrypting message...";
            //SerializedPrivateKey sKey = (SerializedPrivateKey) settingsService.Get("private");
            //bfService.CipherTextCompleted += bfService_CipherTextCompleted;
            //bfService.CipherText(EmailBody, DestinataryEmail, sKey);
        }

        private void AddressChooserExecute()
        {
            EmailAddressChooserTask emailChooser = new EmailAddressChooserTask();
            emailChooser.Completed += emailChooser_Completed;
            emailChooser.Show();
        }

        private void emailChooser_Completed(object sender, EmailResult e)
        {
            DestinataryEmail = e.Email;
            DestinataryName = e.DisplayName;
        }

        private void NavigateToAboutExecute()
        {
            this.navService.NavigateToAboutPage();
        }

        public string Identity
        {
            get { return identity; }
            set
            {
                identity = value;
                RaisePropertyChanged();
            }
        }

        public bool KeyAvailable
        {
            get { return keyAvailable; }
            set
            {
                keyAvailable = value;
                RaisePropertyChanged();
            }
        }

        public string Key
        {
            get { return key; }
            set
            {
                key = value;
                RaisePropertyChanged();
            }
        }

        public ICommand StartNfcPairingCommand
        {
            get { return this.startNfcPairingCommand; }
        }

        private void StartNfcPairingExecute()
        {
            IsBusy = true;
            pairingService.PairingCompleted += pairingService_PairingCompleted;
            pairingService.NfcPairDevices();   
        }

        private void pairingService_PairingCompleted(string otherIdentity, AesManaged aes, StreamSocket socket)
        {
            this.socket = socket;
            this.aes = aes;
            dispatcherService.CallDispatcher(() => {
                IsBusy = false;
                Key = BFUtil.ToHexString(aes.Key);
                Identity = otherIdentity;
            });
        }

        private void NavigateToShareSecureMessageExecute()
        {
            this.navService.NavigateToShareSecureMessagePage();
        }

        /// <summary>
        /// 
        /// </summary>
        private void NavigateToSecureEmailExecute()
        {
            this.navService.NavigateToSecureEmailPage();
        }

        private void LogOutExecute()
        {
            settingsService.Remove("email");
            settingsService.Remove("private");
            settingsService.Remove("token");
            navService.NavigateToLoginPage();
        }


        /// <summary>
        /// Method to be executed by the NavigateToSecondPageCommand.
        /// Use the navigation service instance to navigate to another page.
        /// </summary>
        private void NavigateToSecondPageExecute()
        {
            this.navService.NavigateToSecondPage();
        }

        /// <summary>
        /// Method to be execute by the SetAsBusyCommand.
        /// </summary>
        /// <param name="parameter"></param>
        private void SetAsBusyExecte(bool parameter)
        {
            this.IsBusy = parameter;
        }

        /// <summary>
        /// Method to check if user keys have been obtained
        /// </summary>
        public void CheckKeys()
        {
            if (!settingsService.Contains("private"))
            {
                IsBusy = true;
                apiService.GetUserKeyCompleted += settingsService_GetUserKeyCompleted;
                apiService.GetUserKey((string) settingsService.Get("email"), (LoginToken) settingsService.Get("token"));
            }
            else
            {
                sKey = (SerializedPrivateKey) settingsService.Get("private");
            }
        }

        /// <summary>
        /// Request the server to send the user private key
        /// </summary>
        /// <param name="key">IBE User private key the server sent</param>
        private void settingsService_GetUserKeyCompleted(SerializedPrivateKey key)
        {
            settingsService.Set("private", key);
            sKey = key;
            dispatcherService.CallDispatcher(() => {
                IsBusy = false;
            });
        }

        /// <summary>
        /// Find bluetooth peers to connect to
        /// </summary>
        private async void StartBluetoothPairing()
        {
            if (bluetoothService.StartBluetooth() != PeerDiscoveryTypes.None)
            {
                List<Peer> peersFound = await bluetoothService.FindPeers();
            }
        }

    }
}
