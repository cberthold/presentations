using Common.Interfaces;
using ConsoleApp1;
using System;
using System.Collections.Generic;
using System.Text;

namespace Common
{
    public class InvoiceCreator : ICreateInvoice
    {
        public void CreateInvoice(InvoiceContext context)
        {
            var cart = context.Cart;
            var invoice = Invoice.ConvertFromShoppingCart(context.Cart);
            context.Invoice = invoice;
        }
    }
}
