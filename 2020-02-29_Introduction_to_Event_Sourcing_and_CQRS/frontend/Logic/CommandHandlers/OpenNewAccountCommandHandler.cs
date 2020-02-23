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
using CQRSlite.Domain;

namespace frontend.Logic.CommandHandlers
{
    public class OpenNewAccountCommandHandler : IRequestHandler<OpenNewAccountCommand, Guid>
    {
        private readonly ISession session;

        public OpenNewAccountCommandHandler(ISession session)
        {
            this.session = session;
        }

        public async Task<Guid> Handle(OpenNewAccountCommand request, CancellationToken cancellationToken)
        {
            var account = AccountAggregate.OpenNewAccount(
                request.FirstName, 
                request.LastName,
                request.AccountType);
            await session.Add(account);
            await session.Commit(cancellationToken);
            
            return account.AccountId;
        }
    }
}