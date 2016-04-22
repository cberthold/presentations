using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace TodoMvcApi
{
    public partial class Startup
    {

        // For more information on configuring authentication, please visit http://go.microsoft.com/fwlink/?LinkId=301864
        public void ConfigureAutofac(IAppBuilder app)
        {

            var builder = new ContainerBuilder();
            // register MVC controllers
            builder.RegisterControllers(Assembly.GetExecutingAssembly());
            // register web abstractions
            //builder.RegisterModule<AutofacWebTypesModule>();
            // register property injection in view pages
            //builder.RegisterSource(new ViewRegistrationSource());
            // enable property injection into action filters
            builder.RegisterFilterProvider();

            // register WebAPI controllers
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            builder.RegisterModule<CustomerApiModule>();

            // build IoC Container
            var container = builder.Build();

            // set MVC dependency resolver
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

            // Register the Autofac middleware FIRST, then the Autofac Web API middleware,
            // and finally the standard MVC middleware.
            app.UseAutofacMiddleware(container);
            //app.UseAutofacMvc();

            // configure CORS
            ConfigureCors(app);
            
            // get WebAPI configurations
            var config = new HttpConfiguration();

            // set WebAPI dependency resolver
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);

            // register WebAPI routes
            WebApiConfig.Register(config);

            // OWIN WEB API SETUP:

            // Register the Autofac middleware FIRST, then the Autofac Web API middleware,
            // and finally the standard Web API middleware.
            app.UseAutofacWebApi(config);
            app.UseWebApi(config);

            //app.Map("/api",
            //    inner =>
            //    {

            //        // get WebAPI configurations
            //        var config = new HttpConfiguration();

            //        // set WebAPI dependency resolver
            //        config.DependencyResolver = new AutofacWebApiDependencyResolver(container);

            //        // register WebAPI routes
            //        WebApiConfig.Register(config);

            //        // OWIN WEB API SETUP:

            //        // Register the Autofac middleware FIRST, then the Autofac Web API middleware,
            //        // and finally the standard Web API middleware.
            //        app.UseAutofacWebApi(config);
            //        app.UseWebApi(config);

            //    });


        }
    }
}