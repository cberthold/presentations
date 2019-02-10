using System;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

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

        public Account() {
            AccountId = Guid.NewGuid();
        }


        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid AccountId { get; set; }
        public string AccountName { get; set; }
        public decimal CurrentBalance { get; set; }

        public List<Deposit> Deposits { get; set; }
        public List<Withdrawal> Withdrawals { get; set; }
    }

    public class Deposit
    {
        public Deposit() {
            DepositId = Guid.NewGuid();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid DepositId { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }

        public int AccountId { get; set; }
        public Account Account { get; set; }
    }

    public class Withdrawal
    {
        public Withdrawal() {
            WithdrawalId = Guid.NewGuid();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid WithdrawalId { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }

        public int AccountId { get; set; }
        public Account Account { get; set; }
    }
}