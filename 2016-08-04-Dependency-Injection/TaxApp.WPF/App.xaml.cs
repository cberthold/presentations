using Autofac;
using Microsoft.Shell;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using TaxApp.WPF.Views.MainWindow;
using NLog;

namespace TaxApp.WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application, ISingleInstanceApp
    {
        private const string Unique = "TaxApp";
        private IContainer container;

        [STAThread]
        public static void Main()
        {
            if (SingleInstance<App>.InitializeAsFirstInstance(Unique))
            {
                var application = new App();

                var builder = new ContainerBuilder();

                AppBootstrap.Configure(builder);
                application.container = builder.Build();

                var containerBuilder = new ContainerBuilder();
                containerBuilder.RegisterInstance(application.container)
                    .As<IContainer>();

                containerBuilder.Update(application.container);

                // login would go here

                application.InitializeComponent();
                application.Run();

                // Allow single instance code to perform cleanup operations
                SingleInstance<App>.Cleanup();
            }
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            this.DispatcherUnhandledException += (obj, args) =>
            {
                ILogger logger = LogManager.GetCurrentClassLogger();
                logger.Fatal(args.Exception, "Unhandled Exception");
            };
            var window = container.Resolve<MainWindowView>();
            Current.MainWindow = window;
            Current.MainWindow.Show();
        }

        public bool SignalExternalCommandLineArgs(IList<string> args)
        {
            return true;
        }
    }
}
