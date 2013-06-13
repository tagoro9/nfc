namespace TicTacToe.ViewModels.Base
{
    using Autofac;
    using TicTacToe.Services.Dispatcher;
    using TicTacToe.Services.Navigation;

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
            builder.RegisterType<VMGamePage>();
            container = builder.Build();
        }

        /// <summary>
        /// MainPage ViewModel instance.
        /// </summary>
        public VMGamePage MainViewModel
        {
            get { return this.container.Resolve<VMGamePage>(); }
        }
    }
}
