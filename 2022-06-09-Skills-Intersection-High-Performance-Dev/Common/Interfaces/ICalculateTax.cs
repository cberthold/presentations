using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Interfaces
{
    public interface ICalculateTax
    {
        void CalculateTax(InvoiceContext context);
    }
}
