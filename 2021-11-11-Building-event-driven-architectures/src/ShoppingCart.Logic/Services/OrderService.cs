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
    public class OrderService
    {
        private readonly OrderTableClient client;
        private readonly CartService cartService;
        private readonly PaymentClient payment;

        public OrderService(OrderTableClient client, CartService cartService, PaymentClient payment)
        {
            this.client = client;
            this.cartService = cartService;
            this.payment = payment;
        }

        public async Task<Order> PlaceOrder(Guid cartId)
        {
            var cart = await cartService.LoadCart(cartId);
            var order = cart.MapToOrder();
            var authorization = order.MapToAuthorizationRequest();
            await client.SaveOrder(order);
            var response = await payment.AuthorizePayment(authorization);

            if(response.IsSucess)
            {
                order.Status = "Paid";
            }
            else
            {
                order.Status = "Declined";
            }

            await client.SaveOrder(order);

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
