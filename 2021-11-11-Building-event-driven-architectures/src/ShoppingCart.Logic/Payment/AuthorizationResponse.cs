using System;
using System.Collections.Generic;
using System.Text;

namespace ShoppingCart.Logic.Payment
{
    public class AuthorizationResponse
    {
        public bool IsSucess { get; set; }
        
        public decimal Amount { get; set; }

        public Guid TransactionId { get; set; }

        public DateTime TimeStamp { get; set; }
    }
}
