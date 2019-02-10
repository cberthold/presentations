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
        public async Task<TransactionModel> GetTransactions(Guid accountId, CancellationToken token)
        {
            var query = new GetTransactionQuery
            {
                AccountId = accountId,
            };

            var result = await mediator.Send(query, token);
            return result;
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
