using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using SqlStreamStore;
using Logic.Projections;

namespace Infrastructure
{
    public class ProjectionHostedService : BackgroundService
    {
        private readonly IProjectionService service;

        public ProjectionHostedService(IProjectionService service)
        {
            this.service = service;
        }
        
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            service.SetStopToken(stoppingToken);
            await service.StartProjections();
        }
    }
}