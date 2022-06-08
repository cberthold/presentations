using Common;
using Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PatternsComplete
{
    public class CompositeTaxCalculator : ICalculateTax
    {
        public IEnumerable<ICalculateTax> Calculators { get; }

        public CompositeTaxCalculator(IEnumerable<ICalculateTax> calculators)
        {
            Calculators = calculators ?? Enumerable.Empty<ICalculateTax>();
        }

        public void CalculateTax(InvoiceContext context)
        {
            decimal totalTax = 0m;
            context.Invoice.Tax = 0m;

            foreach (var calc in Calculators)
            {
                calc.CalculateTax(context);
                totalTax += context.Invoice.Tax;
                context.Invoice.Tax = 0m;
            }

            context.Invoice.Tax = totalTax;
        }
    }
}
