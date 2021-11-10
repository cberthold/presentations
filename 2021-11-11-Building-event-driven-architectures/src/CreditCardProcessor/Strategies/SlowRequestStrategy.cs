using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CreditCardProcessor.Strategies
{
    public class SlowRequestStrategy : IRequestStrategy
    {
        public Task Process(AuthorizationRequest request, AuthorizationResponse response)
        {
            response.IsSucess = true;
            return Task.Delay(5000);
        }
    }
}
