namespace Sleemon.Data
{
    using System;
    public class UserTaskModel
    {
        public int UserTaskId { get; set; }

        public string Title { get; set; }

        public int Type { get; set; }

        public byte TaskCategory { get; set; }

        public DateTime StartFrom { get; set; }

        public DateTime? EndTo { get; set; }

        public int Point { get; set; }
        
        public byte UserTaskStatus { get; set; }

        public int? GroupId { get; set; }

        public string GroupTitle { get; set; }

        public int? RequiredGrade { get; set; }
    }
}