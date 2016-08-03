using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace TaxApp.WPF.Navigation
{
    public interface INavigationService
    {
        void NavigateTo<TControl>(object pageParameters)
            where TControl : UserControl;
        void NavigateTo<TControl>()
            where TControl : UserControl;
    }
}
