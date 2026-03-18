    using Company_Registration.APIServices;
    using Company_Registration.Utils;
    using System;
using System.Net.Http.Formatting;
using System.Web.Mvc;
    using Unity;
    using Unity.AspNet.Mvc;

    namespace Company_Registration
    {
        /// <summary>
        /// Specifies the Unity configuration for the main container.
        /// </summary>
        public static class UnityConfig
        {
            private static Lazy<IUnityContainer> container =
              new Lazy<IUnityContainer>(() =>
              {
                  var container = new UnityContainer();
                  RegisterTypes(container);
                  return container;
              });

            public static IUnityContainer Container => container.Value;
            public static void RegisterTypes(IUnityContainer container)
            {
                // Register Services
                container.RegisterType<ICompanyApplicantService, CompanyApplicantService>();

                // Register API Helper
                container.RegisterType<IAPIAccessHelper, ApiHelpers>();

                // Set MVC Dependency Resolver
                DependencyResolver.SetResolver(new UnityDependencyResolver(container));
                // Register your types here
                container.RegisterType<IContentNegotiator, DefaultContentNegotiator>();
            }
        }
    }