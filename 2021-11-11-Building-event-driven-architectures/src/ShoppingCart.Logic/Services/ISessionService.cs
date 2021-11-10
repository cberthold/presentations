using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.Logic.Services
{
    public interface ISessionService
    {
        Task<Guid> AddOrLoadSessionId();

        Task<Guid> ResetSessionId();
    }
}
