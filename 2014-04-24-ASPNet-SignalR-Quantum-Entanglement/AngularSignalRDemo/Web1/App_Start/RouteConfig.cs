// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RouteConfig.cs" company="">
//   Copyright © 2014 
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace App.Web1
{
    using System.Web.Routing;

    using App.Web1.Routing;
    using System.Web.Mvc;

    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            /*
            routes.MapRoute(
                name: "HomeDefault",
                url: "{controller}/{action}/{id}",
                defaults: new { action = "Index", id = UrlParameter.Optional }
            );
            */
            routes.Add("Default", new DefaultRoute());
        }
    }
}
