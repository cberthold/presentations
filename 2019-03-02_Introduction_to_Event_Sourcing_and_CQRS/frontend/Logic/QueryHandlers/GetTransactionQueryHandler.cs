using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using frontend.Models;
using MediatR;

namespace frontend.Logic.Queries
{
    public class GetTransactionQueryHandler : IRequestHandler<GetTransactionQuery, TransactionModel>
    {
        public Task<TransactionModel> Handle(GetTransactionQuery request, CancellationToken cancellationToken)
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
    }
}