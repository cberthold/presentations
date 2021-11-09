using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CreditCardProcessor.Strategies
{
    public class BrokenRequestStrategy : IRequestStrategy
    {
        public Task Process(AuthorizationRequest request, AuthorizationResponse response)
        {
            response.IsSucess = false;
            return Task.Delay(5000);
        }
    }
}
