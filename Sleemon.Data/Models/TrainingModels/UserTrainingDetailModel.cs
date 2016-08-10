namespace Sleemon.Data
{
    using System;
    using System.Collections.Generic;

    public class UserTrainingDetailModel
    {
        public string Subject { get; set; }

        public DateTime? JoinTime { get; set; }

        public int? JoinStatus { get; set; }

        public int? TrainingStatus { get; set; }

        public string Avatar { get; set; }

        public string Description { get; set; }

        public string Location { get; set; }

        public DateTime StartFrom { get; set; }

        public DateTime? EndTo { get; set; }

        public int MaxParticipant { get; set; }

        public DateTime JoinDeadline { get; set; }

        public bool IsCheckInNeeded { get; set; }

        public bool? IsCheckInDone { get; set; }

        public IList<UserTaskModel> UserTasks { get; set; }
    }
}
