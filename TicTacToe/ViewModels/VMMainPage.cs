namespace TicTacToe.ViewModels
{
    using System;
    using System.Linq;
    using System.Windows.Input;
    using TicTacToe.Services.Dispatcher;
    using TicTacToe.Services.Navigation;
    using TicTacToe.ViewModels.Base;

    /// <summary>
    /// MainPage ViewModel.
    /// </summary>
    public class VMMainPage : VMBase
    {
        //Services variables.
        private INavigationService navService;
        private IDispatcherService dispatcherService;

        //Commands variables.
        private DelegateCommand navigateToSecondPageCommand;
        private DelegateCommand<bool> setAsBusyCommand;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="navService">Navigation service instance resolved by DI.</param>
        /// <param name="dispatcherService">Dispatcher service instance resolved by DI.</param>
        public VMMainPage(INavigationService navService, IDispatcherService dispatcherService)
        {
            this.navService = navService;
            this.dispatcherService = dispatcherService;

            this.navigateToSecondPageCommand = new DelegateCommand(NavigateToSecondPageExecute);
            this.setAsBusyCommand = new DelegateCommand<bool>(SetAsBusyExecte);
        }

        /// <summary>
        /// Command to be binded in UI, execute navigation to second page.
        /// </summary>
        public ICommand NavigateToSecondPageCommand
        {
            get { return this.navigateToSecondPageCommand; }
        }

        /// <summary>
        /// Command to be binded in UI, set IsBusy to the passed value.
        /// </summary>
        public ICommand SetAsBusyCommand
        {
            get { return this.setAsBusyCommand; }
        }

        /// <summary>
        /// Method to be executed by the NavigateToSecondPageCommand.
        /// Use the navigation service instance to navigate to another page.
        /// </summary>
        private void NavigateToSecondPageExecute()
        {
            this.navService.NavigateToSecondPage();
        }

        /// <summary>
        /// Method to be execute by the SetAsBusyCommand.
        /// </summary>
        /// <param name="parameter"></param>
        private void SetAsBusyExecte(bool parameter)
        {
            this.IsBusy = parameter;
        }
    }
}
