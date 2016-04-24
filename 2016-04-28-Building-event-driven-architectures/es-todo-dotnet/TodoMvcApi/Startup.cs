using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(TodoMvcApi.Startup))]

namespace TodoMvcApi
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // configure Dependency Injection through Autofac
            ConfigureAutofac(app);
            // setup our command handlers to handle incoming commands
            ConfigureCommandHandlers(app);
        }
    }
}
