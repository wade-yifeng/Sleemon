namespace Sleemon.Common
{
    using System;
    using System.Reflection;
    using System.ComponentModel;

    public static class EnumExtenstion
    {
        public static string GetDescription(this Enum @enum)
        {
            var descriptionAttribute = @enum.GetType().GetCustomAttribute<DescriptionAttribute>();
            return descriptionAttribute == null ? string.Empty : descriptionAttribute.Description;
        }
    }
}
