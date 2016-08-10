using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Sleemon.Portal.Common
{
    public static class Extensions
    {
        public static string ToJsonContent(this object content)
        {
            JsonConvert.DefaultSettings = () =>
            {
                var settings = new JsonSerializerSettings();
                settings.Converters.Add(new StringEnumConverter());

                return settings;
            };
            return JsonConvert.SerializeObject(content);
        }
    }
}