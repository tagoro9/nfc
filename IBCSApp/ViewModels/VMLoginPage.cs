using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IBCSApp.ViewModels.Base;
using IBCSApp.Entities;
using IBCSApp.Services.Navigation;
using IBCSApp.Services.Settings;
using IBCSApp.Services.Dispatcher;
using System.Windows.Input;
using System.Net;
using System.IO;
using IBCSApp.Services.API;
using System.IO.IsolatedStorage;
using IBCSApp.Services.UX;
using IBCSApp.Resources;

namespace IBCSApp.ViewModels 
{
    public class VMLoginPage : VMBase
    {
        //Services variables.
        private INavigationService navService;
        private IDispatcherService dispatcherService;
        private IApiService apiService;
        private ISettingsService settingsService;
        private IUxService uxService;

        //Commands variables.
        private DelegateCommand loginUserCommand;
        private DelegateCommand<bool> setAsBusyCommand;
        private DelegateCommand createAccountCommand;


        private string destination;
        public string Destination
        {
            get { return destination; }
            set { destination = value; }
        }

        private string email;
        private string password;
        public string Email
        {
            get { return email; }
            set
            {
                email = value;
                RaisePropertyChanged();
            }
        }

        public string Password
        {
            get { return password; }
            set
            {
                password = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="navService">Navigation service instance resolved by DI.</param>
        /// <param name="dispatcherService">Dispatcher service instance resolved by DI.</param>
        public VMLoginPage(INavigationService navService, IDispatcherService dispatcherService, IApiService apiService, ISettingsService settingsService, IUxService uxService)
        {
            this.navService = navService;
            this.dispatcherService = dispatcherService;
            this.apiService = apiService;
            this.settingsService = settingsService;
            this.uxService = uxService;

            //this.navigateToRegisterPageCommand = new DelegateCommand(NavigateToRegisterPageExecute);
            this.loginUserCommand = new DelegateCommand(LoginUserExecute);
            this.setAsBusyCommand = new DelegateCommand<bool>(SetAsBusyExecte);
            this.createAccountCommand = new DelegateCommand(CreateAccountExecute);
        }

        public void DeliverToastNotification(string title, string message)
        {
            //this.uxService.ShowToastNotification(title, message);
            this.uxService.ShowMessageBox(title, message);
        }

        private void CreateAccountExecute()
        {
            this.navService.NavigateToCreateAccountPage();
        }

        public ICommand CreateAccountCommand
        {
            get { return this.createAccountCommand; }
        }

        /// <summary>
        /// Command to be binded in UI, set IsBusy to the passed value.
        /// </summary>
        public ICommand SetAsBusyCommand
        {
            get { return this.setAsBusyCommand; }
        }

        public ICommand LoginUserCommand
        {
            get { return this.loginUserCommand; }
        }

        /// <summary>
        /// Method to be executed by the SetAsBusyCommand.
        /// </summary>
        /// <param name="parameter"></param>
        private void SetAsBusyExecte(bool parameter)
        {
            this.IsBusy = parameter;
        }

        /// <summary>
        /// Method to be executed by the LoginUserCommand.
        /// </summary>
        /// <param name="parameter"></param>
        private void LoginUserExecute()
        {
            IsBusy = true;
            this.apiService.LoginUserCompleted += loginService_LoginUserCompleted;
            this.apiService.LoginUser(Email, Password);
        }

        private void loginService_LoginUserCompleted(LoginToken token, string email)
        {
            this.dispatcherService.CallDispatcher(() =>
            {
                if (token.Token != "" && token.Token != null) //Successfully Logged in, store token in settings
                {
                    settingsService.Set("token", token);
                    settingsService.Set("email", email);
                    if (destination != null)
                    {
                        navService.NavigateToUri(destination);
                    }
                    else
                    {
                        navService.NavigateToMainPage();
                    }
                }
                else
                {
                    uxService.ShowMessageBox(AppResources.LoginPageErrorTitle, AppResources.LoginPageErrorMessage);
                }
                IsBusy = false;
            });
        }
    }
}
