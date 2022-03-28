using mbit.common.Settings.Attributes;

namespace oiat.saferinternetbot.LuisApi
{
    public class LuisSettings
    {
        [McSettingsValue("LuisTestMode", "true")]
        public bool TestMode { get; set; }

        [McSettingsValue("LuisBaseUrl", "")]
        public string BaseUrl { get; set; }

        [McSettingsValue("LuisBaseScoreUrl", "")]
        public string BaseScoreUrl { get; set; }

        [McSettingsValue("LuisAppId", "")]
        public string AppId { get; set; }

        [McSettingsValue("LuisAppVersion", "")]
        public string AppVersion { get; set; }

        [McSettingsValue("LuisAppKey", "")]
        public string AppKey { get; set; }

        [McSettingsValue("LuisAppScoreKey", "")]
        public string AppScoreKey { get; set; }
    }
}
