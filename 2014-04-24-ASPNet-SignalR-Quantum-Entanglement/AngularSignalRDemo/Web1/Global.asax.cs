// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Global.asax.cs" company="">
//   Copyright © 2014 
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace App.Web1
{
    using App.Web1.Code;
    using Microsoft.AspNet.SignalR;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;
    using System.Collections.Generic;
    using System.IdentityModel;
    using System.IdentityModel.Services;
    using System.IdentityModel.Tokens;
    using System.Web;
    using System.Web.Http;
    using System.Web.Mvc;
    using System.Web.Optimization;
    using System.Web.Routing;

    public class Application : HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            GlobalConfiguration.Configure(WebApiConfig.Register);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            var repo = RandomUserRepository.GetInstance();


            var formatters = GlobalConfiguration.Configuration.Formatters;
            // force json all the time
            formatters.Remove(formatters.XmlFormatter);
            var jsonFormatter = formatters.JsonFormatter;
            var settings = jsonFormatter.SerializerSettings;
            settings.Formatting = Formatting.Indented;
            // set to camelCasedPropertyNames
            settings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            FederatedAuthentication.FederationConfigurationCreated += FederatedAuthentication_FederationConfigurationCreated;
            

           
        }

        void FederatedAuthentication_FederationConfigurationCreated(object sender, System.IdentityModel.Services.Configuration.FederationConfigurationCreatedEventArgs e)
        {
            var serviceCert = e.FederationConfiguration.ServiceCertificate;
            //
            // Use the <serviceCertificate> to protect the cookies that are
            // sent to the client.
            //
            List<CookieTransform> sessionTransforms =
                new List<CookieTransform>(new CookieTransform[] {
        new DeflateCookieTransform(), 
        new RsaEncryptionCookieTransform(serviceCert),
        new RsaSignatureCookieTransform(serviceCert)  });
            SessionSecurityTokenHandler sessionHandler = new SessionSecurityTokenHandler(sessionTransforms.AsReadOnly());

            e.FederationConfiguration.IdentityConfiguration.SecurityTokenHandlers.AddOrReplace(sessionHandler);
        }


    }
}
