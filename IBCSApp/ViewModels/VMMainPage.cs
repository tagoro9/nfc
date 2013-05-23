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

        //Commands variables.
        private DelegateCommand navigateToSecondPageCommand;
        private DelegateCommand<bool> setAsBusyCommand;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="navService">Navigation service instance resolved by DI.</param>
        /// <param name="dispatcherService">Dispatcher service instance resolved by DI.</param>
        public VMMainPage(INavigationService navService, IDispatcherService dispatcherService, ISettingsService settingsService, IkeysService keysService)
        {
            this.navService = navService;
            this.settingsService = settingsService;
            this.dispatcherService = dispatcherService;
            this.keysService = keysService;

            this.navigateToSecondPageCommand = new DelegateCommand(NavigateToSecondPageExecute);
            this.setAsBusyCommand = new DelegateCommand<bool>(SetAsBusyExecte);
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

        private void settingsService_GetUserKeyCompleted(BFUserPrivateKey key)
        {
            settingsService.Set("private", key.Serialize());
            dispatcherService.CallDispatcher(() => {
                IsBusy = false;
            });
        }

    }
}
