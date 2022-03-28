using System;
using System.Configuration;
using System.Linq;
using System.Reflection;
using Autofac;
using AutoMapper;
using mbit.common.cache;
using mbit.common.Settings;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Azure.Cosmos.Table;
using oiat.saferinternetbot.Business.Identity;
using oiat.saferinternetbot.Business.Mappings;
using oiat.saferinternetbot.DataAccess;
using oiat.saferinternetbot.DataAccess.Entities;
using oiat.saferinternetbot.LuisApi;
using Module = Autofac.Module;

namespace oiat.saferinternetbot.Business
{
    public class BusinessModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterModule<DataModule>();
            builder.RegisterModule<LuisApiModule>();

            var cacheSettings = McSettingsManager.RetrieveSettings<CacheSettings>();
            builder.RegisterType<MemoryCacheService>().As<ICacheService>().WithParameter("cacheExpirationInMinutes", cacheSettings.CacheExpirationInMinutes).SingleInstance();

            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            builder.RegisterType<IdentityFactoryOptions<ApplicationSignInManager>>().AsSelf().InstancePerLifetimeScope();

            builder.RegisterType<RoleStore<IdentityRole>>().As<IRoleStore<IdentityRole, string>>().InstancePerLifetimeScope();

            builder.RegisterType<UserStore<ApplicationUser>>().As<IUserStore<ApplicationUser>>().InstancePerLifetimeScope();

            builder.RegisterType<ApplicationUserManager>().AsSelf().InstancePerLifetimeScope();
            builder.RegisterType<ApplicationSignInManager>().AsSelf().InstancePerLifetimeScope();
            builder.RegisterType<RoleManager<IdentityRole>>().AsSelf().InstancePerLifetimeScope();

            var storageConnectionString = ConfigurationManager.ConnectionStrings["StorageConnectionString"]?.ConnectionString;
            if (!string.IsNullOrEmpty(storageConnectionString))
            {
                builder.RegisterInstance(CloudStorageAccount.Parse(storageConnectionString)).AsSelf().SingleInstance();
                builder.Register(x => x.Resolve<CloudStorageAccount>().CreateCloudTableClient()).AsSelf().InstancePerLifetimeScope();
            }
        }
    }
}
