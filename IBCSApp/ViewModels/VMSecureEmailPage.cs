using IBCSApp.Services.Dispatcher;
using IBCSApp.Services.Navigation;
using IBCSApp.ViewModels.Base;
using Microsoft.Phone.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using IBCS.BF.Key;
using IBCS.BF;
using IBCSApp.Services.Settings;
using Newtonsoft.Json;
using IBCSApp.Services.BF;

namespace IBCSApp.ViewModels
{
    public class VMSecureEmailPage : VMBase
    {

        ///Services
        private INavigationService navService;
        private IDispatcherService dispatcherService;
        private ISettingsService settingsService;
        private IBfService bfService;

        //Commands
        private DelegateCommand addressChooserCommand;
        private DelegateCommand sendSecureEmailCommand;

        private string destinataryEmail;
        private string destinataryName;
        private string emailSubject;
        private string emailBody;
        private string progressMessage;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="navService">Navigation service instance resolved by DI.</param>
        /// <param name="dispatcherService">Dispatcher service instance resolved by DI.</param>
        public VMSecureEmailPage(INavigationService navService, IDispatcherService dispatcherService, ISettingsService settingsService, IBfService bfService)
        {
            this.navService = navService;
            this.dispatcherService = dispatcherService;
            this.settingsService = settingsService;
            this.bfService = bfService;

            this.addressChooserCommand = new DelegateCommand(AddressChooserExecute);
            this.sendSecureEmailCommand = new DelegateCommand(SendSecureEmailExecute);
        }

        public ICommand SendSecureEmailCommand
        {
            get { return sendSecureEmailCommand; }
        }

        private void SendSecureEmailExecute()
        {
            //IsBusy = true;
            //ProgressMessage = "Encrypting message...";
            //SerializedPrivateKey sKey = (SerializedPrivateKey) settingsService.Get("private");
            //bfService.CipherTextCompleted += bfService_CipherTextCompleted;
            //bfService.CipherText(EmailBody, DestinataryEmail, sKey);
        }

        private void bfService_CipherTextCompleted(string ct)
        {
            EmailComposeTask email = new EmailComposeTask();
            email.To = DestinataryEmail;
            email.Subject = EmailSubject;
            email.Body = ct +
            "\n\n This message is ciphered using an Identity Based Cryptosystem. In order the decrypt it download the Windows Phone IBCS app!";
            IsBusy = false;
            ProgressMessage = "";
            email.Show();
        }

        public string ProgressMessage
        {
            get { return progressMessage; }
            set
            {
                progressMessage = value;
                RaisePropertyChanged();
            }
        }

        public string EmailSubject
        {
            get { return emailSubject; }
            set
            {
                emailSubject = value;
                RaisePropertyChanged();
            }
        }

        public string EmailBody
        {
            get { return emailBody; }
            set
            {
                emailBody = value;
                RaisePropertyChanged();
            }
        }

        public string DestinataryEmail
        {
            get { return destinataryEmail; }
            set
            {
                destinataryEmail = value;
                RaisePropertyChanged();
            }
        }

        public string DestinataryName
        {
            get { return destinataryName; }
            set
            {
                destinataryName = value;
                RaisePropertyChanged();
            }
        }

        private void AddressChooserExecute()
        {
            EmailAddressChooserTask emailChooser = new EmailAddressChooserTask();
            emailChooser.Completed += emailChooser_Completed;
            emailChooser.Show();
        }

        private void emailChooser_Completed(object sender, EmailResult e)
        {
            DestinataryEmail = e.Email;
            DestinataryName = e.DisplayName;
        }

        public ICommand AddressChooserCommand
        {
            get { return this.addressChooserCommand; }
        }
    }
}
