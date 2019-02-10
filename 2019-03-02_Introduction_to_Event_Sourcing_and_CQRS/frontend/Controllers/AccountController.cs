using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using frontend.Models;
using frontend.Logic.Commands;
using frontend.Logic.Queries;
using MediatR;

namespace frontend.Controllers
{
    [Route("api/[controller]")]
    public class AccountController : Controller
    {
        private readonly IMediator mediator;

        public AccountController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet("{accountId:guid}/Transactions")]
        public Task<TransactionModel> GetTransactions(Guid accountId, CancellationToken token)
        {
            var query = new GetTransactionQuery
            {
                AccountId = accountId,
            };

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
    }
}
