namespace TicTacToe.Services.Navigation
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
    }
}
