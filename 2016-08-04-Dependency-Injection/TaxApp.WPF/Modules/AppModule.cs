using Autofac;
using Autofac.Core;
using EntityFramework.Repository;

using NLog;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Windows.Input;
using TaxApp.Common.Serialization;
using TaxApp.Common.Queryables;
using TaxApp.WPF.Common;
using TaxApp.WPF.Common.Command;
using TaxApp.WPF.Navigation;
using TaxApp.WPF.Views.MainWindow;
using TaxApp.Data.TaxesDb;
using TaxApp.Logic;
using TaxApp.WPF.Common.Config;
using TaxApp.WPF.Common.Config.Interfaces;

namespace TaxApp.WPF
{
    public class AppModule : Module
    {

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(typeof(IDbContext).Assembly)
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();
            

            builder.RegisterType<SalesOrderProcessor>()
                .AsSelf()
                .PropertiesAutowired()
                .InstancePerLifetimeScope();

            builder.RegisterAssemblyTypes(typeof(IDbContext).Assembly)
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            builder.RegisterAssemblyTypes(typeof(ComplexTaxCalculator).Assembly)
                .AsImplementedInterfaces()
                .PropertiesAutowired()
                .InstancePerLifetimeScope();

            builder.RegisterAssemblyTypes(typeof(TaxesContext).Assembly)
                .AsImplementedInterfaces()
                .AsSelf()
                .InstancePerLifetimeScope();

            builder.RegisterGeneric(typeof(ContextAdaptor<>))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            builder.RegisterGeneric(typeof(UnitOfWork<>))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            builder.RegisterGeneric(typeof(Repository<,>))
                .As(typeof(IRepository<,>))
                .InstancePerLifetimeScope();

            builder.RegisterType<OrderController>()
                .AsSelf()
                .PropertiesAutowired()
                .InstancePerLifetimeScope();



            builder.RegisterType<ComplexTaxCalculator>()
                .PropertiesAutowired()
                .InstancePerLifetimeScope()
                .AsImplementedInterfaces();




            builder.Register<TaxesContext>(ctx =>
            {
                var config = ctx.Resolve<IConfigurationManagement>();
                var connectionString = config.GetConfiguration().ConnectionStrings["Taxes"].ConnectionString;
                var context = new TaxesContext(connectionString);

                return context;
            })
                .AsSelf()
                    .As<ITaxesContext>()
                    .InstancePerLifetimeScope();

            builder.RegisterGeneric(typeof(ContextAdaptor<>))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();



            builder.RegisterGeneric(typeof(Repository<,>))
                .As(typeof(IRepository<,>))
                .InstancePerLifetimeScope();

            builder.RegisterGeneric(typeof(RepositoryFactory<>))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            builder.RegisterGeneric(typeof(QueryableFactory<>))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();



            var iCommandType = typeof(ICommand);
            builder.RegisterTypes((from t in typeof(AppModule).Assembly.GetTypes()
                                   where iCommandType.IsAssignableFrom(t)
                                   select t).ToArray())
                                   .AsSelf();

            builder.RegisterGeneric(typeof(AsyncCommandFactory<>))
                .AsImplementedInterfaces();

            builder.RegisterType<MainWindowView>()
                .AsSelf()
                .SingleInstance();

            var viewModelBaseType = typeof(ViewModelBase);
            builder.RegisterTypes((from t in typeof(AppModule).Assembly.GetTypes()
                                   where viewModelBaseType.IsAssignableFrom(t)
                                   select t).ToArray())
                .PropertiesAutowired()
                .AsSelf()
                .SingleInstance();

            builder.RegisterType<NavigationService>()
                .AsImplementedInterfaces()
                .SingleInstance();


            builder.RegisterType<JsonObjectSerializer>()
                .AsImplementedInterfaces()
                .SingleInstance();


            builder.RegisterType<ConfigurationManagement>()
               .AsSelf()
               .AsImplementedInterfaces()
               .PropertiesAutowired()
               .SingleInstance();

        }

        /*
        public class LoggingInterceptor : IInterceptor
        {
            private readonly Logger _logger;
            public LoggingInterceptor(Logger logger)
            {
                _logger = logger;
            }

            public void Intercept(IInvocation invocation)
            {
                
                if(_logger.IsDebugEnabled)
                {
                    string methodMessage = "Executing: " + string.Format("{0}->{1}", invocation.TargetType.Name, invocation.Method.Name);

                    _logger.Debug(methodMessage);

                    if (_logger.IsTraceEnabled)
                    {
                        var parms = invocation.Method.GetParameters();
                        int iParm = 0;
                        foreach (var arg in invocation.Arguments)
                        {
                            var parm = parms[iParm];
                            if (parm.ParameterType.IsPrimitive)
                            {
                                _logger.Trace("  {0}:{1}", parm.Name, arg);
                            }

                            iParm++;
                        }
                    }
                }

                invocation.Proceed();
                
            }
        }
        */

    }
}
