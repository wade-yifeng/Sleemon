namespace Sleemon.Data
{
    using Newtonsoft.Json;

    public class ProConNoticeResult : ResultBase
    {
        public int NoticeId { get; set; }

        public string UserId { get; set; }

        public int Pros { get; set; }

        public int Cons { get; set; }

        [JsonIgnore]
        public bool IsPro { get; set; }

        [JsonIgnore]
        public bool IsCon { get; set; }
    }
}
