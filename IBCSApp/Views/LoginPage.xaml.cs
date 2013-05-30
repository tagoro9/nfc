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
            //Clear backstack
            while (this.NavigationService.BackStack.Any())
            {
                this.NavigationService.RemoveBackEntry();
            }

            if (IsolatedStorageSettings.ApplicationSettings.Contains("email"))
            {
                NavigationService.Navigate(new Uri("/Views/MainPage.xaml", UriKind.Relative));
                return;
            }
            else {
                LayoutRoot.Visibility = System.Windows.Visibility.Visible;    
            }

            if (NavigationContext.QueryString.ContainsKey("destination"))
            {
                ViewModel.Destination = NavigationContext.QueryString["destination"];
            }
            else if (NavigationContext.QueryString.ContainsKey("created"))
            {
                string message = NavigationContext.QueryString["created"];
                ViewModel.DeliverToastNotification(AppResources.CreateAccountCreatedTitle, AppResources.CreateAccountCreatedMessage);
            }
        }
    }
}