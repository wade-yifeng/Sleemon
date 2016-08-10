namespace Sleemon.WebApi
{
    using System;
    using System.Configuration;

    public class AppSettingsHelper
    {
        public static T GetAppSetting<T>(string key, T defaultValue)
        {
            var value = ConfigurationManager.AppSettings[key];

            if (value == null) return defaultValue;

            return (T)Convert.ChangeType((object)value, typeof(T));
        }
    }
}
