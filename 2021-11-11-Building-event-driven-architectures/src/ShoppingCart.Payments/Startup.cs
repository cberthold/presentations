
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ShoppingCart.Logic.Clients;
using ShoppingCart.Logic.Services;
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

            builder.Services.AddScoped<IPaymentService, PaymentService>();
            builder.Services.AddSingleton<OrderTableClient>((svc) =>
            {
                var config = svc.GetRequiredService<IConfiguration>();
                var client = new OrderTableClient(config["PaymentConnection"]);
                client.CreateIfNotExists();
                return client;
            });

            builder.Services.AddHttpClient<PaymentClient>((client) =>
            {
                client.BaseAddress = new Uri("http://localhost:7071/api/");
            });
        }
    }
}
