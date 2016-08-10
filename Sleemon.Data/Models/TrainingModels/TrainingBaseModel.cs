namespace Sleemon.Data
{
    using System;

    public class TrainingBasicModel
    {
        public int TrainingId { get; set; }

        public string Subject { get; set; }

        public string Description { get; set; }

        public string Location { get; set; }

        public DateTime StartFrom { get; set; }

        public DateTime? EndTo { get; set; }

        public int MaxParticipant { get; set; }

        public DateTime JoinDeadline { get; set; }

        public bool IsPublic { get; set; }

        public bool IsCheckInNeeded { get; set; }

        public string CheckInQRCode { get; set; }

        public string Avatar { get; set; }

        public int Status { get; set; }

        public string LastUpdateUser { get; set; }

        public string LastUpdateUserName { get; set; }

        public DateTime LastUpdateTime { get; set; }
    }
}
