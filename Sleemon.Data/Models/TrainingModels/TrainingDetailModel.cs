namespace Sleemon.Data
{
    using System;
    using System.Collections.Generic;

    public class TrainingDetailModel : TrainingBasicModel
    {
        public List<TaskDetailsModel> Tasks { get; set; }
    }
}