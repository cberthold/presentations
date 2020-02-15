using System;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace frontend.Data
{
    public abstract class DesignTimeContextFactory<TContext>
        : IDesignTimeDbContextFactory<TContext>
        where TContext : DbContext
    {

        protected abstract void ConfigureOptions(IConfiguration configuration, DbContextOptionsBuilder<TContext> builder);

        public TContext CreateDbContext(string[] args)
        {
            // create our configuration for design time building of the context
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            // create our dbcontext configuration builder we will allow
            // our implementing class to configure
            var builder = new DbContextOptionsBuilder<TContext>();

            // configure the options
            ConfigureOptions(configuration, builder);

            // build our new context using the options passed
            var instance = CreateInstance(builder.Options);
            return instance;
        }

        private TContext CreateInstance(DbContextOptions<TContext> options)
        {
            var context = (TContext)Activator.CreateInstance(typeof(TContext), args: new object[] { options });
            return context;
        }
    }
}