
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using ShoppingCart.Logic.Clients;
using ShoppingCart.Payments;
using System;
using System.Collections.Generic;
using System.Text;


[assembly: FunctionsStartup(typeof(Startup))]
namespace ShoppingCart.Payments
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            
            builder.Services.AddHttpClient<PaymentClient>((client) =>
            {
                client.BaseAddress = new Uri("http://localhost:7071/api/");
            });
        }
    }
}
