using Azure.Data.Tables;
using ShoppingCart.Logic.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShoppingCart.Logic.Mapping
{
    public static class CartItemMapping
    {
        public static CartItem MapToCartItem(this TableEntity entity)
        {
            var item = new CartItem();

            item.CartId = Guid.Parse(entity.PartitionKey);
            item.ItemId = Guid.Parse(entity.RowKey);
            item.Timestamp = entity.Timestamp;
            item.Etag = entity.ETag.ToString();
            item.Amount = Convert.ToDecimal(entity["Amount"]);
            item.ProductName = (string)entity["ProductName"];
            item.Position = (int)entity["Position"];

            return item;
        } 

        public static TableEntity MapToTableEntity(this CartItem item)
        {
            var entity = new TableEntity();

            entity.PartitionKey = item.CartId.ToString("N");
            entity.RowKey = item.ItemId.ToString("N");
            entity["Amount"] = item.Amount;
            entity["ProductName"] = item.ProductName;
            entity["Position"] = item.Position;

            return entity;
        }
    }
}
