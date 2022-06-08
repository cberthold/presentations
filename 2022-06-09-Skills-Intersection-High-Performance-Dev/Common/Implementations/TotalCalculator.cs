using Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Common
{
    public class TotalCalculator : ICalculateTotal
    {
        public void CalculateTotal(InvoiceContext context)
        {
            var invoice = context.Invoice;
            invoice.Total = invoice.SubTotal + invoice.Tax;
        }
    }
}
