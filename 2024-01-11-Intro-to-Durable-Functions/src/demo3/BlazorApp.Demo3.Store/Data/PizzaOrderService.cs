using System.Collections.Concurrent;
using BlazorApp.Demo3.Store.Hubs;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Options;
using Shared.Demo3;

namespace BlazorApp.Demo3.Store.Data
{
    public class PizzaOrderService
    {
        private readonly ConcurrentQueue<PizzaOrderRequest> orderQueue = new ConcurrentQueue<PizzaOrderRequest>();
        private readonly ConcurrentDictionary<int, PizzaOrderRequest> orders = new ConcurrentDictionary<int, PizzaOrderRequest>();

        private readonly IHubContext<OrdersHub> hubContext;
        private readonly IHttpClientFactory httpClientFactory;

        public PizzaOrderService(IHubContext<OrdersHub> hubContext, IHttpClientFactory httpClientFactory)
        {
            this.hubContext = hubContext;
            this.httpClientFactory = httpClientFactory;
        }


        public async Task AddOrder(PizzaOrderRequest order)
        {

            orders.TryAdd(order.Order.OrderNumber, order);
            orderQueue.Enqueue(order);
            await hubContext.Clients.All.SendAsync("UpdateOrders");
        }

        public PizzaOrderRequest[] GetOrders()
        {
            return orderQueue.ToArray();
        }

        public async Task CompleteOrder(int orderId)
        {
            if (orderQueue.TryPeek(out PizzaOrderRequest order) 
                && order.Order.OrderNumber == orderId)
            {
                if (orderQueue.TryDequeue(out var request))
                {
                    // complete order
                    var client = httpClientFactory.CreateClient(Options.DefaultName);
                    client.BaseAddress = new Uri("http://localhost:7034/api/");
                    string url = $"pizza/made/{request.InstanceId}";
                    await client.GetAsync(url);
                }
            }
            await hubContext.Clients.All.SendAsync("UpdateOrders");
        }
    }
}
