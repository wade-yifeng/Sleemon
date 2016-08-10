namespace Sleemon.Data
{
    using System.Collections.Generic;

    public class UserStorePatrolListModel
    {
        public IList<UserStorePatrolPreviewModel> ListUserTask { get; set; }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public int TotalCount { get; set; }
    }
}
