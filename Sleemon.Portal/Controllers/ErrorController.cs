using System.Web.Mvc;

namespace Sleemon.Portal.Controllers
{
    public class ErrorController : Controller
    {
        [HttpGet]
        public ActionResult Index(int errorCode)
        {
            ViewBag.ErrorCode = errorCode;
            ViewBag.ErrorMessage = "系统发生异常，您的请求失败了";

            return this.View();
        }
    }
}