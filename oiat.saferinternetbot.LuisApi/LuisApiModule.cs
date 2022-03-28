using Autofac;
using mbit.common.Settings;
using oiat.saferinternetbot.LuisApi.ApiClient;

namespace oiat.saferinternetbot.LuisApi
{
    public class LuisApiModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var settings = McSettingsManager.RetrieveSettings<LuisSettings>();

            if (settings.TestMode)
            {
                builder.RegisterInstance(new MockLuisApiClient()).As<ILuisApiClient>().SingleInstance();
            }
            else
            {
                builder.RegisterInstance(new RestLuisApiClient(settings.BaseUrl, settings.BaseScoreUrl, settings.AppId, settings.AppVersion, settings.AppKey, settings.AppScoreKey)).As<ILuisApiClient>().SingleInstance();
            }
        }
    }
}
