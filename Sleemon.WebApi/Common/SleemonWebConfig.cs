namespace Sleemon.WebApi
{
    using System.Configuration;

    public static class SleemonWebConfig
    {
        public static int CommandTimeout
        {
            get
            {
                return AppSettingsHelper.GetAppSetting("CommandTimeout", Constants.Default_Timeout_Minutes);
            }
        }

        public static string DbEfConnectionString
        {
            get
            {
                return ConfigurationManager.ConnectionStrings["SleemonEntities"].ConnectionString;
            }
        }

        public static int AuthorizationCookieExpiredInHours
        {
            get
            {
                return AppSettingsHelper.GetAppSetting<int>("AuthorizationCookieExpiredInHours", 720);
            }
        }
    }

    public static class Constants
    {
        #region Constants

        /// <summary>
        /// The default_ timeout_ minutes.
        /// </summary>
        public const int Default_Timeout_Minutes = 30;

        /// <summary>
        /// The default user's session timeout minutes.
        /// </summary>
        public const int UserSession_Timeout_Minutes = 30;

        public const string DateTimeFormat = "yyyy-MM-dd";

        public const int DefaultPageSize = 20;

        #endregion
    }
}
