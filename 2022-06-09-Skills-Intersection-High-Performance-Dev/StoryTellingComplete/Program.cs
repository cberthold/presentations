using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            var givenOrder = ShoppingCart.GivenOrder();
            Invoice invoice = default;
            
            var processor = new InvoiceProcessor();
            invoice = processor.ProcessOrder(givenOrder);
            Console.WriteLine($"SubTotal: {invoice.SubTotal}");
            Console.WriteLine($"Tax: {invoice.Tax}");
            Console.WriteLine($"Total: {invoice.Total}");
        }
    }

    public class InvoiceProcessor
    {
        public Invoice ProcessOrder(ShoppingCart cart)
        {
            // we create a context to store the interesting parts of our story
            var context = new InvoiceContext();

            // the first major component is the cart
            context.Cart = cart;

            // our story has 4 major steps to it
            // 1. Create Invoice
            CreateInvoice(context);
            // 2. Calculate Subtotal
            CalculateSubtotal(context);
            // 3. Calculate Tax
            CalculateTax(context);
            // 4. Calculate Total
            CalculateTotal(context);

            return context.Invoice;
        }

        private void CreateInvoice(InvoiceContext context)
        {
            var cart = context.Cart;
            var invoice = Invoice.ConvertFromShoppingCart(context.Cart);
            context.Invoice = invoice;
        }

        private void CalculateSubtotal(InvoiceContext context)
        {
            var invoice = context.Invoice;
            invoice.SubTotal = invoice.Items.Sum(a => a.ItemTotal);
        }

        private void CalculateTax(InvoiceContext context)
        {
            var invoice = context.Invoice;
            invoice.Tax = Math.Round(invoice.SubTotal * 0.06m, 2);
        }

        private void CalculateTotal(InvoiceContext context)
        {
            var invoice = context.Invoice;
            invoice.Total = invoice.SubTotal + invoice.Tax;
        }
    }

    public class InvoiceContext
    {
        public Invoice Invoice { get; set; }
        public ShoppingCart Cart { get; internal set; }
    }

}
