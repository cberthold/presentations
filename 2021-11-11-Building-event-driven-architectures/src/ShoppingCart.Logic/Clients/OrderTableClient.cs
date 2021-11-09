using Azure.Data.Tables;
using ShoppingCart.Logic.DTO;
using ShoppingCart.Logic.Mapping;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.Logic.Clients
{
    public class OrderTableClient : TableClient
    {
        public OrderTableClient(string connectionString) : base(connectionString, "Order") { }

        public async Task SaveOrder(Order order)
        {
            var entity = order.MapToTableEntity();
            await UpsertEntityAsync(entity);
        }

        public async Task<Order> GetOrder(string orderDate, string orderKey)
        {
            var orderEntity = await GetEntityAsync<TableEntity>(orderDate, orderKey);
            var order = orderEntity.Value.MapToOrder();
            return order;
        }
    }
}
