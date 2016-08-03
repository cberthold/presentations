using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace TaxApp.Logic
{
    using TaxApp.Data.TaxesDb;

    public class BasicTaxCalculator : ITaxCalculator
    {

        protected virtual decimal GetTaxPercentageForOrder(SalesOrder order)
        {
            return 7.00m;
        }

        public virtual decimal CalculateTax(SalesOrder order)
        {
            var taxPercentage = GetTaxPercentageForOrder(order);
            var taxAsDecimal = taxPercentage / 100.0m;
            return Math.Round(order.SubTotal * taxAsDecimal, 2);
        }

    }

}
