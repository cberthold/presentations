using Common;
using Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PatternsComplete
{
    public class ProductATaxFreeTaxCalculator : ICalculateTax
    {
        public void CalculateTax(InvoiceContext context)
        {
            var productATotal = context.Cart.Items.Where(a => a.Description == "Product A").Sum(a => a.ItemTotal);
            var tax = Math.Round(productATotal * 0.06m, 2);

            if (tax > 0)
            {
                context.Invoice.Tax = -1 * tax;
            }
        }
    }
}
