using ShoppingCart.Logic.Clients;
using ShoppingCart.Logic.DTO;
using ShoppingCart.Logic.Mapping;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.Logic.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly OrderTableClient orderClient;
        private readonly PaymentClient payment;

        public PaymentService(OrderTableClient orderClient, PaymentClient payment)
        {
            this.orderClient = orderClient;
            this.payment = payment;
        }

        public async Task AuthorizePayment(Order order)
        {
            var authorization = order.MapToAuthorizationRequest();

            try
            {
                var response = await payment.AuthorizePayment(authorization);

                if (response.IsSucess)
                {
                    order.Status = "Paid";
                }
                else
                {
                    order.Status = "Declined";
                }
            }
            catch
            {
                //order.Status = "Failed";
            }

            await orderClient.SaveOrder(order);
        }
    }
}
