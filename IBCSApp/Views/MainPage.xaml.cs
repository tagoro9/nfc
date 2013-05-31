using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using IBCSApp.Resources;
using IBCSApp.ViewModels;
using System.Windows.Interactivity;

namespace IBCSApp.Views
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();
        }

        private VMMainPage ViewModel
        {
            get
            {
                return this.DataContext as VMMainPage;
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var entry = NavigationService.BackStack.FirstOrDefault();
            if (entry != null && entry.Source.OriginalString.Contains("Login"))
            {
                NavigationService.RemoveBackEntry();   
            }
            ((VMMainPage)this.DataContext).CheckKeys();
            //((VMMainPage)this.DataContext).StartPublishingIdentity();
        }

        private void PhoneTextBox_ActionIconTapped(object sender, EventArgs e)
        {
            ViewModel.AddressChooserCommand.Execute(null);
        }
    }
}