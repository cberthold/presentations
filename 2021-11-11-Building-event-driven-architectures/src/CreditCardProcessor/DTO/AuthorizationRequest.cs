using System;
using System.Collections.Generic;
using System.Text;

namespace CreditCardProcessor
{
    public class AuthorizationRequest
    {
        public decimal Amount { get; set; }

        public Guid TransactionId { get; set; }
    }
}
