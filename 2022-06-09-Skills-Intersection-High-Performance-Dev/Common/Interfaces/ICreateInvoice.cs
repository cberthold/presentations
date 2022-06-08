using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Interfaces
{
    public interface ICreateInvoice
    {
        void CreateInvoice(InvoiceContext context);
    }
}
