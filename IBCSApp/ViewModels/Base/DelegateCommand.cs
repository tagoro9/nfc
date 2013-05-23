namespace IBCSApp.ViewModels.Base
{
    using System;
    using System.Windows.Input;

    /// <summary>
    /// ICommand implementation to reuse among all our ViewModels.
    /// </summary>
    public class DelegateCommand : ICommand
    {
        private Action execute;
        private Func<bool> canExecute;

        /// <summary>
        /// Constructor without can execute.
        /// </summary>
        /// <param name="execute">Action to be launched when the command is executed.</param>
        public DelegateCommand(Action execute)
            : this(execute, null) { }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="execute">Action to be launched when the command is executed.</param>
        /// <param name="canExecute">Func to be executed to evaluate if a command can or can´t be executed.</param>
        public DelegateCommand(Action execute, Func<bool> canExecute)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }

        /// <summary>
        /// ICommand call this method to evaluate if the command can be executed.
        /// When called, invoke the Func we have stored in canExecute if it is null return always true.
        /// </summary>
        /// <param name="parameter">Command parameter, ignored in this implementation.</param>
        /// <returns>True if the command can be execute, otherwise false.</returns>
        public bool CanExecute(object parameter)
        {
            if (this.canExecute != null)
                return this.canExecute();

            return true;
        }

        public event EventHandler CanExecuteChanged;

        /// <summary>
        /// ICommand call this method to execute the command action.
        /// When called, invoke the Action we have stored in execute if it isn´t null.
        /// </summary>
        /// <param name="parameter">Command parameter, ignored in this implementation.</param>
        public void Execute(object parameter)
        {
            if (this.execute != null)
                this.execute();
        }

        /// <summary>
        /// This method can be used to manually launch the command CanExecute evaluation.
        /// </summary>
        public void RaiseCanExecuteChanged()
        {
            var Handler = CanExecuteChanged;
            if (Handler != null)
                Handler(this, new EventArgs());
        }
    }

    /// <summary>
    /// ICommand generic implementation to reuse among all our ViewModels.
    /// </summary>
    /// <typeparam name="T">Type to use in the Execute and CanExecute parameters.</typeparam>
    public class DelegateCommand<T> : ICommand
    {
        private Action<T> execute;
        private Func<T, bool> canExecute;

        /// <summary>
        /// Constructor without can execute.
        /// </summary>
        /// <param name="execute">Action to be launched when the command is executed.</param>
        public DelegateCommand(Action<T> execute)
            : this(execute, null) { }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="execute">Action to be launched when the command is executed.</param>
        /// <param name="canExecute">Func to be executed to evaluate if a command can or can´t be executed.</param>
        public DelegateCommand(Action<T> execute, Func<T, bool> canExecute)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }

        /// <summary>
        /// ICommand call this method to evaluate if the command can be executed.
        /// When called, invoke the Func we have stored in canExecute if it is null return always true.
        /// </summary>
        /// <param name="parameter">Command parameter, we try to cast it to T.</param>
        /// <returns>True if the command can be execute, otherwise false.</returns>
        public bool CanExecute(object parameter)
        {
            if (this.canExecute != null)
                return this.canExecute((T)parameter);

            return true;
        }

        public event EventHandler CanExecuteChanged;

        /// <summary>
        /// ICommand call this method to execute the command action.
        /// When called, invoke the Action we have stored in execute if it isn´t null.
        /// </summary>
        /// <param name="parameter">Command parameter, we try to cast it to T.</param>
        public void Execute(object parameter)
        {
            if (this.execute != null)
                this.execute((T)parameter);
        }

        /// <summary>
        /// This method can be used to manually launch the command CanExecute evaluation.
        /// </summary>
        public void RaiseCanExecuteChanged()
        {
            var Handler = CanExecuteChanged;
            if (Handler != null)
                Handler(this, new EventArgs());
        }
    }
}
