using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Globalization;
using System.Threading;
using NetspeedMainWebsite.Binders;

namespace NetspeedMainWebsite
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            AntiForgeryConfig.UniqueClaimTypeIdentifier = ClaimTypes.NameIdentifier;

            ModelBinders.Binders[typeof(DateTime?)] = new DateBinder();
            ModelBinders.Binders[typeof(DateTime)] = new DateBinder();
           

        }
        //protected void Application_PreSendRequestHeaders(object sender, EventArgs e)
        //{
        //    // This code will mark the __RequestVerificationToken cookie SameSite=Strict 
        //    if (Request.Cookies.Count > 0)
        //    {
        //        foreach (string s in Request.Cookies.AllKeys)
        //        {
        //            if (s.ToLower() == "__requestverificationtoken")
        //            {
        //                HttpCookie c = Request.Cookies[s];
        //                c.SameSite = System.Web.SameSiteMode.None;
        //                c.Secure = false;
        //                Response.Cookies.Set(c);
        //            }
        //        }
        //    }
        //}
    }
}
