using Sleemon.Data;
using Sleemon.Portal.Common;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Sleemon.Portal.Controllers
{
    /// <summary>
    /// 主页加载后需要进入ng的route
    /// 所以主页不需要权限限制
    /// </summary>
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            ViewBag.UserPermission = HttpContextUtility.GetContextItem<Dictionary<string, bool>>(SiteConfiguration.PremissionSessionKey) ?? new Dictionary<string, bool>();
            return View();
        }

        [HttpGet]
        public ActionResult Navigator()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Sidebar()
        {
            return View();
        }
    }
}