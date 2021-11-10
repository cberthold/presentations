using System;
using System.Collections.Generic;
using System.Text;

namespace ShoppingCart.Logic.DTO
{
    public class CartItem
    {
        public Guid CartId { get; set; }

        public Guid ItemId { get; set; }

        public DateTimeOffset? Timestamp { get; set; }

        public string Etag { get; set; }

        public int Position { get; set; }

        public string ProductName { get; set; }

        public decimal Amount { get; set; }
    }
}
