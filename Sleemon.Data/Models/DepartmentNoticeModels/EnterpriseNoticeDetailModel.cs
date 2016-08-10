namespace Sleemon.Data
{
    using System;

    using Newtonsoft.Json;

    public class EnterpriseNoticeDetailModel : EnterpriseNoticeBaseModel
    {
        private string context;

        public string Context
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(context))
                {
                    context = context.Replace(STATIC_RESOURCES_RELATIVE_PATH, STATIC_RESOURCES_DOMAIN + STATIC_RESOURCES_RELATIVE_PATH);
                }
                return context;
            }
            set { context = value; }
        }

        public byte Category { get; set; }

        public string Summary { get; set; }

        public int Pros { get; set; }

        public int Cons { get; set; }

        public DateTime LastUpdateTime { get; set; }
    }
}
