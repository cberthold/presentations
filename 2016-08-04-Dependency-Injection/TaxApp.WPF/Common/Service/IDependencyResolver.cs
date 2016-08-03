using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaxApp.WPF.Common.Service
{
    public interface IDependencyResolver
    {
        object Resolve(Type serviceType);
        TService Resolve<TService>();
        bool TryResolve(Type serviceType, out object instance);
        bool TryResolve<T>(out T instance);
    }
}
