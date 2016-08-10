using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using Microsoft.Practices.Unity;
using Sleemon.Common;
using Sleemon.Core;
using Sleemon.Data;
using Sleemon.Portal.Common;
using Sleemon.Portal.Core;
using System.Collections.Generic;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;

namespace Sleemon.Portal.Controllers
{
    public class AccountController : Controller
    {
        [Dependency]
        public ImplementServiceClient ServiceClient { get; set; }

        public AccountController()
            : base() {}

        [HttpGet]
        public ActionResult Login(string returnUrl)
        {
            var model = new Models.User { RedirectUrl = returnUrl };
            // 使用Owin传入的ReturnUrl
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(Models.User user)
        {
            var userModel = this.ServiceClient.Request<IUserService, Data.User>((service) => service.GetUserById(user.UserUniqueId));

            var errorMessage = string.Empty;
            if (userModel == null || !EncryptUtility.MD5(user.Password).Equals(userModel.Password) || !userModel.IsAdmin)
            {
                return new JsonContentAction(new { errorMessage = "用户名或密码不正确" });
            }
            if (userModel.IsOriginalPassword)
            {
                return new JsonContentAction(new { reset = true });
            }

            var userIdentity = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, userModel.Name),
                new Claim(ClaimTypes.NameIdentifier, userModel.UserUniqueId),
                new Claim(ClaimsIdentityExtensions.AvatarClaim, userModel.Avatar),
                new Claim(ClaimsIdentityExtensions.IsSuperAdminClaim, userModel.IsSuperAdmin.ToString())
            }, DefaultAuthenticationTypes.ApplicationCookie);

            var authenticationManager = HttpContext.GetOwinContext().Authentication;
            authenticationManager.SignIn(new AuthenticationProperties { IsPersistent = user.IsAutoLogin }, userIdentity);

            return new JsonContentAction(new { success = true, redirectUrl = user.RedirectUrl.ToString() });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult ResetPassword(Models.User user)
        {
            var userUniqueId = User.GetUserUniqueId();
            if (user.Password.Equals(userUniqueId))
            {
                return new JsonContentAction(new { errorMessage = "无效的密码（用户名和密码不能一致）" });
            }

            if (!this.ServiceClient.Request<IUserService, bool>((service) => service.ResetPassword(userUniqueId, EncryptUtility.MD5(user.Password))))
            {
                return new JsonContentAction(new { errorMessage = "修改密码失败" });
            }

            return new JsonContentAction(new { success = true });
        }
    }
}