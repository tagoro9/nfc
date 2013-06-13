namespace TicTacToe.Services.Navigation
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
    }
}
