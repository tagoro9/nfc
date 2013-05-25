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
    public class VMShareSecureMessage : VMBase
    {
        private string messageText;
        private string identity;

        private DelegateCommand shareSecureMessageCommand;

        public VMShareSecureMessage()
        {
            this.shareSecureMessageCommand = new DelegateCommand(ShareSecureMessageExecute);
        }

        private void ShareSecureMessageExecute()
        {
            ShareLinkTask shareLink = new ShareLinkTask();
            shareLink.LinkUri = new Uri("ibcs:decrypt?message=esto_deberia_ser_el_mensaje_cifrado?id=i_identidad");
            shareLink.Title = "IBCS cencrypted message";
            shareLink.Message = "Message intended to mi_identidad";
            shareLink.Show();
        }

        public ICommand ShareSecureMessageCommand
        {
            get { return shareSecureMessageCommand; }
        }

        public string MessageText
        {
            get { return messageText; }
            set
            {
                messageText = value;
                RaisePropertyChanged();
            }
        }

        public string Identity
        {
            get { return identity; }
            set
            {
                identity = value;
                RaisePropertyChanged();
            }
        }
    }
}
