using System;

namespace frontend.Logic.DomainEvents
{
    public static class EventCatalog
    {
        public static class Types
        {
            public static class Account
            {
                public static readonly Type Opened = typeof(AccountOpenedEvent);
                public static readonly Type Deposit = typeof(AmountDepositedEvent);
                public static readonly Type Withdrawal = typeof(AmountWithdrawnEvent);
            }
        }

        public static class TypeNames 
        {
            public static class Account
            {
                public static readonly string Opened = Types.Account.Opened.FullName;
                public static readonly string Deposit = Types.Account.Deposit.FullName;
                public static readonly string Withdrawal = Types.Account.Withdrawal.FullName;
            }
        }
    }
}