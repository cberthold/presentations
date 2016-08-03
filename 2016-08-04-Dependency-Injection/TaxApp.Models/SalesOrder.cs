using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaxApp.Models
{
    public class SalesOrder
    {
        private List<SalesOrderLineItem> lineItems;

        public IReadOnlyList<SalesOrderLineItem> LineItems => lineItems.AsReadOnly();

        public SalesOrder()
        {
            Address = new Address();
            lineItems = new List<SalesOrderLineItem>();

        }

        public decimal SubTotal { get; private set; }
        public decimal Tax { get; private set; }
        public decimal Total { get; private set; }

        public Address Address { get; private set; }

        public void AddLineItem(SalesOrderLineItem item)
        {
            lineItems.Add(item);
        }

        public void SetSubTotal(decimal subtotal)
        {
            SubTotal = subtotal;
            SetTotal();
        }

        public void SetTax(decimal tax)
        {
            Tax = tax;
            SetTotal();
        }

        private void SetTotal()
        {
            Total = SubTotal + Tax;
        }
    }
}
