using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CQRSlite.Domain;
using CQRSlite.Domain.Exception;
using frontend.Logic.DomainEvents;

namespace CQRSlite.Domain
{
    public static class AggregateExtensions
    {
        public static async Task<AccountAggregate> GetAccount(this ISession session, Guid accountId, CancellationToken token)
        {
                var account = await session.Get<AccountAggregate>(accountId, cancellationToken: token);
                return account;
        }
    }
}