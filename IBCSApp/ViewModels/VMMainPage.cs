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
    using IBCSApp.Models;
    using IBCS.BF.Key;
    using IBCSApp.Services.NFC;

    /// <summary>
    /// MainPage ViewModel.
    /// </summary>
    public class VMMainPage : VMBase
    {
        ///NFC message types
        public static string NFC_IBCS = "Windows.IBCS.Identity";

        //Services variables.
        private INavigationService navService;
        private IDispatcherService dispatcherService;
        private ISettingsService settingsService;
        private IkeysService keysService;
        private INFCService nfcService;

        //Commands variables.
        private DelegateCommand navigateToSecondPageCommand;
        private DelegateCommand<bool> setAsBusyCommand;
        private DelegateCommand logOutCommand;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="navService">Navigation service instance resolved by DI.</param>
        /// <param name="dispatcherService">Dispatcher service instance resolved by DI.</param>
        public VMMainPage(INavigationService navService, IDispatcherService dispatcherService, ISettingsService settingsService, IkeysService keysService, INFCService nfcService)
        {
            this.navService = navService;
            this.settingsService = settingsService;
            this.dispatcherService = dispatcherService;
            this.keysService = keysService;
            this.nfcService = nfcService;

            this.navigateToSecondPageCommand = new DelegateCommand(NavigateToSecondPageExecute);
            this.setAsBusyCommand = new DelegateCommand<bool>(SetAsBusyExecte);
            this.logOutCommand = new DelegateCommand(LogOutExecute);
        }

        private void LogOutExecute()
        {
            settingsService.Remove("email");
            settingsService.Remove("private");
            settingsService.Remove("token");
            navService.NavigateToLoginPage();
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
        }

        /// <summary>
        /// Request the server to send the user private key
        /// </summary>
        /// <param name="key">IBE User private key the server sent</param>
        private void settingsService_GetUserKeyCompleted(SerializedPrivateKey key)
        {
            settingsService.Set("private", key);
            dispatcherService.CallDispatcher(() => {
                IsBusy = false;
            });
        }

        /// <summary>
        /// Start publishing the user identity by NFC
        /// </summary>
        public void StartPublishingIdentity()
        {
            if (nfcService.ConnectDefaultProximityDevice())
            {
                nfcService.SubscribeToMessage(NFC_IBCS);
                nfcService.MessageReceivedCompleted += nfcService_MessageReceivedCompleted;
                nfcService.PublishTextMessage(NFC_IBCS, (string) settingsService.Get("email"));
            }
        }

        /// <summary>
        /// Handle a message reception through NFC
        /// </summary>
        /// <param name="message">Message received over NFC</param>
        private void nfcService_MessageReceivedCompleted(string message)
        {
            //Once we have the identity, we can unsubscribe from that channel
            int a = 1;
        }

    }
}
