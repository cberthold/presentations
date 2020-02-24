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
    public class AccountProjection
    {
        private readonly IStreamStore store;
        private readonly IServiceProvider provider;
        private readonly IEventMap<BankAccountsContext> map;
        private IAllStreamSubscription subscription;

        public AccountProjection(IStreamStore store, IServiceProvider provider)
        {
            this.store = store;
            this.provider = provider;
            map = BuildMap();
        }

        private static IEventMap<BankAccountsContext> BuildMap()
        {
            var builder = new EventMapBuilder<BankAccountsContext>();
            
            builder.Map<AccountOpenedEvent>()
                .As(async (e, ctx) => {
                    var accountShort = e.AccountId.ToString("d");
                    accountShort = accountShort.Substring(accountShort.Length - 6, 6);
                    var account = new Account
                    {
                        AccountId = e.AccountId,
                        AccountName =  $"{e.FirstName} {e.LastName} - ({accountShort})",
                    };

                    await ctx.Accounts.AddAsync(account);
                    await ctx.SaveChangesAsync();
                });

            builder.Map<AmountDepositedEvent>()
                .As(async (e, ctx) => {
                    var account = await ctx.Accounts.FindAsync(e.AccountId);
                    account.CurrentBalance += e.Amount;
                    await ctx.SaveChangesAsync();
                });

            builder.Map<AmountWithdrawnEvent>()
                .As(async (e, ctx) => {
                    var account = await ctx.Accounts.FindAsync(e.AccountId);
                    account.CurrentBalance -= e.Amount;
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