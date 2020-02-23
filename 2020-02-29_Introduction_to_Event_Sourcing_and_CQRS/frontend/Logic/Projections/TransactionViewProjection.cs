using System;
using System.Threading;
using System.Threading.Tasks;
using frontend.Data;
using frontend.Logic.DomainEvents;
using LiquidProjections;
using SqlStreamStore;
using SqlStreamStore.Streams;
using Microsoft.Extensions.DependencyInjection;

namespace Logic.Projections
{
    public class TransactionViewProjection
    {
        private readonly IStreamStore store;
        private readonly IServiceProvider provider;
        private readonly IEventMap<BankAccountsContext> map;
        private IAllStreamSubscription subscription;

        public TransactionViewProjection(IStreamStore store, IServiceProvider provider)
        {
            this.store = store;
            this.provider = provider;
            map = BuildMap();
        }

        private static IEventMap<BankAccountsContext> BuildMap()
        {
            var builder = new EventMapBuilder<BankAccountsContext>();
            
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

            return builder.Build(new ProjectorMap<BankAccountsContext>
            {
                Custom = (ctx, projector) => projector()
            });
        }

        private int contextCounter = 0;
        private IServiceScope childScope = null;



        public Task StartProjection(CancellationToken cancellationToken)
        {
            subscription = this.store.SubscribeToAll(null, StreamMessageReceived);

            cancellationToken.Register(() => {
                try 
                {
                    subscription?.Dispose();
                }
                finally
                {
                    subscription = null;
                }
            });
            
            return Task.CompletedTask;
        }

        private async Task StreamMessageReceived(IAllStreamSubscription subscription, StreamMessage streamMessage, CancellationToken cancellationToken)
        {
            var streamEvent = await streamMessage.RehydrateEventObject(cancellationToken);

            if(contextCounter % 50 == 0)
            {
                childScope?.Dispose();
                childScope = null;
            }

            contextCounter++;

            childScope = childScope ?? provider.CreateScope();
            var context = childScope.ServiceProvider.GetService<BankAccountsContext>();
            await map.Handle(streamEvent, context);
        }
    }

}