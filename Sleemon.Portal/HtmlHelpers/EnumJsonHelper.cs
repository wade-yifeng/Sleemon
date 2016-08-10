using Sleemon.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace Sleemon.Portal.HtmlHelpers
{
    public static class EnumJsonHelper
    {
        public static HtmlString GetEnum<T>(this HtmlHelper helper) where T : struct
        {
            var descs = EnumHelper.GetEnumDescriptions<T>();
            var script = string.Format("Enum.{0} = {1};", typeof(T).Name, Json.Encode(descs));
            return new HtmlString(script);
        }
    }
}