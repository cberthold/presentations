using System;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace frontend.Data
{
    public class BankAccountsContext : DbContext
    {
        public BankAccountsContext(DbContextOptions<BankAccountsContext> options)
            : base(options)
        { }

        public DbSet<Account> Accounts { get; set; }
        public DbSet<Deposit> Deposits { get; set; }
    }

    public class Account
    {
        public Guid AccountId { get; set; }
        public string AccountName { get; set; }
        public decimal CurrentBalance { get; set; }

        public List<Deposit> Deposits { get; set; }
        public List<Withdrawal> Withdrawals { get; set; }
    }

    public class Deposit
    {
        public Guid DepositId { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }

        public int AccountId { get; set; }
        public Account Account { get; set; }
    }

    public class Withdrawal
    {
        public Guid WithdrawalId { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }

        public int AccountId { get; set; }
        public Account Account { get; set; }
    }
}