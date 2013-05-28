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
        public void NavigateToLoginPage(string param = "")
        {
            App.RootFrame.Navigate(new Uri("/Views/LoginPage.xaml?" + param, UriKind.Relative));
        }

        /// <summary>
        /// Method implementation to navigate to page called secure email
        /// </summary>
        public void NavigateToSecureEmailPage()
        {
            App.RootFrame.Navigate(new Uri("/Views/SecureEmailPage.xaml", UriKind.Relative));
        }

        /// <summary>
        /// Method implementation to navigate to page called share secure message
        /// </summary>
        public void NavigateToShareSecureMessagePage()
        {
            App.RootFrame.Navigate(new Uri("/Views/ShareSecureMessagePage.xaml", UriKind.Relative));
        }

        /// <summary>
        /// Method implementation to navigate to page called create account
        /// </summary>
        public void NavigateToCreateAccountPage()
        {
            App.RootFrame.Navigate(new Uri("/Views/CreateAccountPage.xaml", UriKind.Relative));
        }
    }
}
