using System.Web.Mvc;
using Sleemon.WebApi.Core;

namespace Sleemon.WebApi
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorsAttribute());
        }
    }
}
