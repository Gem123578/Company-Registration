using Company_Registration_API.DataAccess;
using Company_Registration_API.Services;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;

namespace Company_Registration_API
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Cross-origin allow
            var cors = new EnableCorsAttribute("https://localhost:44389", "*", "*");
            config.EnableCors(cors);

            // Web API routes
            config.MapHttpAttributeRoutes();
            // Unity DI setup
           
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
            // Remove XML
            config.Formatters.Remove(config.Formatters.XmlFormatter);

        }
    }
}
