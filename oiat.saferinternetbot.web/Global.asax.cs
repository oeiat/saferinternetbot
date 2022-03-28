using oiat.saferinternetbot.web.App_Start;
using System;
using System.Web;
using System.Web.Routing;
using System.Web.Http;

namespace oiat.saferinternetbot.web
{
    public class Global : HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.Register();
        }
    }
}