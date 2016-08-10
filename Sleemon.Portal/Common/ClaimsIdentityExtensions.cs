using System;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;

namespace Sleemon.Portal.Common
{
    public static class ClaimsIdentityExtensions
    {
        public const string AvatarClaim = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/avatar";
        public const string IsSuperAdminClaim = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/IsSuperAdmin";

        public static ClaimsIdentity AsClaimsIdentity(this IIdentity identity)
        {
            return identity as ClaimsIdentity;
        }

        public static string GetUserUniqueId(this IPrincipal user)
        {
            var userUniqueId = string.Empty;
            if (user != null)
            {
                var identity = user.Identity.AsClaimsIdentity();
                if (identity != null)
                {
                    var claim =
                        identity.Claims.SingleOrDefault(
                            item => item.Type.Equals(ClaimTypes.NameIdentifier, StringComparison.OrdinalIgnoreCase));

                    userUniqueId = claim == null ? string.Empty : claim.Value;
                }
            }

            return userUniqueId;
        }

        public static string GetAvatar(this ClaimsIdentity identity)
        {
            var claim = identity.Claims.SingleOrDefault(item => item.Type.Equals(AvatarClaim, StringComparison.OrdinalIgnoreCase));
            return claim == null ? null : claim.Value;
        }

        public static bool IsSuperAdmin(this ClaimsIdentity identity)
        {
            var claim = identity.Claims.SingleOrDefault(item => item.Type.Equals(IsSuperAdminClaim, StringComparison.OrdinalIgnoreCase));
            return claim == null ? false : claim.Value == Boolean.TrueString;
        }
    }
}
