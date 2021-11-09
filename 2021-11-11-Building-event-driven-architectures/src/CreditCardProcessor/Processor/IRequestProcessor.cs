using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CreditCardProcessor
{
    public interface IRequestProcessor
    {
        Task<AuthorizationResponse> Process(AuthorizationRequest request);
    }
}
