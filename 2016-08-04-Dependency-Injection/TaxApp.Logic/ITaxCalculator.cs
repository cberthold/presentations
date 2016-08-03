using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaxApp.Logic
{
    using TaxApp.Data.TaxesDb;

    public interface ITaxCalculator
    {
        decimal CalculateTax(SalesOrder order);
    }
}
