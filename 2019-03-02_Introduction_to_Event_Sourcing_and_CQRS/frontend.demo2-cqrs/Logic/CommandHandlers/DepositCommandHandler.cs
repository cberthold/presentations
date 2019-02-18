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
    public class DepositCommandHandler : IRequestHandler<DepositCommand, TransactionResponse>
    {
        private readonly IBankAccountsContext context;
        private readonly IMediator mediator;

        public DepositCommandHandler(IBankAccountsContext context, IMediator mediator)
        {
            this.context = context;
            this.mediator = mediator;
        }

        public async Task<TransactionResponse> Handle(DepositCommand request, CancellationToken cancellationToken)
        {
            var deposit = new Deposit
            {
                Date = DateTime.Today,
                Amount = request.Amount,
                AccountId = request.AccountId,
            };

            using (var tx = await context.Database.BeginTransactionAsync(cancellationToken))
            {

                context.Deposits.Add(deposit);
                await context.SaveChangesAsync(cancellationToken);
                
                var depositedEvent = new AmountDepositedEvent(
                    deposit.DepositId,
                    deposit.Amount,
                    deposit.Date,
                    deposit.AccountId);

                await mediator.Publish(depositedEvent, cancellationToken);
                
                tx.Commit();

                var response = new TransactionResponse
                {
                    Type = "Deposit",
                    Amount = request.Amount,
                };

                return response;
            }

        }
    }
}