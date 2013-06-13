namespace TicTacToe.ViewModels.Base
{
    using System.ComponentModel;
    using System.Runtime.CompilerServices;

    /// <summary>
    /// ViewModel base to implement change notification in all of our viewmodels.
    /// Also implement IsBusy property to show in the UX when we are doing a time intensive operation.
    /// </summary>
    public class VMBase : INotifyPropertyChanged
    {
        private bool isBusy = false;

        /// <summary>
        /// Constructor.
        /// </summary>
        public VMBase()
        {
        }

        /// <summary>
        /// This property can be binded in the view to show a visual clue when we are doing an operation 
        /// that require large time to complete.
        /// </summary>
        public bool IsBusy
        {
            get { return this.isBusy; }
            set
            {
                this.isBusy = value;
                RaisePropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Public method, called from the ViewModel to raise the PropertyChanged event.
        /// </summary>
        /// <param name="propertyName">Optional, name of the property who update it value.</param>
        public void RaisePropertyChanged([CallerMemberName]string propertyName = "")
        {
            var Handler = PropertyChanged;
            if (Handler != null)
                Handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
