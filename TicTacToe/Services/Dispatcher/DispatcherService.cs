namespace TicTacToe.Services.Dispatcher
{
    using System;

    /// <summary>
    /// IDispatcherService contract implementation for Windows Phone 8.
    /// </summary>
    public class DispatcherService : IDispatcherService
    {
        /// <summary>
        /// Method implementation to call code using the UI Thread.
        /// </summary>
        /// <param name="action">Code to be executed.</param>
        public void CallDispatcher(Action action)
        {
            App.Dispatcher.BeginInvoke(() =>
            {
                action();
            });
        }
    }
}
