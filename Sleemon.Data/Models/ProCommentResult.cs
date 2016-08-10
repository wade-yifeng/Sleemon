namespace Sleemon.Data
{
    using Newtonsoft.Json;

    public class ProCommentResult : ResultBase
    {
        public int CommentId { get; set; }

        public string UserId { get; set; }

        public int Pros { get; set; }

        [JsonIgnore]
        public bool IsPro { get; set; }
    }
}
