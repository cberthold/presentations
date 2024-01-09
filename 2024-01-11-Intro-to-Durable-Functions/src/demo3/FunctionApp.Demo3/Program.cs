using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureServices(svc =>
    {
        svc.AddHttpClient(Options.DefaultName);
    })
    .Build();

host.Run();
