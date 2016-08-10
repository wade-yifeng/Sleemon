using System.Collections.Generic;
namespace Sleemon.Data
{
    public class UserStorePatrolDetailModel
    {
        public string UserName { get; set; }

        public short UserTaskStatus { get; set; }
        public IList<UserStorePatrolDetailPartialModel> PatialModel { get; set; }
    }
    public class UserStorePatrolDetailPartialModel
    {
        public string Description { get; set; }

        public string FilePath { get; set; }

        public double? AdminRate { get; set; }

        public string AdminComment { get; set; }

        public int StorePatrolId { get; set; }

        public int UserStorePatrolId { get; set; }

        public string ScenceName { get; set; }
    }
}
