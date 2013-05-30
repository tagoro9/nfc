namespace IBCSApp.Services.Navigation
{
    /// <summary>
    /// This service encapsulate page navigation and exposes it to ViewModels
    /// in a MVVM friendly manner.
    /// Using this interface your ViewModel is technology agnostic and can be reused
    /// between technologies without dependencies, simply implementing the behavior
    /// you want in each platform.
    /// </summary>
    public interface INavigationService
    {
        /// <summary>
        /// Navigate to second page.
        /// </summary>
        void NavigateToSecondPage();
        /// <summary>
        /// Navigate to main page.
        /// </summary>
        void NavigateToMainPage();
        /// <summary>
        /// Navigate to login page.
        /// </summary>
        void NavigateToLoginPage(string param = "");
        /// <summary>
        /// Navigate to secure email page.
        /// </summary>
        void NavigateToSecureEmailPage();

        void NavigateToShareSecureMessagePage();
        /// <summary>
        /// Navigate to create account page
        /// </summary>
        void NavigateToCreateAccountPage();
        /// <summary>
        /// Navigate to about page
        /// </summary>
        void NavigateToAboutPage();
        /// <summary>
        /// Navigate to specific uri
        /// </summary>
        /// <param name="uri"></param>
        void NavigateToUri(string uri);
    }
}
