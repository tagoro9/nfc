namespace TicTacToe.ViewModels.Base
{
    /// <summary>
    /// SecondPage ViewModel.
    /// </summary>
    public class VMSecondPage : VMBase
    {
        private string message = string.Empty;

        /// <summary>
        /// Default constructor. in this we dont need any service.
        /// </summary>
        public VMSecondPage()
        {
            message = "This message is created in the ViewModel.";
        }

        /// <summary>
        /// Message showed in the Page.
        /// </summary>
        public string Message
        {
            get { return this.message; }
        }
    }
}
