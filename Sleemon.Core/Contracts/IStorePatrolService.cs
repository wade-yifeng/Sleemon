namespace Sleemon.Core
{
    using System;
    using System.Collections.Generic;

    using Sleemon.Data;

    public interface IStorePatrolService
    {
        IList<int> GetTaskPatrolCategories(int taskId);

        UserStorePatrolDetailModel GetUserStorePatrolDetails(int userTaskId);

        IList<UserStorePatrolPreviewModel> GetStorePatrolList(int pageIndex, int pageSize, string userName, DateTime? startFrom, DateTime? endTo, out int totalCount);

        ResultBase UpLoadStorePatrol(string userUniqueId, IEnumerable<UserStorePatrolModel> userStorePatrols);

        ResultBase PointStorePatrol(bool isPass, IEnumerable<UserStorePatrolModel> userStorePatrols);

        //UserStorePatrolDetailsModel GetStorePatrolDetail(int taskId, string userUniqueId);

        IList<StorePatrolSceneModel> GetStorePatrolScenes();
    }
}
