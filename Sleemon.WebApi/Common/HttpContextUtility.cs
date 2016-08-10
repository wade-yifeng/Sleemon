using System;
using System.Globalization;
using System.Web;

namespace Sleemon.WebApi.Common
{
    public static class HttpContextUtility
    {
        public static string CurrentController
        {
            get
            {
                if (CurrentContext == null || !CurrentContext.Request.RequestContext.RouteData.Values.ContainsKey("controller"))
                {
                    return "Home";
                }

                return CurrentContext.Request.RequestContext.RouteData.Values["controller"].ToString();
            }
        }

        public static string CurrentAction
        {
            get
            {
                if (CurrentContext == null || !CurrentContext.Request.RequestContext.RouteData.Values.ContainsKey("action"))
                {
                    return "Index";
                }

                return CurrentContext.Request.RequestContext.RouteData.Values["action"].ToString();
            }
        }

        private static HttpContext CurrentContext
        {
            get { return HttpContext.Current; }
        }

        public static T GetContextItem<T>(string key)
        {
            if (CurrentContext != null)
            {
                var value = CurrentContext.Items[key];
                if (value != null)
                {
                    return (T)Convert.ChangeType(value, typeof(T), CultureInfo.InvariantCulture);
                }
            }

            return default(T);
        }
    }
}