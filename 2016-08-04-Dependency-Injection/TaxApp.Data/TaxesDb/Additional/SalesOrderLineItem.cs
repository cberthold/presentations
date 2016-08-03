using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaxApp.Data.TaxesDb
{
    public partial class SalesOrderLineItem
    {
        partial void InitializePartial()
        {
            this.Id = Guid.NewGuid();
        }
    }
}
