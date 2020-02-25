using System;
using System.Threading;
using System.Threading.Tasks;
using frontend.Data;
using frontend.Logic.DomainEvents;
using LiquidProjections;
using SqlStreamStore;
using SqlStreamStore.Streams;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Logic.Projections
{
    public abstract class AbstractProjectionBase<TContext> : IDisposable
        where TContext : DbContext
    {
        private IStreamStore store;
        private IServiceProvider provider;
        private IEventMap<TContext> map;
        private IAllStreamSubscription subscription;

        public AbstractProjectionBase(IStreamStore store, IServiceProvider provider)
        {
            this.store = store;
            this.provider = provider;
            map = BuildMap();
        }

        private IEventMap<TContext> BuildMap()
        {
            var builder = new EventMapBuilder<TContext>();

            BuildEventMap(builder);

            return builder.Build(new ProjectorMap<TContext>
            {
                Custom = CreateProjectorMap(),
            });
        }

        protected virtual CustomHandler<TContext> CreateProjectorMap()
        {
            return (ctx, projector) => projector();
        }

        protected abstract void BuildEventMap(EventMapBuilder<TContext> builder);

        private int contextCounter = 0;
        private IServiceScope childScope = null;

        protected abstract string GetProjectionName();

        private async Task<long?> GetLastPosition(CancellationToken cancellationToken)
        {

            var lastPosition = default(long?);
            using(var scope = provider.CreateScope())
            {
                var context = scope.ServiceProvider.GetService<BankAccountsContext>();
                var projectionName = GetProjectionName();
                var checkpointQuery = context.Checkpoints.Where(a => a.ProjectionName == projectionName).Select(a => (long?)a.LastCheckpoint);
                lastPosition = await checkpointQuery.FirstOrDefaultAsync(cancellationToken);

                if(lastPosition == null)
                {
                    context.Checkpoints.Add(new Checkpoint
                    {
                        ProjectionName = GetProjectionName(),
                        LastCheckpoint = 0,
                    });
                    await context.SaveChangesAsync();
                }
            }

            return lastPosition;
        }

        private void DisposeSubscription()
        {
            try 
            {
                subscription?.Dispose();
            }
            finally
            {
                subscription = null;
            }
        }

        public async Task StartProjection(CancellationToken cancellationToken)
        {
            await RestartProjection(cancellationToken);
            cancellationToken.Register(() => {
                DisposeSubscription();
            });
        }

        public void StopProjection()
        {
            DisposeSubscription();
        }

        private async Task RestartProjection(CancellationToken cancellationToken)
        {
            DisposeSubscription();
            var lastPosition = await GetLastPosition(cancellationToken);
            subscription = this.store.SubscribeToAll(lastPosition, StreamMessageReceived);
        }

        private async Task StreamMessageReceived(IAllStreamSubscription subscription, StreamMessage streamMessage, CancellationToken cancellationToken)
        {
            var streamEvent = await streamMessage.RehydrateEventObject(cancellationToken);

            if(contextCounter % 50 == 0)
            {
                childScope?.Dispose();
                childScope = null;
            }

            contextCounter++;

            childScope = childScope ?? provider.CreateScope();
            var context = childScope.ServiceProvider.GetService<TContext>();
            using(var tx = await context.Database.BeginTransactionAsync()) 
            {
                const string UPDATE_SQL = "UPDATE Checkpoints SET LastCheckpoint = @p0 WHERE ProjectionName = @p1";
                await HandleStreamEvent(streamEvent, childScope, context);
                await context.Database.ExecuteSqlCommandAsync(UPDATE_SQL, streamMessage.Position, GetProjectionName());
                tx.Commit();
            }
            
        }

        protected virtual Task HandleStreamEvent(object streamEvent, IServiceScope scope, TContext context)
        {
            return map.Handle(streamEvent, context);
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    DisposeSubscription();
                    this.store = null;
                    this.provider = null;
                    this.map = null;
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~TransactionViewProjection()
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