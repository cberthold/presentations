using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp1
{

    public class ShoppingCart
    {
        public Address ShippingAddress { get; set; }
        public Address BillingAddress { get; set; }

        public List<ShoppingItem> Items { get; set; }

        /// <summary>
        /// This method factory builds the Given Order for our Case Study
        /// John Smith, a customer in Ft Myers FL, ordered some products
        /// </summary>
        /// <returns></returns>
        public static ShoppingCart GivenOrder()
        {
            var address = new Address
            {
                FirstName = "John",
                LastName = "Smith",
                Line1 = "123 Main St",
                Line2 = "",
                City = "Fort Myers",
                State = "FL",
                Zip = "33933"
            };

            return new ShoppingCart()
            {
                BillingAddress = address,
                ShippingAddress = address,
                Items = new List<ShoppingItem>
                {
                    new ShoppingItem
                    {
                        Description = "Product A",
                        Quantity = 1,
                        Price = 4.99m,
                        ItemTotal = 4.99m
                    },
                    new ShoppingItem
                    {
                        Description = "Product A",
                        Quantity = 4,
                        Price = 2.50m,
                        ItemTotal = 10.00m
                    },

                }
            };
        }
    }

    public class ShoppingItem
    {
        public string Description { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal ItemTotal { get; set; }
    }
}
