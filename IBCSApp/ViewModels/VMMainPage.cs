namespace IBCSApp.ViewModels
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

    /// <summary>
    /// MainPage ViewModel.
    /// </summary>
    public class VMMainPage : VMBase
    {
        //Services variables.
        private INavigationService navService;
        private IDispatcherService dispatcherService;
        private ISettingsService settingsService;
        private IkeysService keysService;
        private INFCService nfcService;
        private IBluetoothService bluetoothService;
        private IBfService bfService;
        private IPairingService pairingService;

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

        private string identity;
        private string key;
        private string ct;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="navService">Navigation service instance resolved by DI.</param>
        /// <param name="dispatcherService">Dispatcher service instance resolved by DI.</param>
        public VMMainPage(INavigationService navService, IDispatcherService dispatcherService,
            ISettingsService settingsService, IkeysService keysService, INFCService nfcService,
            IBluetoothService bluetoothService, IBfService bfService, IPairingService pairingService)
        {
            this.navService = navService;
            this.settingsService = settingsService;
            this.dispatcherService = dispatcherService;
            this.keysService = keysService;
            this.nfcService = nfcService;
            this.bluetoothService = bluetoothService;
            this.bfService = bfService;
            this.pairingService = pairingService;

            this.navigateToSecondPageCommand = new DelegateCommand(NavigateToSecondPageExecute);
            this.setAsBusyCommand = new DelegateCommand<bool>(SetAsBusyExecte);
            this.logOutCommand = new DelegateCommand(LogOutExecute);
            this.navigateToSecureEmailCommand = new DelegateCommand(NavigateToSecureEmailExecute);
            this.navigateToShareSecureMessageCommand = new DelegateCommand(NavigateToShareSecureMessageExecute);
            this.startNfcPairingCommand = new DelegateCommand(StartNfcPairingExecute);

            Identity = "no identity...";
            Key = "no key...";
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

        public ICommand NavigateToShareSecureMessageCommand
        {
            get { return this.navigateToShareSecureMessageCommand; }
        }

        /// <summary>
        /// Command to be binded in UI, send a secure email.
        /// </summary>
        public ICommand NavigateToSecureEmailCommand
        {
            get { return this.navigateToSecureEmailCommand; }
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
                keysService.GetUserKeyCompleted += settingsService_GetUserKeyCompleted;
                keysService.GetUserKey((string) settingsService.Get("email"), (LoginToken) settingsService.Get("token"));
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
