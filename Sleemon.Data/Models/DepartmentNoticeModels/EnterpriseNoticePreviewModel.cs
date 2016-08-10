namespace Sleemon.Data
{
    using Newtonsoft.Json;

    public class EnterpriseNoticePreviewModel : EnterpriseNoticeBaseModel
    {
        public string Summary { get; set; }

        public byte Category { get; set; }

        public int CommentCount { get; set; }

        public System.DateTime LastUpdateTime { get; set; }
    }
}
