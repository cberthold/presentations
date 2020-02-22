using System;
using System.Linq;
using frontend.Data;
using Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;

namespace frontend
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            if(args?.Where(a => a.ToLower() == "initdb").Any() == true)
            {
                var dbinit = host.Services.GetService<MsSqlDatabaseInitializer>();
                dbinit.Initialize();

                var context = host.Services.GetService<BankAccountsContext>();
                context.Database.Migrate();
            }
            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
