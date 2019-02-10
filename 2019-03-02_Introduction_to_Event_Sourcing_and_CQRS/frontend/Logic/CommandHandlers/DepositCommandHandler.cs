using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using frontend.Models;
using frontend.Logic.Commands;
using MediatR;
using frontend.Data;

namespace frontend.Logic.CommandHandlers
{
    public class DepositCommandHandler : IRequestHandler<DepositCommand, TransactionResponse>
    {
        private readonly IBankAccountsContext context;

        public DepositCommandHandler(IBankAccountsContext context)
        {
            this.context = context;
        }

        public async Task<TransactionResponse> Handle(DepositCommand request, CancellationToken cancellationToken)
        {
            var deposit = new Deposit
            {
                Date = DateTime.Today,
                Amount = request.Amount,
                AccountId = request.AccountId,
            };

            context.Deposits.Add(deposit);
            await context.SaveChangesAsync(cancellationToken);

            var response = new TransactionResponse {
                Type = "Deposit",
                Amount = request.Amount,
            };

            return response;
        }
    }
}