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
        private readonly IStreamStore store;
        private readonly IServiceProvider provider;
        private readonly TransactionViewProjection transactionView;
        private readonly AccountProjection account;

        public ProjectionHostedService(IStreamStore store, IServiceProvider provider)
        {
            this.store = store;
            this.provider = provider;
            transactionView = new TransactionViewProjection(store, provider);
            account = new AccountProjection(store, provider);
        }
        
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await transactionView.StartProjection(stoppingToken);
            await account.StartProjection(stoppingToken);
        }
    }
}