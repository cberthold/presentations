using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CreditCardProcessor
{
    public class RequestProcessor : IRequestProcessor
    {
        private readonly IRequestStrategy strategy;

        public RequestProcessor(IRequestStrategy strategy)
        {
            this.strategy = strategy;
        }
        public async Task<AuthorizationResponse> Process(AuthorizationRequest request)
        {
            var response = new AuthorizationResponse();
            response.Amount = request.Amount;
            response.TransactionId = request.TransactionId;
            await strategy.Process(request, response);
            response.TimeStamp = DateTime.UtcNow;
            response.IsSucess = true; //always approved
            return response;
        }
    }
}
