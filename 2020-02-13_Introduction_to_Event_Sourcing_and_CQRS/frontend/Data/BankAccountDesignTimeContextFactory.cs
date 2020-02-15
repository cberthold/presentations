using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace frontend.Data
{
    public class BankAccountDesignTimeContextFactory : DesignTimeContextFactory<BankAccountsContext>
    {
        protected override void ConfigureOptions(IConfiguration configuration, DbContextOptionsBuilder<BankAccountsContext> builder)
        {
            builder.UseSqlServer(configuration.GetConnectionString("BankAccountContext"));
        }
    }
}