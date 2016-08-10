namespace Sleemon.Data
{
    using System;

    public class ExamBasicModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public double TotalScore { get; set; }

        public double PassingScore { get; set; }

        public string LastUpdateUserName { get; set; }

        public string LastUpdateUser { get; set; }

        public DateTime LastUpdateTime { get; set; }

        public byte Status { get; set; }
    }
}
