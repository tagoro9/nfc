using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.IO.IsolatedStorage;
using IBCSApp.ViewModels;
using IBCSApp.Resources;

namespace IBCSApp.Views
{
    public partial class LoginPage : PhoneApplicationPage
    {
        public LoginPage()
        {
            InitializeComponent();
        }

        private VMLoginPage ViewModel
        {
            get
            {
                return this.DataContext as VMLoginPage;
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (NavigationContext.QueryString.Any())
            {
                string message = NavigationContext.QueryString["created"];
                ViewModel.DeliverToastNotification(AppResources.CreateAccountCreatedTitle, AppResources.CreateAccountCreatedMessage);
            }
            if (!ViewModel.CheckLoggedIn())
            {
                NavigationService.RemoveBackEntry();   
            }
        }
    }
}