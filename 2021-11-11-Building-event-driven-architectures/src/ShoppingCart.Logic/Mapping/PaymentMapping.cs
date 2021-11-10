using ShoppingCart.Logic.DTO;
using ShoppingCart.Logic.Payment;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShoppingCart.Logic.Mapping
{
    public static class PaymentMapping
    {
        public static AuthorizationRequest MapToAuthorizationRequest(this Order order)
        {
            var auth = new AuthorizationRequest();
            auth.TransactionId = order.SessionId;
            auth.Amount = order.OrderAmount;
            return auth;
        }
    }
}
