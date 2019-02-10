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
    public class WithdrawalCommandHandler : IRequestHandler<WithdrawalCommand, TransactionResponse>
    {
        private readonly IBankAccountsContext context;

        public WithdrawalCommandHandler(IBankAccountsContext context)
        {
            this.context = context;
        }

        public async Task<TransactionResponse> Handle(WithdrawalCommand request, CancellationToken cancellationToken)
        {
            var withdrawal = new Withdrawal
            {
                Date = DateTime.Today,
                Amount = request.Amount,
                AccountId = request.AccountId,
            };

            context.Withdrawals.Add(withdrawal);
            await context.SaveChangesAsync(cancellationToken);

            var response = new TransactionResponse {
                Type = "Withdrawal",
                Amount = request.Amount,
            };

            return response;
        }

    }
}