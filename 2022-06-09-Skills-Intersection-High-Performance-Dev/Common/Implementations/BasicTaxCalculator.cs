using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Interfaces
{
    public class BasicTaxCalculator : ICalculateTax
    {
        public void CalculateTax(InvoiceContext context)
        {
            var invoice = context.Invoice;
            invoice.Tax = Math.Round(invoice.SubTotal * 0.06m, 2);
        }
    }
}
