using ShoppingCart.Logic.DTO;
using System;
using System.Threading.Tasks;

namespace ShoppingCart.Logic.Services
{
    public interface ICartService
    {
        Task<Cart> AddCartItemAsync(string productName, decimal amount);
        Task<Cart> LoadCart();
        Task<Cart> LoadCart(Guid cartId);
        Task<Cart> RemoveCartItemAsync(string itemId);
        Task ResetCart();
    }
}