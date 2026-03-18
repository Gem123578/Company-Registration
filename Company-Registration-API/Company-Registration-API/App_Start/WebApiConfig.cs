using Company_Registration_API.DataAccess;
using Company_Registration_API.Services;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;
using System.Web.Http;
using Unity;
using Unity.Lifetime;

namespace Company_Registration_API
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();
            // Unity DI setup
            var container = new UnityContainer();

            container.RegisterType<ApplicantDbContext>(new HierarchicalLifetimeManager());
            container.RegisterType<ApplicantRegistrationDao>(new HierarchicalLifetimeManager());
            container.RegisterType<CompanyRegistrationDao>(new HierarchicalLifetimeManager());
            container.RegisterType<ICompanyApplicantService, CompanyApplicantService>(new HierarchicalLifetimeManager());
            container.RegisterType<ICompanyRegistrationService, CompanyRegistrationService>(new HierarchicalLifetimeManager());

            config.DependencyResolver = new UnityResolver(container);

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
