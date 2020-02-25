using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using frontend.Logic.Commands;
using Logic.Projections;
using MediatR;

namespace frontend.Logic.CommandHandlers
{
    public class ReplayCommandHandler : IRequestHandler<ReplayCommand>
    {
        private readonly IProjectionService service;

        public ReplayCommandHandler(IProjectionService service)
        {
            this.service = service;
        }

        public async Task<Unit> Handle(ReplayCommand request, CancellationToken cancellationToken)
        {
            await service.ReplayProjections(cancellationToken);
            return Unit.Value;
        }
    }
}
