using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;

namespace Company_Registration
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            UnityConfig.RegisterTypes(UnityConfig.Container);
        }
        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {
            HttpCookie authCookie = Request.Cookies[FormsAuthentication.FormsCookieName];
            if (authCookie != null)
            {
                try
                {
                    FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(authCookie.Value);
                    if (authTicket != null && !string.IsNullOrEmpty(authTicket.Name))
                    {
                        // GenericIdentity အစား FormsIdentity အသုံးပြု
                        var identity = new FormsIdentity(authTicket);
                        var principal = new System.Security.Principal.GenericPrincipal(identity, new string[] { });
                        HttpContext.Current.User = principal;

                        // Session restore only if session exists
                        var ctx = HttpContext.Current;
                        if (ctx != null && ctx.Session != null)
                        {
                            ctx.Session["UserName"] = ctx.Session["UserName"] ?? authTicket.Name;
                            ctx.Session["ApplicantId"] = ctx.Session["ApplicantId"] ?? authTicket.UserData;
                        }
                    }
                }
                catch
                {
                    // invalid cookie
                }
            }
        }

    }

}


