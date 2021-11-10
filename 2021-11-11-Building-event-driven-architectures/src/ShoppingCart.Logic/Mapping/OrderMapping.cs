using Azure.Data.Tables;
using ShoppingCart.Logic.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShoppingCart.Logic.Mapping
{
    public static class OrderMapping
    {
        public static Order MapToOrder(this Cart cart)
        {
            var fullId = cart.Id.ToString("N");
            var orderKey = fullId.Substring(fullId.Length - 10, 10);
            var order = new Order();
            order.DatePartitionKey = DateTime.Now.ToString("yyyyMMdd");
            order.OrderRowKey = orderKey;
            order.SessionId = cart.Id;
            order.OrderDate = DateTime.UtcNow;
            order.Status = "New";
            order.OrderAmount = cart.Total;
            return order;
        }

        public static TableEntity MapToTableEntity(this Order order)
        {
            var tableEntity = new TableEntity(order.DatePartitionKey, order.OrderRowKey);
            tableEntity["SessionId"] = order.SessionId;
            tableEntity["OrderDate"] = order.OrderDate;
            tableEntity["Status"] = order.Status;
            tableEntity["OrderAmount"] = order.OrderAmount;

            return tableEntity;
        }

        public static Order MapToOrder(this TableEntity entity)
        {
            var order = new Order();
            order.DatePartitionKey = entity.PartitionKey;
            order.OrderRowKey = entity.RowKey;
            order.SessionId = (Guid)entity["SessionId"];
            order.OrderDate = ((DateTimeOffset)entity["OrderDate"]).UtcDateTime;
            order.Status = (string)entity["Status"];
            order.OrderAmount = Convert.ToDecimal(entity["OrderAmount"]);

            return order;
        }
    }
}
