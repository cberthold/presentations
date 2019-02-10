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
    public class WithdrawalCommandHandler : IRequestHandler<WithdrawalCommand, TransactionResponse>
    {
        public Task<TransactionResponse> Handle(WithdrawalCommand request, CancellationToken cancellationToken)
        {
            var response = new TransactionResponse {
                Type = "Withdrawal",
                Amount = request.Amount,
            };

            return Task.FromResult(response);
        }

    }
}