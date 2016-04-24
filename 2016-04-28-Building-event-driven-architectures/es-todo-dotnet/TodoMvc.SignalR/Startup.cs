using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;
using Microsoft.AspNet.SignalR.Owin;
using Microsoft.Owin.Cors;

[assembly: OwinStartup(typeof(TodoMvc.SignalR.Startup))]

namespace TodoMvc.SignalR
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseCors(CorsOptions.AllowAll);
            app.MapSignalR();
            
        }
    }
}
