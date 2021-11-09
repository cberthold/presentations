using CreditCardProcessor.Strategies;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;


[assembly: FunctionsStartup(typeof(CreditCardProcessor.Startup))]
namespace CreditCardProcessor
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            //builder.Services.AddHttpClient();
            // sets the mode service for tracking whih mode we are in
            builder.Services.AddSingleton<IProcessModeService, ProcessModeService>();
            // register individual request strategies
            builder.Services.AddTransient<NormalRequestStrategy>();
            builder.Services.AddTransient<SlowRequestStrategy>();
            builder.Services.AddTransient<BrokenRequestStrategy>();
            // picks the request strategy based on the current mode
            builder.Services.AddTransient<IRequestStrategy>(svc =>
            {
                var modeService = svc.GetRequiredService<IProcessModeService>();
                var mode = modeService.Mode;
                switch (mode)
                {
                    case ProcessMode.Broken:
                        return svc.GetRequiredService<BrokenRequestStrategy>();                    
                    case ProcessMode.Slow:
                        return svc.GetRequiredService<SlowRequestStrategy>();
                    default:
                        return svc.GetRequiredService<NormalRequestStrategy>();
                }
            });
            builder.Services.AddScoped<IRequestProcessor, RequestProcessor>();
            
        }
    }
}
