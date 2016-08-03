using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Example3
{
    using Autofac;
    using EntityFramework.Repository;
    using TaxApp.Data.TaxesDb;
    using TaxApp.Logic;

    class Program
    {
        static void Main(string[] args)
        {


            var salesOrderId = Guid.NewGuid();

            var salesOrder = new SalesOrder();
            salesOrder.Id = salesOrderId;

            salesOrder.Address = new Address();
            var address = salesOrder.Address;
            address.Address1 = "1600 Pennsylvania Ave NW";
            address.Address2 = string.Empty;
            address.City = "Washington";
            address.State = "FL";
            address.Zip = "20500";


            #region complex example
            
            using (var context = new TaxesContext())
            {

                context.SalesOrders.Add(salesOrder);

                var salesProcessor = new SalesOrderProcessor();

                var taxCalculator = new ComplexTaxCalculator();

                taxCalculator.DbContext = context;

                salesProcessor.TaxCalculator = taxCalculator;

                AddLines(salesOrder, salesProcessor);

                context.SaveChanges();
            }


            #endregion

            #region even more complex

            /*

            // create the context
            using (var context = new TaxesContext())
            // create the context adapter for unit of work
            using (var factory = new ContextAdaptor<TaxesContext>(context))
            // create the unit of work
            using (IUnitOfWork<TaxesContext> uow = new UnitOfWork<TaxesContext>(factory))
            {
                // start the transaction
                using (var session = uow.StartSession())
                {
                    // create the repository for sales orders
                    var orderRepo = new Repository<TaxesContext, SalesOrder>(factory);
                    // insert sales order
                    orderRepo.Insert(salesOrder);

                    // create repository for state taxes
                    var taxRepo = new Repository<TaxesContext, StateTax>(factory);

                    // create the calculator
                    var taxCalc2 = new ComplexTaxCalculator2();
                    // apply the state taxes repository
                    taxCalc2.StateTaxes = taxRepo;

                    // create sales order processor
                    var salesProcessor = new SalesOrderProcessor();
                    // apply the tax calculator
                    salesProcessor.TaxCalculator = taxCalc2;

                    // add some sales order lines
                    AddLines(salesOrder, salesProcessor);

                    // save the changes in the context
                    uow.SaveChanges();

                    // complex the transaction
                    session.Commit();

                }


            }

            */
            #endregion


            using (var context2 = new TaxesContext())
            {

                salesOrder = (from s in context2.SalesOrders
                                        .Include(a => a.Address)
                                        .Include(a => a.SalesOrderLineItems)
                              where s.Id == salesOrderId
                              select s).FirstOrDefault();
            }





            SalesOrderOutput.OutputSalesOrder(salesOrder);
        }

        #region more complex completed

        public static void MoreComplexExampleCompleted()
        {
            var salesOrderId = Guid.NewGuid();

            var salesOrder = new SalesOrder();
            salesOrder.Id = salesOrderId;

            salesOrder.Address = new Address();
            var address = salesOrder.Address;
            address.Address1 = "1600 Pennsylvania Ave NW";
            address.Address2 = string.Empty;
            address.City = "Washington";
            address.State = "FL";
            address.Zip = "20500";

            using (var container = BuildContainer()) // this represents application scope
            using (var scope = container.BeginLifetimeScope()) // this represents a unit of work
            {
                var uow = scope.Resolve<IUnitOfWork<TaxesContext>>();
                using (var session = uow.StartSession())
                {
                    // get the sales processor
                    var salesProcessor = scope.Resolve<SalesOrderProcessor>();
                    // get the order repository
                    var orderRepo = scope.Resolve<IRepository<TaxesContext, SalesOrder>>();
                    orderRepo.Insert(salesOrder);

                    AddLines(salesOrder, salesProcessor);
                    uow.SaveChanges();

                    session.Commit();

                    #region even better
                    var controller = scope.Resolve<OrderController>();
                    controller.CreateASalesOrder(salesOrder);
                    #endregion

                }


            }

        }

        private static IContainer BuildContainer()
        {
            var builder = new ContainerBuilder();
            builder.RegisterAssemblyModules(typeof(TaxApp.Modules.TaxAppModule).Assembly);
            return builder.Build();
        }

        #endregion


        public static void AddLines(SalesOrder salesOrder, SalesOrderProcessor salesProcessor)
        {
            SalesOrderLineItem lineItem1, lineItem2;

            lineItem1 = new SalesOrderLineItem();
            lineItem1.Product = "Pack of 10 Pencils";
            lineItem1.Price = 10000.00m;
            lineItem1.Quantity = 10;
            salesProcessor.AddLineItemToSalesOrder(salesOrder, lineItem1);

            lineItem2 = new SalesOrderLineItem();
            lineItem2.Product = "Toilet Seat Covers";
            lineItem2.Price = 900.00m;
            lineItem2.Quantity = 18;
            salesProcessor.AddLineItemToSalesOrder(salesOrder, lineItem2);
        }
    }
}
