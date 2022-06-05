using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp1
{
    /// <summary>
    /// This invoice class represents the minimal object to represent our Invoice Case Study
    /// </summary>
    public class Invoice
    {
        public Address ShippingAddress { get; set; }
        public Address BillingAddress { get; set; }

        public List<InvoiceItem> Items { get; set; } = new List<InvoiceItem>();

        public decimal SubTotal { get; set; }
        public decimal Tax { get; set; }
        public decimal Total { get; set; }


        public static Invoice ConvertFromShoppingCart(ShoppingCart cart)
        {
            var invoice = new Invoice();
            invoice.BillingAddress = cart.BillingAddress;
            invoice.ShippingAddress = cart.ShippingAddress;

            foreach (var cartItem in cart.Items)
            {
                invoice.Items.Add(new InvoiceItem(cartItem));
            }

            return invoice;
        }
    }

    public class InvoiceItem
    {
        public string Description { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal ItemTotal { get; set; }

        public InvoiceItem(ShoppingItem item)
        {
            Description = item.Description;
            Quantity = item.Quantity;
            Price = item.Price;
            ItemTotal = item.ItemTotal;
        }
    }
}
