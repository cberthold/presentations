using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace TaxApp.Logic
{
    using TaxApp.Data.TaxesDb;

    public static class SalesOrderOutput
    {
        public static void OutputSalesOrder(SalesOrder salesOrder)
        {
            const string LINE_FORMAT = "line item: {0} qty: {1} price: {2}";
            foreach (var item in salesOrder.SalesOrderLineItems)
            {
                Console.WriteLine(string.Format(
                LINE_FORMAT,
                item.Product,
                item.Quantity,
                item.Price));
            }


            const string ORDER_FORMAT = "sales order sub total: {0} tax: {1} total: {2}";
            Console.WriteLine(string.Format(
                ORDER_FORMAT,
                salesOrder.SubTotal,
                salesOrder.Tax,
                salesOrder.Total));
            Console.ReadLine();
        }
    }
}
