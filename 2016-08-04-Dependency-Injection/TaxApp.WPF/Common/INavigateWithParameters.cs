using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaxApp.WPF.Common
{
    public interface INavigateWithParameters<TParameters>
        where TParameters : class
    {
        void SetParameters(TParameters parameters);
    }
}
