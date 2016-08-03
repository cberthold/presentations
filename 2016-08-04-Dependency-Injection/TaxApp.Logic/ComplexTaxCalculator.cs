using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace TaxApp.Logic
{
    using Data.TaxesDb;
    
    public class ComplexTaxCalculator : BasicTaxCalculator
    {
        public ITaxesContext DbContext { get; set; }

        public decimal DefaultTax { get; set; } = 6.50m;

        protected override decimal GetTaxPercentageForOrder(SalesOrder order)
        {
            var state = order.Address.State;

            var stateTax = (from s in DbContext.StateTaxes
                            where s.State == state
                            select s).FirstOrDefault();

            if(stateTax != null)
            {
                return stateTax.TaxPercent;
            }

            return DefaultTax;
        }
    }
}
