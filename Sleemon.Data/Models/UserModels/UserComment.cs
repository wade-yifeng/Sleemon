namespace Sleemon.Data
{
    using System;

    using Newtonsoft.Json;

    public class UserComment
    {
        public int Id { get; set; }

        public string Avatar { get; set; }

        public string Name { get; set; }

        public string Comment { get; set; }

        public byte Category { get; set; }

        public byte LegalStatus { get; set; }

        public int Pros { get; set; }

        public bool IsProByMe { get; set; }

        public DateTime CommentTime { get; set; }
    }
}
