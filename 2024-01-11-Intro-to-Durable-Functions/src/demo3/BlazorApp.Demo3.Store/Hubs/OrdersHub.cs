using Microsoft.AspNetCore.SignalR;

namespace BlazorApp.Demo3.Store.Hubs
{
    public class OrdersHub : Hub
    {
        public async Task UpdateOrders()
        {
            await Clients.All.SendAsync("UpdateOrders");
        }
    }
}
