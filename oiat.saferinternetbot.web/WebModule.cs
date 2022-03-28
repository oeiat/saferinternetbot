
using System.CodeDom;
using System.Reflection;
using System.Web;
using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using Microsoft.Owin.Security.DataProtection;
using oiat.saferinternetbot.Business;
using oiat.saferinternetbot.Core;
using oiat.saferinternetbot.Web;
using Owin;
using Module = Autofac.Module;

namespace oiat.saferinternetbot.web
{
    public class WebModule : Module
    {
        private readonly IAppBuilder _app;

        public WebModule(IAppBuilder app)
        {
            _app = app;
        }
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterModule<AutofacWebTypesModule>();
            builder.RegisterControllers(Assembly.GetExecutingAssembly());
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            builder.Register(c => _app.GetDataProtectionProvider()).InstancePerLifetimeScope();
            builder.Register(c => HttpContext.Current.GetOwinContext().Authentication);
            builder.RegisterModule<BusinessModule>();
            builder.RegisterModule<BotModule>();

            var mappingModule = new MappingModule(typeof(WebModule), typeof(BusinessModule));
            builder.RegisterModule(mappingModule);
        }
    }
}