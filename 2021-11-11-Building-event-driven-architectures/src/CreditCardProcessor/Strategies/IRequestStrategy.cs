using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CreditCardProcessor
{
    public interface IRequestStrategy
    {
        Task Process(AuthorizationRequest request, AuthorizationResponse response);
    }
}
