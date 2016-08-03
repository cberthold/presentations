using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace TaxApp.Logic
{
    using Data.TaxesDb;
    using EntityFramework.Repository;
    public class ComplexTaxCalculator2 : BasicTaxCalculator
    {
        public IRepository<TaxesContext, StateTax> StateTaxes { get; set; }

        public decimal DefaultTax { get; set; } = 6.50m;

        protected override decimal GetTaxPercentageForOrder(SalesOrder order)
        {
            var state = order.Address.State;

            var query = StateTaxes
                            .AsQueryable()
                            .AsNoTracking();

            var stateTax = (from s in query
                            where s.State == state
                            select s).FirstOrDefault();

            if (stateTax != null)
            {
                return stateTax.TaxPercent;
            }

            return DefaultTax;
        }
    }
}
