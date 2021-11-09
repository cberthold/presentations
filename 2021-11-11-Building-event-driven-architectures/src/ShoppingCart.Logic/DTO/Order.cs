using System;
using System.Collections.Generic;
using System.Text;

namespace ShoppingCart.Logic.DTO
{
    public class Order
    {
        public Guid SessionId { get; set; }

        public string DatePartitionKey { get; set; }

        public string OrderRowKey { get; set; }

        public DateTime OrderDate { get; set; }

        public string Status { get; set; }

        public decimal OrderAmount { get; set; }
    }
}
