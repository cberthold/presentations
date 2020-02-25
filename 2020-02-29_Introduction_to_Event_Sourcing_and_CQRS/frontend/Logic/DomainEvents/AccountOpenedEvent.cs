using System;
using System.Collections.Generic;
using System.Linq;
using MediatR;

namespace frontend.Logic.DomainEvents
{
    public class AccountOpenedEvent : AbstractDomainEvent
    {
        public Guid AccountId { get; private set; }

        public string FirstName { get; private set; }

        public string LastName { get; private set; }

        public string AccountType { get; private set; }

        public AccountOpenedEvent(
            Guid accountId,
            string firstName,
            string lastName,
            string accountType)
        {
            AccountId = accountId;
            FirstName = firstName;
            LastName = lastName;
            AccountType = accountType;
        }
    }
}