using System.Web.Mvc;

namespace Sleemon.WebApi.Core
{
    public class HandleErrorsAttribute : HandleErrorAttribute
    {
        public override void OnException(ExceptionContext filterContext)
        {
            if (filterContext.HttpContext.Request.Headers["X-Requested-With"] != "XMLHttpRequest")
            {
                return;
            }

            filterContext.ExceptionHandled = true;

            filterContext.Result = new JsonResult
            {
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                Data = new { message = "" }
            };
        }
    }
}