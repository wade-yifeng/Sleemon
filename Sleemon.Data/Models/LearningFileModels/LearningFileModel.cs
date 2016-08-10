namespace Sleemon.Data
{
    using System;

    public class LearningFileModel
    {
        public int Id { get; set; }

        public string Subject { get; set; }

        public string Description { get; set; }

        public string Content { get; set; }

        public byte FileType { get; set; }

        public int No { get; set; }

        public string LastUpdateUserName { get; set; }

        public string LastUpdateUser { get; set; }

        public DateTime LastUpdateTime { get; set; }
    }
}
