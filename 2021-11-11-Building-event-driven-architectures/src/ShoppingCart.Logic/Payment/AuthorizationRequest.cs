using System;
using System.Collections.Generic;
using System.Text;

namespace ShoppingCart.Logic.Payment
{
    public class AuthorizationRequest
    {
        public decimal Amount { get; set; }

        public Guid TransactionId { get; set; }
    }
}
