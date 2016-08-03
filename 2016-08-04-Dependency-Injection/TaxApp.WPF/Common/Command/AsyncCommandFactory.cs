using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TaxApp.WPF.Common.Command
{
    public class AsyncCommandFactory<TCallingType> : IAsyncCommandFactory<TCallingType>
    {
        private readonly Logger _logger;

        public AsyncCommandFactory(Logger logger)
        {
            _logger = logger;
        }


        public AsyncCommand<object> Create(Func<Task> command)
        {
            return new AsyncCommand<object>(_logger, async _ => { await command(); return null; });
        }

        public AsyncCommand<TResult> Create<TResult>(Func<Task<TResult>> command)
        {
            return new AsyncCommand<TResult>(_logger, _ => command());
        }

        public AsyncCommand<object> Create(Func<CancellationToken, Task> command)
        {
            return new AsyncCommand<object>(_logger, async token => { await command(token); return null; });
        }

        public AsyncCommand<TResult> Create<TResult>(Func<CancellationToken, Task<TResult>> command)
        {
            return new AsyncCommand<TResult>(_logger, command);
        }
    }
}
