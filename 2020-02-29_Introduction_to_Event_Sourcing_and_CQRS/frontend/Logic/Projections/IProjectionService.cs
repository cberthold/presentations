using System;
using System.Threading;
using System.Threading.Tasks;

namespace Logic.Projections
{
    public interface IProjectionService
    {
        void SetStopToken(CancellationToken stopToken);
        Task StartProjections();
        void StopProjections();
        Task ReplayProjections(CancellationToken cancellationToken);
    }
} 