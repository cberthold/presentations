using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace TaxApp.WPF.Navigation
{
    public class NavigationService : INavigationService
    {
        private IContainer container;
        private ILifetimeScope previousPageScope;

        public NavigationService(IContainer container)
        {
            this.container = container;
        }

        private TControl CreatePage<TControl>(ILifetimeScope pageScope)
            where TControl : UserControl
        {
            var page = pageScope.Resolve<TControl>();
            return page;
        }

        private StackPanel GetMainFrame()
        {
            var frame = (StackPanel)Application.Current.MainWindow.FindName("_mainFrame");
            return frame;
        }
        

        private void NavigateToPage(UserControl page, ILifetimeScope scope)
        {
            var frame = GetMainFrame();
            frame.Children.Clear();
            
            frame.Children.Add(page);
            
            if (previousPageScope != null)
            {
                previousPageScope.Dispose();
            }

            
            previousPageScope = scope;
        }

        public void NavigateTo<TControl>() 
            where TControl : UserControl
        {
            var pageScope = container.BeginLifetimeScope();
            var page = CreatePage<TControl>(pageScope);

            NavigateToPage(page, pageScope);
        }
        

        public void NavigateTo<TControl>(object pageParameters) 
            where TControl : UserControl
        {
            throw new NotImplementedException();
        }
    }
}
