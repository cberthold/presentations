using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestProject1
{
    using Autofac;
    using System.Collections.Generic;
    using TaxApp.Data.TaxesDb;
    using TaxApp.Logic;

    [TestClass]
    public class UnitTest1
    {

        private static IContainer BuildContainer(Action<ContainerBuilder> afterBuild = null)
        {
            var builder = new ContainerBuilder();
            builder.RegisterAssemblyModules(typeof(TaxApp.Modules.TaxAppModule).Assembly);

            var container = builder.Build();

            builder = new ContainerBuilder();
            afterBuild?.Invoke(builder);

            builder.Update(container);

            return container;
        }

        private const decimal DEFAULT_TAX = 6.5m;
        private const decimal DC_TAX = 0;
        private const decimal FL_TAX = 6m;

        private static IContainer MockContainer()
        {
            return BuildContainer((builder) =>
            {
                builder.Register<ITaxesContext>((ctx, parms) =>
                {
                    // talk about IDisposable here ^^^
                    var taxesContext = new FakeTaxesContext();

                    taxesContext.StateTaxes = new FakeDbSet<StateTax>();

                    var stateTaxes = taxesContext.StateTaxes;
                    stateTaxes.Add(new StateTax()
                    {
                        Id = Guid.NewGuid(),
                        State = "FL",
                        TaxPercent = FL_TAX

                    });
                    stateTaxes.Add(new StateTax()
                    {
                        Id = Guid.NewGuid(),
                        State = "DC",
                        TaxPercent = DC_TAX,
                    });

                    return taxesContext;
                });
                
            });
        }

        [TestMethod]
        public void TestDC()
        {
            using (var container = MockContainer())
            using (var scope = container.BeginLifetimeScope())
            {
                var calc = container.Resolve<ITaxCalculator>();
                var salesOrder = new SalesOrder();
                salesOrder.Address = new Address
                {
                    State = "DC",
                };
                salesOrder.SubTotal = 1;
                var tax = calc.CalculateTax(salesOrder);
                Assert.AreEqual(0m, tax);
            }
        }

        [TestMethod]
        public void TestFL()
        {
            using (var container = MockContainer())
            using (var scope = container.BeginLifetimeScope())
            {
                var calc = scope.Resolve<ITaxCalculator>();
                var salesOrder = new SalesOrder();
                salesOrder.Address = new Address
                {
                    State = "FL",
                };
                salesOrder.SubTotal = 1;
                var tax = calc.CalculateTax(salesOrder);
                Assert.AreEqual(0.06m, tax);
            }
        }
    }
}
