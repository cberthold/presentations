using System;
using System.Threading;
using System.Threading.Tasks;
using frontend.Data;
using frontend.Logic.DomainEvents;
using LiquidProjections;
using SqlStreamStore;
using SqlStreamStore.Streams;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Logic.Projections
{
    public class TransactionViewProjection : AbstractProjectionBase<BankAccountsContext>
    {
        public TransactionViewProjection(IStreamStore store, IServiceProvider provider) 
            : base(store, provider)
        {
        }

        protected override void BuildEventMap(EventMapBuilder<BankAccountsContext> builder)
        {
            builder.Map<AmountDepositedEvent>()
                .As(async (e, ctx) => {
                    var transaction = new TransactionsView
                    {
                        TransactionId = e.DepositId,
                        Amount = e.Amount,
                        Date = e.Date,
                        AccountId = e.AccountId,
                        Type = "Depsit"
                    };

                    await ctx.Transactions.AddAsync(transaction);
                    await ctx.SaveChangesAsync();
                });

            builder.Map<AmountWithdrawnEvent>()
                .As(async (e, ctx) => {
                    var transaction = new TransactionsView
                    {
                        TransactionId = e.WithdrawalId,
                        Amount = e.Amount,
                        Date = e.Date,
                        AccountId = e.AccountId,
                        Type = "Withdrawal"
                    };

                    await ctx.Transactions.AddAsync(transaction);
                    await ctx.SaveChangesAsync();
                });
        }

        protected override string GetProjectionName()
        {
            var projectionName = nameof(TransactionViewProjection);
            return projectionName;
        }
    }

}