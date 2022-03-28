using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Net;
using Autofac;
using oiat.saferinternetbot.Business;
using oiat.saferinternetbot.Core;
using mbit.common.cache;
using oiat.saferinternetbot.LuisApi.ApiClient;
using oiat.saferinternetbot.DataAccess;
using System.Data.Entity;

namespace oiat.whatsapp.functions.Infrastructure
{
    public sealed class DIContainer
    {
        private static readonly IContainer _instance = Build();

        public static IContainer Instance => _instance;

        static DIContainer()
        { }

        private DIContainer()
        { }

        private static IContainer Build()
        {
            var builder = new ContainerBuilder();

            #region Settings
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();

            builder.RegisterInstance(configuration).As<IConfiguration>();
            #endregion

            builder.Register(activator =>
            {
                var config = activator.Resolve<IConfiguration>();
                var apiSettings = config.GetSection("MessengerPeopleAPI");
                var clientId = apiSettings["ClientId"];
                var clientSecret = apiSettings["ClientSecret"];
                var scope = apiSettings["Scope"];
                var authUrl = apiSettings["AuthUrl"];
                var sendUrl = apiSettings["SendUrl"];

                var apiClient = new MessengerApiClient(clientId, clientSecret, scope, authUrl, sendUrl);
                return apiClient;
            });

            builder.RegisterModule<BusinessModule>();
            var mappingModule = new MappingModule(typeof(BusinessModule));
            builder.RegisterModule(mappingModule);
            builder.RegisterType<NoCacheService>().As<ICacheService>();

            if (!configuration.GetValue<bool>("LuisAPI:TestMode"))
            {
                builder.Register(activator =>
                {
                    var config = activator.Resolve<IConfiguration>();
                    var settings = config.GetSection("LuisAPI");
                    return new RestLuisApiClient(settings["BaseUrl"], settings["BaseScoreUrl"], settings["AppId"], settings["AppVersion"], settings["AppKey"], settings["AppScoreKey"]);
                }).As<ILuisApiClient>().InstancePerLifetimeScope();
            }

            builder.Register(activator =>
            {
                var config = activator.Resolve<IConfiguration>();
                var connectionString = config.GetConnectionString("DefaultConnection");
                return new DataContext(connectionString);
            }).As<DbContext>().InstancePerLifetimeScope();

            return builder.Build();
        }
    }
}
