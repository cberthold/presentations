using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TaxApp.WPF.Common.Command
{
    public interface IAsyncCommandFactory<TCallingType>
    {
        AsyncCommand<object> Create(Func<Task> command);
        AsyncCommand<TResult> Create<TResult>(Func<Task<TResult>> command);
        AsyncCommand<object> Create(Func<CancellationToken, Task> command);
        AsyncCommand<TResult> Create<TResult>(Func<CancellationToken, Task<TResult>> command);
    }
}
