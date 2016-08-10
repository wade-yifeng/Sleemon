using Microsoft.Practices.Unity;
using Sleemon.Portal.Common;
using Sleemon.Portal.Core;
using System.Web.Mvc;

namespace Sleemon.Portal.Controllers
{
    [SiteAuthorize]
    public class BaseController : Controller
    {
        [Dependency]
        protected ImplementServiceClient ServiceClient { get; set; }

        protected string UserUniqueId
        {
            get
            {
                return User.GetUserUniqueId();
            }
        }
    }
}