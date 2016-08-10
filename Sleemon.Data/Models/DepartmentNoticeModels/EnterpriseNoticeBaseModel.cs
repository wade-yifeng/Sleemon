namespace Sleemon.Data
{
    using Newtonsoft.Json;
    using System.Configuration;

    public class EnterpriseNoticeBaseModel
    {
        protected readonly string STATIC_RESOURCES_DOMAIN = ConfigurationManager.AppSettings["STATIC_RESOURCES_DOMAIN"];
        protected readonly string STATIC_RESOURCES_RELATIVE_PATH = ConfigurationManager.AppSettings["STATIC_RESOURCES_RELATIVE_PATH"];

        private string avatarPath;

        public int Id { get; set; }

        public string Subject { get; set; }

        public byte NoticeType { get; set; }

        [JsonProperty(PropertyName = "avatar")]
        public virtual string AvatarPath
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(avatarPath))
                {
                    avatarPath = STATIC_RESOURCES_DOMAIN + avatarPath;
                }
                return avatarPath;
            }
            set { avatarPath = value; }
        }
    }
}
