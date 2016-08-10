using System;
using System.Configuration;
using System.Linq;

namespace Sleemon.Portal.Common
{
    public static class SiteConfiguration
    {
        public static readonly string SiteDomain = ConfigurationManager.AppSettings["SITE_DOMAIN"];

        public static readonly int[] RootDepartments = ConfigurationManager.AppSettings["ROOT_DEPARTMENTS"].Split(',').Select(Int32.Parse).ToArray();

        public static readonly string PremissionSessionKey = ConfigurationManager.AppSettings["PERMISSION_SESSION_KEY"];
    }

    public static class Paths
    {
        public static string SignInPath = ConfigurationManager.AppSettings["SITE_SIGNIN_PATH"];
        public static string SignOutPath = ConfigurationManager.AppSettings["SITE_SIGNOUT_PATH"];
    }
}