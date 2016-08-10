namespace Sleemon.WebApi
{
    using System.Collections.Generic;

    using Sleemon.Data;

    public class UploadStorePatrolContext
    {
        public string UserUniqueId { get; set; }

        public IEnumerable<UserStorePatrolModel> UserStorePatrols { get; set; }
    }
}
