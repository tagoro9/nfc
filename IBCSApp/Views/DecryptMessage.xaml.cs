using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using IBCSApp.ViewModels;

namespace IBCSApp.Views
{
    public partial class DecryptMessage : PhoneApplicationPage
    {
        public DecryptMessage()
        {
            InitializeComponent();
        }

        private VMDecryptMessage ViewModel
        {
            get
            {
                return this.DataContext as VMDecryptMessage;
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var entry = NavigationService.BackStack.FirstOrDefault();
            if (entry != null && entry.Source.OriginalString.Contains("Login"))
            {
                NavigationService.RemoveBackEntry();
            }

            if (NavigationContext.QueryString.ContainsKey("id") && NavigationContext.QueryString.ContainsKey("message"))
            {
                ViewModel.DecryptMessage(NavigationContext.QueryString["id"], NavigationContext.QueryString["message"]);
            }
        }
    }
}