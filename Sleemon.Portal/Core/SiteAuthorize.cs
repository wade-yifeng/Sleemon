using Microsoft.Practices.Unity;
using Sleemon.Core;
using Sleemon.Data;
using Sleemon.Portal.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sleemon.Portal.Core
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public sealed class SiteAuthorize : AuthorizeAttribute
    {
        [Dependency]
        public ImplementServiceClient ServiceClient { get; set; }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (httpContext == null)
            {
                throw new ArgumentNullException("httpContext");
            }

            var userClaims = httpContext.User.Identity;
            if (!userClaims.IsAuthenticated)
            {
                return false;
            }
            
            // 获取当前用户的权限
            if (httpContext.Session[SiteConfiguration.PremissionSessionKey] == null)
            {
                var permissions = httpContext.User.Identity.AsClaimsIdentity().IsSuperAdmin() ?
                    this.ServiceClient.Request<IPermissionService, IList<Permission>>((service) => service.GetAllPermission()) :
                    this.ServiceClient.Request<IPermissionService, IList<Permission>>((service) => service.GetPermissionByUser(httpContext.User.GetUserUniqueId()));
                httpContext.Session[SiteConfiguration.PremissionSessionKey] = permissions.Where(p=> p.IsMenu && !string.IsNullOrWhiteSpace(p.Url)).ToDictionary(p => p.Url, p => p.IsMenu);
            }
            
            HttpContext.Current.Items[SiteConfiguration.PremissionSessionKey] = (Dictionary<string, bool>)httpContext.Session[SiteConfiguration.PremissionSessionKey];

            return true;
        }
    }
}