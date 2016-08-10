namespace Sleemon.Data
{
    using System;
    using System.Collections.Generic;

    public class GroupTaskDetailModel : GroupTaskBasicModel
    {
        public List<GroupSubTaskDetailModel> SubTasks { get; set; }
    }

    public class GroupSubTaskDetailModel : TaskDetailsModel
    {
        public int OffsetDays { get; set; }
    }
}
