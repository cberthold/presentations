using ShoppingCart.Logic.DTO;
using System.Threading.Tasks;

namespace ShoppingCart.Logic.Services
{
    public interface IPaymentService
    {
        Task AuthorizePayment(Order order);
    }
}