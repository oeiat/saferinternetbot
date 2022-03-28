using System;
using System.Web.Http;
using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using oiat.saferinternetbot.Business;
using oiat.saferinternetbot.Business.Identity;
using oiat.saferinternetbot.DataAccess.Entities;
using Owin;

[assembly: OwinStartup(typeof(oiat.saferinternetbot.web.Startup))]

namespace oiat.saferinternetbot.web
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureDependencies(app);
            ConfigureAuth(app);
        }

        private void ConfigureDependencies(IAppBuilder app)
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule(new WebModule(app));
            var container = builder.Build();
            
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
            GlobalConfiguration.Configuration.DependencyResolver = new AutofacWebApiDependencyResolver(container);
        }

        private void ConfigureAuth(IAppBuilder app)
        {
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Login/Index"),
                Provider = new CookieAuthenticationProvider
                {
                    // Enables the application to validate the security stamp when the user logs in.
                    // This is a security feature which is used when you change a password or add an external login to your account.  
                    OnValidateIdentity = SecurityStampValidator.OnValidateIdentity<ApplicationUserManager, ApplicationUser>(
                        validateInterval: TimeSpan.FromMinutes(30),
                        regenerateIdentity: (manager, user) => user.GenerateUserIdentityAsync(manager))
                }
            });
        }
    }
}
