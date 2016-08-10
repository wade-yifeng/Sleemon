namespace Sleemon.Data
{
    using System.Collections.Generic;

    public class StorePatrolListModel
    {
        public IList<TaskDetailsModel> ListTask { get; set; }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public int TotalCount { get; set; }
    }
}