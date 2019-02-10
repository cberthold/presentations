using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace frontend.Controllers
{
    [Route("api/[controller]")]
    public class AccountController : Controller
    {
        [HttpGet("{accountId:guid}/Transactions")]
        public Task<TransactionModel> GetTransactions(Guid accountId, CancellationToken token)
        {
            var items = Enumerable.Range(-5, 5).Select(index => new TransactionItem
            {
                Id = Guid.NewGuid(),
                DateFormatted = DateTime.Now.AddDays(index).ToString("d"),
                Type = "Deposit",
                Amount = 1000 + index * 3,
                Summary = "Deposited at bank"
            });

            var tx = new TransactionModel()
            {
                CurrentBalance = items.Sum(a=> a.Amount),
                Transactions = items,
            };

            return Task.FromResult(tx);
        }

        [HttpPost("{accountId:guid}/[action]")]
        public IActionResult Deposit(Guid accountId, DepositModel model, CancellationToken token)
        {
            var response = new TransactionResponse {
                Type = "Deposit",
                Amount = model.Amount,
            };

            return Ok(response);
        }

        [HttpPost("{accountId:guid}/[action]")]
        public IActionResult Withdrawal(Guid accountId, WithdrawalModel model, CancellationToken token)
        {
            var response = new TransactionResponse {
                Type = "Withdrawal",
                Amount = model.Amount,
            };

            return Ok(response);
        }

        public class TransactionResponse {
            public string Type { get; set; }
            public decimal Amount {get; set; }
        }

        public class DepositModel
        {
            public decimal Amount { get; set; }
        }

        public class WithdrawalModel
        {
            public decimal Amount { get; set; }
        }

        public class TransactionModel
        {
            public decimal CurrentBalance {get; set; }
            public IEnumerable<TransactionItem> Transactions { get; set; }
        }

        public class TransactionItem
        {
            public Guid Id { get; set; }
            public string DateFormatted { get; set; }
            public string Type { get; set; }
            public decimal Amount { get; set; }
            public string Summary { get; set; }
        }
    }
}
