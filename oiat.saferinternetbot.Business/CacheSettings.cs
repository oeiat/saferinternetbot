using mbit.common.Settings.Attributes;

namespace oiat.saferinternetbot.Business
{
    public class CacheSettings
    {
        [McSettingsValue("CacheExpirationInMinutes", "1440")]
        public int CacheExpirationInMinutes { get; set; }
    }
}
