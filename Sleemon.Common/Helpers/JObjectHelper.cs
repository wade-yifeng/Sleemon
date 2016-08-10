namespace Sleemon.Common
{
    using System;

    using Newtonsoft.Json.Linq;

    public static class JObjectHelper
    {
        public static T TryGetValue<T>(JObject obj, string key)
        {
            if (obj == null) return (T)Convert.ChangeType(null, typeof(T));
            else if (typeof(T) == typeof(int))
                return (T)Convert.ChangeType(ConvertHelper.ConvertToInt(obj[key]), TypeCode.Int32);
            else if (typeof(T) == typeof(bool))
                return (T)Convert.ChangeType(ConvertHelper.ConvertToBool(obj[key]), TypeCode.Boolean);
            else if (typeof (T) == typeof (byte))
                return (T) Convert.ChangeType(ConvertHelper.ConvertToByte(obj[key]), TypeCode.Byte);
            else return (T)Convert.ChangeType(obj[key], typeof(T));
        }
    }
}
