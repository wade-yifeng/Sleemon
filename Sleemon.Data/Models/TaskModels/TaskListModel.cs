namespace Sleemon.Data
{
    public class TaskListModel : TaskBasicModel
    {
        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public int TotalCount { get; set; }
    }
}
