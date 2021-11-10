using Azure.Data.Tables;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShoppingCart.Logic.Clients
{
    public class CartTableClient : TableClient
    {
        public CartTableClient(string connectionString) : base(connectionString, "Cart") { }
    }
}
