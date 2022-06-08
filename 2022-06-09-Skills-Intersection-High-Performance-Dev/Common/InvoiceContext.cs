using ConsoleApp1;
using System;
using System.Collections.Generic;
using System.Text;

namespace Common
{
    public class InvoiceContext
    {
        public Invoice Invoice { get; set; }
        public ShoppingCart Cart { get; set; }
    }
}
