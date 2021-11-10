using Azure.Data.Tables;
using ShoppingCart.Logic.Clients;
using ShoppingCart.Logic.DTO;
using ShoppingCart.Logic.Mapping;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.Logic.Services
{
    public class OrderService : IOrderService
    {
        private readonly OrderTableClient client;
        private readonly ICartService cartService;
        private readonly IPaymentService paymentService;

        public OrderService(OrderTableClient client, ICartService cartService, IPaymentService paymentService)
        {
            this.client = client;
            this.cartService = cartService;
            this.paymentService = paymentService;
        }

        public async Task<Order> PlaceOrder(Guid cartId)
        {
            var cart = await cartService.LoadCart(cartId);
            var order = cart.MapToOrder();
            await client.SaveOrder(order);
            await paymentService.AuthorizePayment(order);

            return order;
        }

        public async Task<Order> GetOrder(string orderDate, string orderKey)
        {
            var orderEntity = await client.GetEntityAsync<TableEntity>(orderDate, orderKey);
            var order = orderEntity.Value.MapToOrder();
            return order;
        }
    }
}
