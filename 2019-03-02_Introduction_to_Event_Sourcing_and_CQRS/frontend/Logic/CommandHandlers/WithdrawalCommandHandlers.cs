using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using frontend.Models;
using frontend.Logic.Commands;
using MediatR;
using frontend.Data;
using frontend.Logic.DomainEvents;

namespace frontend.Logic.CommandHandlers
{
    public class WithdrawalCommandHandler : IRequestHandler<WithdrawalCommand, TransactionResponse>
    {
        private readonly IBankAccountsContext context;
        private readonly IMediator mediator;

        public WithdrawalCommandHandler(IBankAccountsContext context, IMediator mediator)
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

            using (var tx = await context.Database.BeginTransactionAsync(cancellationToken))
            {
                context.Withdrawals.Add(withdrawal);
                await context.SaveChangesAsync(cancellationToken);

                tx.Commit();
                
                var depositedEvent = new AmountWithdrawnEvent(
                    withdrawal.WithdrawalId,
                    withdrawal.Amount,
                    withdrawal.Date,
                    withdrawal.AccountId);

                await mediator.Publish(depositedEvent, cancellationToken);
                

                var response = new TransactionResponse
                {
                    Type = "Withdrawal",
                    Amount = request.Amount,
                };

                return response;
            }
        }

    }
}