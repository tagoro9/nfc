namespace IBCSApp.ViewModels.Base
{
    using Autofac;
    using IBCSApp.Services.Dispatcher;
    using IBCSApp.Services.Navigation;
    using IBCSApp.Services.API;
    using IBCSApp.Services.Settings;
    using IBCSApp.Services.NFC;
    using IBCSApp.Services.Bluetooth;
    using IBCSApp.Services.BF;
    using IBCSApp.Services.UX;

    /// <summary>
    /// This class allows us to resolve our ViewModels in one unique point.
    /// </summary>
    public class VMLocator
    {
        /// <summary>
        /// Autofac container.
        /// </summary>
        IContainer container;

        /// <summary>
        /// Constructor.
        /// </summary>
        public VMLocator()
        {
            BuildContainer();
        }

        /// <summary>
        /// This method build the Autofac container with the registered types and instances.
        /// </summary>
        private void BuildContainer()
        {
            ContainerBuilder builder = new ContainerBuilder();

            builder.RegisterType<NavigationService>().As<INavigationService>().SingleInstance();
            builder.RegisterType<DispatcherService>().As<IDispatcherService>().SingleInstance();
            builder.RegisterType<ApiService>().As<IApiService>().SingleInstance();
            builder.RegisterType<SettingsService>().As<ISettingsService>().SingleInstance();
            builder.RegisterType<NFCService>().As<INFCService>().SingleInstance();
            builder.RegisterType<BluetoothService>().As<IBluetoothService>().SingleInstance();
            builder.RegisterType<PairingService>().As<IPairingService>().SingleInstance();
            builder.RegisterType<BfService>().As<IBfService>().SingleInstance();
            builder.RegisterType<UxService>().As<IUxService>().SingleInstance();
            builder.RegisterType<VMMainPage>();
            builder.RegisterType<VMSecondPage>();
            builder.RegisterType<VMLoginPage>();
            builder.RegisterType<VMSecureEmailPage>();
            builder.RegisterType<VMShareSecureMessage>();
            builder.RegisterType<VMCreateAccountPage>();
            container = builder.Build();
        }

        /// <summary>
        /// ShareSecureMessagePage ViewModel instance.
        /// </summary>
        public VMShareSecureMessage ShareSecureMessageViewModel
        {
            get { return this.container.Resolve<VMShareSecureMessage>(); }
        }

        /// <summary>
        /// LoginPage ViewModel instance.
        /// </summary>
        public VMLoginPage LoginViewModel
        {
            get { return this.container.Resolve<VMLoginPage>(); }
        }

        /// <summary>
        /// MainPage ViewModel instance.
        /// </summary>
        public VMMainPage MainViewModel
        {
            get { return this.container.Resolve<VMMainPage>(); }
        }

        /// <summary>
        /// SecondPage ViewModel instance.
        /// </summary>
        public VMSecondPage SecondViewModel
        {
            get { return this.container.Resolve<VMSecondPage>(); }
        }

        /// <summary>
        /// SecureEmailPage ViewModel instance.
        /// </summary>
        public VMSecureEmailPage SecureEmailViewModel
        {
            get { return this.container.Resolve<VMSecureEmailPage>(); }
        }

        /// <summary>
        /// CreateAccountPage ViewModel instance.
        /// </summary>
        public VMCreateAccountPage CreateAccountViewModel
        {
            get { return this.container.Resolve<VMCreateAccountPage>(); }
        }
    }
}
