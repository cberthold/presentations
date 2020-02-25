using System;
using System.Collections.Generic;
using System.Linq;
using CQRSlite.Domain;
using frontend.Data;

namespace frontend.Logic.DomainEvents
{
    public class AccountAggregate : AggregateRoot
    {
        public Guid AccountId { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string AccountType { get; private set; }
        public decimal CurrentBalance { get; private set; } = 0;
        public bool IsAccountOpen { get; private set; } = false;

        private static readonly HashSet<string> ALLOWED_ACCOUNT_TYPES = 
            new HashSet<string>(new string[]
            {
                "Checking",
                "Savings",
            });

        private AccountAggregate() : base()
        {}

        public static AccountAggregate OpenNewAccount(
            string firstName,
            string lastName,
            string accountType)
        {
            var accountId = Guid.NewGuid();

            if(!ALLOWED_ACCOUNT_TYPES.Contains(accountType))
            {
                throw new ApplicationException($"Account type of `{accountType}` is not an allowed account type.");
            }

            var account = new AccountAggregate();
            // normally opening a new account we would be generating this in another service
            account.Id = accountId;
            account.ApplyChange(new AccountOpenedEvent(accountId, firstName, lastName, accountType));
            return account;
        }

        private void Apply(AccountOpenedEvent @event)
        {
            AccountId = @event.AccountId;
            AccountType = @event.AccountType;
            FirstName = @event.FirstName;
            LastName = @event.LastName;
            IsAccountOpen = true;
            CurrentBalance = 0;
        }

        public void Deposit(Deposit deposit)
        {
            if(!IsAccountOpen)
            {
                throw new ApplicationException("Account is not open");
            }

            var depositedEvent = new AmountDepositedEvent(
                    deposit.DepositId,
                    deposit.Amount,
                    deposit.Date,
                    deposit.AccountId);

            ApplyChange(depositedEvent);
        }

        private void Apply(AmountDepositedEvent @event)
        {
            CurrentBalance += @event.Amount;
        }

        public void Withdraw(Withdrawal withdrawal)
        {
            if(!IsAccountOpen)
            {
                throw new ApplicationException("Account is not open");
            }

            var withdrawnEvent = new AmountWithdrawnEvent(
                    withdrawal.WithdrawalId,
                    withdrawal.Amount,
                    withdrawal.Date,
                    withdrawal.AccountId);

            ApplyChange(withdrawnEvent);
        }

        private void Apply(AmountWithdrawnEvent @event)
        {
            CurrentBalance -= @event.Amount;
        }
    }
}