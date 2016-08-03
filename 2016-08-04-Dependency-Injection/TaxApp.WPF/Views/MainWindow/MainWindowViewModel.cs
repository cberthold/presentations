using Autofac;
using TaxApp.WPF.Common;
using TaxApp.WPF.Common.Command;
using TaxApp.WPF.Navigation;
using NLog;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using TaxApp.WPF.Common.Config.Interfaces;
using TaxApp.Logic;
using TaxApp.Data.TaxesDb;

namespace TaxApp.WPF.Views.MainWindow
{
    public class MainWindowViewModel : ViewModelBase
    {
        #region injected properties

        public ILogger Logger { private get; set; }
        public IAsyncCommandFactory<MainWindowViewModel> CommandFactory { private get; set; }
        public INavigationService NavigationService { private get; set; }
        private IConfigurationManagement _configuration;
        public IConfigurationManagement Configuration
        {
            private get { return _configuration; }
            set
            {
                _configuration = value;
                ApplicationTitle = string.Format("TaxApp Environment: {0}", _configuration.CurrentEnvironment);
            }
        }
        public IContainer Container { private get; set; }

        #endregion

        #region injected commands

        #endregion

        #region locally created commands

        Lazy<AsyncCommand<object>> calculateInit;
        public AsyncCommand<object> Calculate { get { return calculateInit.Value; } }

        #endregion

        #region local properties

        private string _applicationTitle;
        public string ApplicationTitle
        {
            get { return _applicationTitle; }
            set
            {
                _applicationTitle = value;
                OnPropertyChanged();
            }
        }


        private Guid orderId;

        public Guid OrderId
        {
            get { return orderId; }
            set
            {
                orderId = value;
                OnPropertyChanged();
            }
        }



        private string address1;

        public string Address1
        {
            get { return address1; }
            set
            {
                address1 = value;
                OnPropertyChanged();
            }
        }

        private string address2;

        public string Address2
        {
            get { return address2; }
            set
            {
                address2 = value;
                OnPropertyChanged();
            }
        }

        private string city;

        public string City
        {
            get { return city; }
            set
            {
                city = value;
                OnPropertyChanged();
            }
        }

        private string state;

        public string State
        {
            get { return state; }
            set
            {
                state = value;
                OnPropertyChanged();
            }
        }

        private string zip;

        public string Zip
        {
            get { return zip; }
            set
            {
                zip = value;
                OnPropertyChanged();
            }
        }

        public SalesOrder ToOrder()
        {
            var order = new SalesOrder();
            order.Id = OrderId;

            var address = new Address();
            address.Address1 = Address1;
            address.Address2 = Address2;
            address.City = City;
            address.State = State;
            address.Zip = Zip;

            order.Address = address;

            return order;
        }

        #endregion


        #region constructor

        public MainWindowViewModel()
        {


            ResetOrder();

            calculateInit = new Lazy<AsyncCommand<object>>(() =>
            {
                var vm = this;
                return vm.CommandFactory.Create(RunCalculation);
            });

        }

        private void ResetOrder()
        {
            OrderId = Guid.NewGuid();
            Address1 = "Address1";
            Address2 = "line #2";
            City = "Somewhere";
            State = "FL";
            Zip = "33928";

        }

        #endregion

        #region private methods


        private Task RunCalculation(CancellationToken token)
        {
            return Task.Run(() =>
            {
                // start scope in a new lifetime so IDisposable will automatically be handled on
                // resources injected
                using (var scope = Container.BeginLifetimeScope())
                {

                    var service = scope.Resolve<OrderController>();

                    service.CreateASalesOrder(ToOrder());

                    ResetOrder();

                }
            }, token);
        }


        #endregion

        #region public methods

        public void Initialize()
        {
            //Task.Run(async () =>
            //{
            //    await Initsomestuff();
            //});
        }

        #endregion

    }
}
