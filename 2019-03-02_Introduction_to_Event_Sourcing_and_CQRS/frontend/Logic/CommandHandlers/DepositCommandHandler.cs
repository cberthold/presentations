using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using frontend.Models;
using frontend.Logic.Commands;
using MediatR;

namespace frontend.Logic.CommandHandlers
{
    public class DepositCommandHandler : IRequestHandler<DepositCommand, TransactionResponse>
    {
        public Task<TransactionResponse> Handle(DepositCommand request, CancellationToken cancellationToken)
        {
            var response = new TransactionResponse {
                Type = "Deposit",
                Amount = request.Amount,
            };

            return Task.FromResult(response);
        }
    }
}