
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaxApp.Modules
{
    using Autofac;
    using Data.TaxesDb;
    using EntityFramework.Repository;
    using Logic;

    public class TaxAppModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<TaxesContext>()
                .AsSelf()
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();


            builder.RegisterType<SalesOrderProcessor>()
                .AsSelf()
                .InstancePerLifetimeScope();

            #region more complex example

            builder.RegisterAssemblyTypes(typeof(IDbContext).Assembly)
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            builder.RegisterAssemblyTypes(typeof(ComplexTaxCalculator).Assembly)
                .AsImplementedInterfaces()
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

            #endregion


            builder.RegisterType<ComplexTaxCalculator>()
                .PropertiesAutowired()
                .InstancePerLifetimeScope()
                .AsImplementedInterfaces();

            


        builder.RegisterType<TaxesContext>()
                .As<ITaxesContext>()
                .InstancePerLifetimeScope();

        }
    }
}
