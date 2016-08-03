using Autofac;
using TaxApp.WPF.Navigation;
using TaxApp.WPF.Views.MainWindow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaxApp.WPF
{
    public class AppBootstrap
    {
        public static void Configure(ContainerBuilder builder)
        {
            builder.RegisterAssemblyModules(typeof(AppBootstrap).Assembly);
                        
        }
    }
}
