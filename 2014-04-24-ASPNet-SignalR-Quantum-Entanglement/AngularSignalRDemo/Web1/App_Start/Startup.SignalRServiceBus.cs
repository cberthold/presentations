using Microsoft.AspNet.SignalR;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace App.Web1
{
	public partial class Startup
	{
		public void ConfigureSignalRServiceBus(IAppBuilder app)
        {
            string serviceBusConnectionString = "Endpoint=sb://swfldotnetsignalr.servicebus.windows.net/;SharedSecretIssuer=owner;SharedSecretValue=b6asjUzi45qp/JHIGxjJC2wnPMUA1hY4nKg5+3NPPeo=";

            var scaleOutConfig = new ServiceBusScaleoutConfiguration(serviceBusConnectionString, "web1");
            GlobalHost.DependencyResolver.UseServiceBus(scaleOutConfig);

            app.MapSignalR();
        }
	}
}