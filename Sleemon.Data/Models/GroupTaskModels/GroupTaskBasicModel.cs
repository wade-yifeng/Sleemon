namespace Sleemon.Data
{
    using System;

    public class GroupTaskBasicModel
    {
        public int GroupTaskId { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public int RequiredGrade { get; set; }

        public bool OnOff { get; set; }

        public byte Status { get; set; }

        public string LastUpdateUser { get; set; }

        public string LastUpdateUserName { get; set; }

        public DateTime LastUpdateTime { get; set; }
    }
}
