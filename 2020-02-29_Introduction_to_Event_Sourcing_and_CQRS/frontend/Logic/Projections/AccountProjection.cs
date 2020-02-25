using System;
using System.Threading;
using System.Threading.Tasks;
using frontend.Data;
using frontend.Logic.DomainEvents;
using LiquidProjections;
using SqlStreamStore;
using SqlStreamStore.Streams;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Logic.Projections
{
    public class AccountProjection : AbstractProjectionBase<BankAccountsContext>
    {
        public AccountProjection(IStreamStore store, IServiceProvider provider) 
            : base(store, provider)
        {
        }

        protected override void BuildEventMap(EventMapBuilder<BankAccountsContext> builder)
        {
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
        }

        protected override string GetProjectionName()
        {
            var projectionName = nameof(AccountProjection);
            return projectionName;
        }
    }
}