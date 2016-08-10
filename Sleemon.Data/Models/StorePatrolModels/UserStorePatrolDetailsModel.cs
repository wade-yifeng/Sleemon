namespace Sleemon.Data
{
    using System.Collections.Generic;

    public class UserStorePatrolDetailsModel : UserStorePatrolPreviewModel
    {
        public IList<UserStorePatrolModel> UserStorePatrols { get; set; }
    }
}
