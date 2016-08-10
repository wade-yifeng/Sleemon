using Sleemon.Portal.Core;
using System.Web.Mvc;

namespace Sleemon.Portal
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorsAttribute());
        }
    }
}