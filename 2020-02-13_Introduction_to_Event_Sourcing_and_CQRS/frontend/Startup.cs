using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MediatR;
using frontend.Data;
using Microsoft.EntityFrameworkCore;
using CQRSlite.Domain;
using frontend.Infrastructure;
using CQRSlite.Events;

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

            // add bank accounts context
            services.AddDbContext<BankAccountsContext>(options => 
                options.UseSqlServer(Configuration.GetConnectionString("BankAccountContext")));

            // and map to its interface as well
            services.AddScoped<IBankAccountsContext, BankAccountsContext>((svc) => svc.GetRequiredService<BankAccountsContext>());
            
            services.AddScoped<ISession, Session>();
            services.AddScoped<IRepository, Repository>();
            services.AddScoped<SqlEventStore>();
            services.AddScoped<IEventStore>((s) => s.GetRequiredService<SqlEventStore>());
            services.AddScoped<IReplayEventStore>((s) => s.GetRequiredService<SqlEventStore>());
            services.AddScoped<Infrastructure.IEventPublisher, Infrastructure.MediatorPublisher>();
        
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
