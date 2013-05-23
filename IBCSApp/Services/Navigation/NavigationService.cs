namespace IBCSApp.Services.Navigation
{
    using System;

    /// <summary>
    /// INavigationService contract implementation for Windows Phone 8.
    /// </summary>
    public class NavigationService : INavigationService
    {
        /// <summary>
        /// Method implementation to navigate to page called SecondPage.
        /// </summary>
        public void NavigateToSecondPage()
        {
            App.RootFrame.Navigate(new Uri("/Views/SecondPage.xaml", UriKind.Relative));
        }

        /// <summary>
        /// Method implementation to navigate to page called MainPage
        /// </summary>
        public void NavigateToMainPage()
        {
            App.RootFrame.Navigate(new Uri("/Views/MainPage.xaml", UriKind.Relative));
        }

        /// <summary>
        /// Method implementation to navigate to page called LoginPage
        /// </summary>
        public void NavigateToLoginPage()
        {
            App.RootFrame.Navigate(new Uri("/Views/LoginPage.xaml", UriKind.Relative));
        }
    }
}
