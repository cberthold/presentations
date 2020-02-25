using System;
using System.Threading;
using System.Threading.Tasks;
using SqlStreamStore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using frontend.Data;

namespace Logic.Projections
{
    public class ProjectionService : IProjectionService, IDisposable
    {
        private readonly IServiceProvider provider;
        private IStreamStore store;
        private AccountProjection accountProjection;
        private TransactionViewProjection transactionViewProjection;
        private CancellationToken stopToken;

        public ProjectionService(IServiceProvider provider)
        {
            this.provider = provider;
        }

        public void SetStopToken(CancellationToken stopToken)
        {
            this.stopToken = stopToken;
        }

        public Task StartProjections()
        {
            return RestartProjections(stopToken);
        }

        public void StopProjections()
        {
            DisposeProjections();
            DisposeStore();
        }

        public async Task ReplayProjections(CancellationToken cancellationToken)
        {
            StopProjections();

            using(var childScope = provider.CreateScope())
            {
                var childProvider = childScope.ServiceProvider;
                var context = childProvider.GetService<BankAccountsContext>();
                var data = context.Database;
                using(var tx = await data.BeginTransactionAsync())
                {
                    const string TRUNCATE_CHECKPOINTS = "TRUNCATE TABLE [dbo].[Checkpoints]";
                    const string TRUNCATE_ACCOUNTS = "TRUNCATE TABLE [dbo].[Accounts]";
                    const string TRUNCATE_TRANSACTIONS = "TRUNCATE TABLE [dbo].[Transactions]";
                    await data.ExecuteSqlCommandAsync(TRUNCATE_CHECKPOINTS, cancellationToken);
                    await data.ExecuteSqlCommandAsync(TRUNCATE_ACCOUNTS, cancellationToken);
                    await data.ExecuteSqlCommandAsync(TRUNCATE_TRANSACTIONS, cancellationToken);
                    tx.Commit();
                }
            }

            await RestartProjections(cancellationToken);
        }

        private async Task RestartProjections(CancellationToken cancellationToken)
        {
            DisposeProjections();
            DisposeStore();

            store = provider.GetService<IStreamStore>();
            accountProjection = new AccountProjection(store, provider);
            transactionViewProjection = new TransactionViewProjection(store, provider);

            await accountProjection.StartProjection(cancellationToken);
            await transactionViewProjection.StartProjection(cancellationToken);
        }

        private void DisposeProjections()
        {
            try 
            {
                accountProjection?.StopProjection();
                accountProjection?.Dispose();
            }
            finally
            {
                accountProjection = null;
            }

            try 
            {
                transactionViewProjection?.StopProjection();
                transactionViewProjection?.Dispose();
            }
            finally
            {
                transactionViewProjection = null;
            }
        }

        private void DisposeStore()
        {
            try 
            {
                store?.Dispose();
            }
            finally
            {
                store = null;
            }
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                    DisposeProjections();
                    DisposeStore();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~ProjectionService()
        // {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
} 
