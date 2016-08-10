namespace Sleemon.Data
{
    public class TaskItemSearchContext
    {
        public int TaskCategory { get; set; }

        public string Title { get; set; }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }
    }
}
