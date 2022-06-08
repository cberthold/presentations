using Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common
{
    public class SubtotalCalculator : ICalculateSubtotal
    {
        public void CalculateSubtotal(InvoiceContext context)
        {
            var invoice = context.Invoice;
            invoice.SubTotal = invoice.Items.Sum(a => a.ItemTotal);
        }
    }
}
