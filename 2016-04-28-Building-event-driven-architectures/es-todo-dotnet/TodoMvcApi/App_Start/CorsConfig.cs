using Microsoft.Owin.Cors;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Cors;

namespace TodoMvcApi
{
    public partial class Startup
    {

        // For more information on configuring authentication, please visit http://go.microsoft.com/fwlink/?LinkId=301864
        public void ConfigureCors(IAppBuilder app)
        {
            const string CORS_ORIGINS = "*";
            const string CORS_HEADERS = "*";
            const string CORS_METHODS = "GET, POST, OPTIONS, PUT, DELETE";

            var policy = new CorsPolicy()
            {
                AllowAnyHeader = true,
                AllowAnyMethod = true,
                AllowAnyOrigin = true
            };

            var corsOptions = new CorsOptions()
            {
                PolicyProvider = new CorsPolicyProvider
                {
                    PolicyResolver = context => Task.FromResult(policy)
                }
            };

            app.UseCors(corsOptions);
        }
    }
}