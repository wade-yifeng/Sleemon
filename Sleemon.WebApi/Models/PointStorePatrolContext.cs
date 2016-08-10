namespace Sleemon.WebApi
{
    using System.Collections.Generic;

    using Sleemon.Data;

    public class PointStorePatrolContext
    {
        public bool IsPass { get; set; }

        public IEnumerable<UserStorePatrolModel> UserStorePatrols { get; set; }
    }
}
