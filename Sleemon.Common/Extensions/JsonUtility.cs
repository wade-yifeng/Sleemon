namespace Sleemon.Common
{
    using System.IO;

    using Newtonsoft.Json;

    public static class JsonUtility
    {
        private static readonly JsonSerializer JsonSerializer = new JsonSerializer();

        public static string SerializeToJson(this object data)
        {
            var json = string.Empty;

            try
            {
                using (var writer = new StringWriter())
                {
                    JsonSerializer.Serialize(writer, data);
                    writer.Flush();
                    json = writer.GetStringBuilder().ToString();
                }
            }
            catch
            { }

            return json;
        }
    }
}
