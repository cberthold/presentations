using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using frontend.Logic.Commands;
using MediatR;

namespace frontend.Logic.CommandHandlers
{
    public class ReplayCommandHandler : IRequestHandler<ReplayCommand>
    {
        public async Task<Unit> Handle(ReplayCommand request, CancellationToken cancellationToken)
        {
            return Unit.Value;
        }
    }
}
