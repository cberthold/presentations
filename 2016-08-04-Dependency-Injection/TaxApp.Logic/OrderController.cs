using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaxApp.Logic
{
    using Data.TaxesDb;
    using EntityFramework.Repository;

    public class OrderController
    {
        public IUnitOfWork<TaxesContext> UnitOfWork { private get; set; }
        public SalesOrderProcessor Processor { private get; set; }
        public IRepository<TaxesContext, SalesOrder> Repository { private get; set; }
        

        public void CreateASalesOrder(SalesOrder salesOrder)
        {
            
            // begin transaction
            using (var session = UnitOfWork.StartSession())
            {
                // insert sales order into context
                Repository.Insert(salesOrder);
                // add the lines
                AddLines(salesOrder, Processor);
                // tell context
                UnitOfWork.SaveChanges();
                // commit transaction
                session.Commit();

            }
        }

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
