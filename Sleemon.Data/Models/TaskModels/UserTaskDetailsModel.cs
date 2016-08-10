namespace Sleemon.Data
{
    using System;

    using Newtonsoft.Json;

    public class UserTaskDetailsModel : TaskBasicModel
    {
        public DateTime AssignTime { get; set; }

        public double UserScore { get; set; }

        public int UserPoint { get; set; }

        public byte UserTaskStatus { get; set; }
    }
}