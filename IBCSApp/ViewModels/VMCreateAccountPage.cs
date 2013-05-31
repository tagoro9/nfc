using IBCSApp.Resources;
using IBCSApp.Services.API;
using IBCSApp.Services.Dispatcher;
using IBCSApp.Services.Navigation;
using IBCSApp.ViewModels.Base;
using Microsoft.Phone.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace IBCSApp.ViewModels
{
    public class VMCreateAccountPage : VMBase
    {
        //Services
        IApiService apiService;
        IDispatcherService dispatcherService;
        INavigationService navService;

        //Commands
        private DelegateCommand createAccountCommand;
        private DelegateCommand navigateToServiceConditionsCommand;

        //View data
        private string name;
        private string email;
        private string password;
        private string passwordConfirmation;
        private bool serviceConditions;

        public VMCreateAccountPage(IApiService apiService, IDispatcherService dispatcherService, INavigationService navService)
        {
            this.apiService = apiService;
            this.dispatcherService = dispatcherService;
            this.navService = navService;

            this.createAccountCommand = new DelegateCommand(CreateAccountExecute);
            this.navigateToServiceConditionsCommand = new DelegateCommand(NavigateToServiceConditionsExecute);
        }

        private void NavigateToServiceConditionsExecute()
        {
            WebBrowserTask task = new WebBrowserTask();
            task.Uri = new Uri(AppResources.CreateAccountServiceConditionsUrl);
            task.Show();
        }

        public ICommand NavigateToServiceConditionsCommand
        {
            get { return navigateToServiceConditionsCommand; }
        }

        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                RaisePropertyChanged();
            }
        }

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

        public string PasswordConfirmation
        {
            get { return passwordConfirmation; }
            set
            {
                passwordConfirmation = value;
                RaisePropertyChanged();
            }
        }

        public bool ServiceConditions
        {
            get { return serviceConditions; }
            set
            {
                serviceConditions = value;
                RaisePropertyChanged();
            }
        }

        public ICommand CreateAccountCommand
        {
            get { return this.createAccountCommand; }
        }

        private void CreateAccountExecute()
        {
            //We first validate the data
            if (Name == String.Empty || Name == null || Email == String.Empty || Name == null)
            {
                MessageBox.Show(AppResources.CreateAccountDataError);
            }
            else if (Password == String.Empty || Password != PasswordConfirmation || Password == null)
            {
                MessageBox.Show(AppResources.CreateAccountPasswordError);
            }
            else if (ServiceConditions == false)
            {
                MessageBox.Show(AppResources.CreateAccountServiceConditionsError);
            }
            else
            {
                apiService.CreateAccountCompleted += apiService_CreateAccountCompleted;
                IsBusy = true;
                apiService.CreateAccount(Email, Password, Name);
            }
        }

        private void apiService_CreateAccountCompleted()
        {
            dispatcherService.CallDispatcher(() => IsBusy = false);
            navService.NavigateToLoginPage("created=true");
        }
    }
}
