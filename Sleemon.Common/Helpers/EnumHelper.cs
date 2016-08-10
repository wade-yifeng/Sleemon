namespace Sleemon.Common
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Reflection;

    public class EnumHelper
    {
        public static string GetEnumDescription(Enum enumItem)
        {
            FieldInfo fi = enumItem.GetType().GetField(enumItem.ToString());

            DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(
                typeof(DescriptionAttribute), false);

            if (attributes != null && attributes.Length > 0)
            {
                return attributes[0].Description;
            }
            else
            {
                return enumItem.ToString();
            }
        }

        public static Dictionary<string, KeyValuePair<int, string>> GetEnumDescriptions<T>()
        {
            var type = typeof(T);
            var result = new Dictionary<string, KeyValuePair<int, string>>();
            if (!type.IsEnum)
            {
                return result;
            }
   
            var fieldArray = type.GetFields(BindingFlags.Public | BindingFlags.Static);
            foreach (var field in fieldArray)
            {
                var currentKey = field.Name;

                if (currentKey == "None") continue;

                var currentValue = (int)Enum.Parse(type, field.Name);
                var att = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute), false) as DescriptionAttribute;
                if (att != null)
                {
                    result.Add(currentKey, new KeyValuePair<int, string>(currentValue, att.Description));
                }
            }
            return result;
        }
    }
}