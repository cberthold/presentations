using ShoppingCart.Logic.DTO;
using System;
using System.Threading.Tasks;

namespace ShoppingCart.Logic.Services
{
    public interface IOrderService
    {
        Task<Order> GetOrder(string orderDate, string orderKey);
        Task<Order> PlaceOrder(Guid cartId);
    }
}