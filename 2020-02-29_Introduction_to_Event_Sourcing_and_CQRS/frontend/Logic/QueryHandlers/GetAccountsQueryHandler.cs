using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using frontend.Models;
using frontend.Logic.Queries;
using MediatR;
using frontend.Data;
using Microsoft.EntityFrameworkCore;

namespace frontend.Logic.QueryHandlers
{
    public class GetAccountsQueryHandler : IRequestHandler<GetAccountsQuery, IEnumerable<AccountModel>>
    {
        private readonly IBankAccountsContext context;
        public GetAccountsQueryHandler(IBankAccountsContext context)
        {
            this.context = context;
        }
        public async Task<IEnumerable<AccountModel>> Handle(GetAccountsQuery request, CancellationToken cancellationToken)
        {

            var accountsFromDb = (from t in context.Accounts
                              orderby t.AccountName
                              select t);


            var items = await accountsFromDb.Select(item => new AccountModel
            {
                AccountId = item.AccountId,
                AccountName = item.AccountName,
                CurrentBalance = item.CurrentBalance,
            }).ToArrayAsync(cancellationToken);

            return items;
        }
    }
}