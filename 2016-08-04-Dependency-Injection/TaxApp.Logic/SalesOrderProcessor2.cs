using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaxApp.Logic
{
    using TaxApp.Data.TaxesDb;

    public class SalesOrderProcessor2 : SalesOrderProcessor
    {
        public SalesOrderProcessor2(ITaxCalculator taxCalc)
        {
            TaxCalculator = taxCalc;
        }
    }

    public class SalesOrderProcessor
    {

        private ITaxCalculator calculator = null;
        public ITaxCalculator TaxCalculator
        {
            get {
                if(calculator == null)
                {
                    throw new Exception("You expected to show them about the constructor vs autowiring of properties");
                }

                return calculator; }
            set { calculator = value; }
        }

        public void AddLineItemToSalesOrder(SalesOrder salesOrder, SalesOrderLineItem lineItem)
        {
            lineItem.LineTotal = Math.Round(lineItem.Quantity * lineItem.Price);
            salesOrder.SalesOrderLineItems.Add(lineItem);
            RecalculateSalesOrder(salesOrder);
        }

        private void RecalculateSalesOrder(SalesOrder order)
        {
            decimal currentSubTotal = 0m;

            foreach (var lineItem in order.SalesOrderLineItems)
            {
                currentSubTotal += lineItem.LineTotal;
            }

            order.SubTotal = currentSubTotal;

            decimal currentTax = TaxCalculator.CalculateTax(order);

            order.Tax = currentTax;

            decimal currentTotal = currentSubTotal + currentTax;

            order.Total = currentTotal;

        }
    }

}
