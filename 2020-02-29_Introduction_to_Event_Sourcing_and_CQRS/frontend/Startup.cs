using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MediatR;
using frontend.Data;
using Microsoft.EntityFrameworkCore;
using CQRSlite.Domain;
using CQRSlite.Events;
using Infrastructure;
using SqlStreamStore;

namespace frontend
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();

            // add mediatr
            services.AddMediatR();

            // In production, the React files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/build";
            });

            string bankAccountConnectionString = Configuration.GetConnectionString("BankAccountContext");

            // add bank accounts context
            services.AddDbContext<BankAccountsContext>(options => 
                options.UseSqlServer(bankAccountConnectionString));

            // and map to its interface as well
            services.AddScoped<IBankAccountsContext, BankAccountsContext>((svc) => svc.GetRequiredService<BankAccountsContext>());
            
            services.AddScoped<ISession, Session>();
            services.AddTransient<IRepository, Repository>();

            // sql stream store
            services.AddTransient<IEventStore, SqlStreamStoreEventStoreAdapter>();

            services.AddSingleton(
                (svc)=> new MsSqlStreamStoreV3Settings(bankAccountConnectionString)
                {
                    Schema = "ES",
                    DisableDeletionTracking = true,
                });
            
            services.AddTransient<MsSqlDatabaseInitializer>();
            services.AddTransient<MsSqlStreamStoreV3>();
            services.AddTransient<IStreamStore>(svc => svc.GetService<MsSqlStreamStoreV3>());
            services.AddHostedService<ProjectionHostedService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSpaStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
            });

            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseReactDevelopmentServer(npmScript: "start");
                }
            });
        }
    }
}
