using Microsoft.Owin;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

[assembly: OwinStartup(typeof(Web2.App_Start.Startup))]
namespace Web2.App_Start
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // this maps signalr hubs to ~/signalr
            app.MapSignalR();
        }
    }
}