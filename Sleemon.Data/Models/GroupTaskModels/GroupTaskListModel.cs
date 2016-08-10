namespace Sleemon.Data
{
    public class GroupTaskListModel : GroupTaskBasicModel
    {
        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public int TotalCount { get; set; }
    }
}
