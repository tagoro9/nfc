using IBCSApp.Resources;
using IBCSApp.ViewModels.Base;
using Microsoft.Phone.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace IBCSApp.ViewModels
{
    public class VMAboutPage : VMBase
    {
        //Commands
        private DelegateCommand twitterFollowCommand;
        private DelegateCommand goToWebCommand;

        public VMAboutPage()
        {
            this.twitterFollowCommand = new DelegateCommand(TwitterFollowExecute);
            this.goToWebCommand = new DelegateCommand(GoToWebExecute);
        }

        public ICommand TwitterFollowCommand
        {
            get { return this.twitterFollowCommand; }
        }
        public ICommand GoToWebCommand
        {
            get { return this.goToWebCommand; }
        }

        private void GoToWebExecute()
        {
            WebBrowserTask task = new WebBrowserTask();
            task.Uri = new Uri(AppResources.WebUrl);
            task.Show();
        }

        private void TwitterFollowExecute()
        {
            WebBrowserTask task = new WebBrowserTask();
            task.Uri = new Uri(AppResources.TwitterUrl);
            task.Show();
        }

    }
}
