namespace IBCSApp.Services.Dispatcher
{
    using System;

    /// <summary>
    /// This service provides access to execute code in the UI Thread
    /// from our ViewModel, isolating the platform specific use of 
    /// Dispatcher.
    /// </summary>
    public interface IDispatcherService
    {
        /// <summary>
        /// Use the dispatcher to execute the indicated code.
        /// </summary>
        /// <param name="action">Code to be executed.</param>
        void CallDispatcher(Action action);
    }
}
