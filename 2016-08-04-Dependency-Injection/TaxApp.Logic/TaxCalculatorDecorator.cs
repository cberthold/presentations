using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaxApp.Data.TaxesDb;

namespace TaxApp.Logic
{
    public class TaxCalculatorDecorator : ITaxCalculator
    {
        public ITaxCalculator Inner { get; private set; }

        public TaxCalculatorDecorator(ITaxCalculator inner)
        {
            Inner = inner;
        }

        public decimal CalculateTax(SalesOrder order)
        {
            Console.WriteLine(string.Format("Calculating tax for sales order: {0}", order.Id));
            var calculatedTax = Inner.CalculateTax(order);
            Console.WriteLine(string.Format("Calculated Tax at: {0}", calculatedTax));

            return calculatedTax;
        }
    }
}
