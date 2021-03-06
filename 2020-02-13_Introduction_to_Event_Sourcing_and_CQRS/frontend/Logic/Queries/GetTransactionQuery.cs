using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using frontend.Models;
using MediatR;

namespace frontend.Logic.Queries
{
    public class GetTransactionQuery : IRequest<TransactionModel>
    {
        public Guid AccountId { get; set; }
    }
}