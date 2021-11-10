using ShoppingCart.Logic.Clients;
using ShoppingCart.Logic.DTO;
using ShoppingCart.Logic.Mapping;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.Logic.Services
{
    public class QueuedPaymentService : IPaymentService
    {
        private readonly OrderTableClient orderClient;
        private readonly AuthorizationQueueClient client;

        public QueuedPaymentService(OrderTableClient orderClient, AuthorizationQueueClient client)
        {
            this.orderClient = orderClient;
            this.client = client;
        }

        public async Task AuthorizePayment(Order order)
        {
            order.Status = "Processing";
            await orderClient.SaveOrder(order);
            await client.Authorize(order);
        }
    }
}
