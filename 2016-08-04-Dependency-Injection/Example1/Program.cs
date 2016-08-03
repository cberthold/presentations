using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace Example1
{
    using TaxApp.Logic;
    using TaxApp.Data.TaxesDb;
    using Autofac;
    public partial class Program
    {

        static void Main(string[] args)
        {
            var salesOrder = new SalesOrder();
            salesOrder.Address = new Address();
            var address = salesOrder.Address;
            address.Address1 = "1600 Pennsylvania Ave NW";
            address.City = "Washington";
            address.State = "DC";
            address.Zip = "20500";

            var salesProcessor = new SalesOrderProcessor();
            var taxCalculator = new BasicTaxCalculator();
            
            salesProcessor.TaxCalculator = taxCalculator;

            AddLines(salesOrder, salesProcessor);

            SalesOrderOutput.OutputSalesOrder(salesOrder);
        }


        private static void AddLines(SalesOrder salesOrder, SalesOrderProcessor salesProcessor)
        {
            var lineItem1 = new SalesOrderLineItem();
            lineItem1.Product = "Pack of 10 Pencils";
            lineItem1.Price = 10000.00m;
            lineItem1.Quantity = 10;
            salesProcessor.AddLineItemToSalesOrder(salesOrder, lineItem1);

            var lineItem2 = new SalesOrderLineItem();
            lineItem2.Product = "Toilet Seat Covers";
            lineItem2.Price = 900.00m;
            lineItem2.Quantity = 18;
            salesProcessor.AddLineItemToSalesOrder(salesOrder, lineItem2);
        }

        #region with dependency injection

        static void Completed(string[] args)
        {

            var builder = new ContainerBuilder();
            builder.RegisterType<SalesOrderProcessor>()
                .AsSelf();
            builder.RegisterType<BasicTaxCalculator>()
                .As<ITaxCalculator>();
            //.AsImplementedInterfaces();

            var salesOrder = new SalesOrder();
            salesOrder.Address = new Address();
            var address = salesOrder.Address;
            address.Address1 = "1600 Pennsylvania Ave NW";
            address.City = "Washington";
            address.State = "DC";
            address.Zip = "20500";

            using (var container = builder.Build())
            {
                
                var salesProcessor = container.Resolve<SalesOrderProcessor>();
                AddLines(salesOrder, salesProcessor);

            }
            SalesOrderOutput.OutputSalesOrder(salesOrder);
        }


        #endregion



    }


}
