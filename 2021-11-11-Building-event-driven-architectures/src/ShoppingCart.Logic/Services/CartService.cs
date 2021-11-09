using Azure;
using Azure.Data.Tables;
using ShoppingCart.Logic.Clients;
using ShoppingCart.Logic.DTO;
using ShoppingCart.Logic.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.Logic.Services
{
    public class CartService
    {
        private readonly CartTableClient client;
        private readonly ISessionService sessionService;

        public CartService(CartTableClient client, ISessionService sessionService)
        {
            this.client = client;
            this.sessionService = sessionService;
        }

        public async Task<Cart> LoadCart()
        {
            var cartId = await sessionService.AddOrLoadSessionId();
            return await LoadCart(cartId);
        }

        public async Task<Cart> LoadCart(Guid cartId)
        {
            var cart = new Cart();
            cart.Id = cartId;

            var items = await GetCartItemsForCartId(cartId);
            cart.AddRange(items);
            return cart;
        }

        private async Task<IEnumerable<CartItem>> GetCartItemsForCartId(Guid id)
        {

            string filter = $"PartitionKey eq '{id.ToString("N")}'";
            AsyncPageable<TableEntity> entities = client.QueryAsync<TableEntity>(filter);
            
            var list = await entities.Select(e => e.MapToCartItem()).ToListAsync();
            var sorted = list.OrderBy(a => a.Position);
            return sorted;
        }

        public async Task<Cart> RemoveCartItemAsync(string itemId)
        {
            var cart = await LoadCart();

            string partitionKey = cart.Id.ToString("N");
            string rowKey = itemId;

            await client.DeleteEntityAsync(partitionKey, rowKey);

            return await LoadCart();
        }

        public async Task<Cart> AddCartItemAsync(string productName, decimal amount)
        {
            var cart = await LoadCart();

            var item = new CartItem();
            item.Position = cart.Count == 0 ? 1 : cart.Max(a => a.Position) + 1;
            item.ProductName = productName;
            item.Amount = amount;
            item.CartId = cart.Id;
            item.ItemId = Guid.NewGuid();
            
            var entity = item.MapToTableEntity();
            await client.UpsertEntityAsync(entity);


            return await LoadCart();
        }
    }
}
    