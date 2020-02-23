using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using frontend.Models;
using MediatR;

namespace frontend.Logic.Commands
{
    public class OpenNewAccountCommand : IRequest<Guid>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string AccountType { get; set; }
    }
}