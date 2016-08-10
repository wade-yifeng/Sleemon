namespace Sleemon.Service
{
    using System;
    using System.Linq;
    using System.Data.Entity;
    using System.Data.SqlClient;
    using System.Collections.Generic;

    using Microsoft.Practices.Unity;

    using Sleemon.Core;
    using Sleemon.Data;
    using Sleemon.Common;

    public class StorePatrolService : IStorePatrolService
    {
        private readonly ISleemonEntities _invoicingEntities;

        public StorePatrolService()
        {
            this._invoicingEntities = new SleemonEntities();
        }

        public IList<int> GetTaskPatrolCategories(int taskId)
        {
            return
                this._invoicingEntities.StorePatrol.Where(p => p.IsActive && p.Task.IsActive && p.TaskId == taskId)
                    .Select(p => p.PatrolCategory)
                    .ToList();
        }

        public UserStorePatrolDetailModel GetUserStorePatrolDetails(int userTaskId)
        {
            UserStorePatrolDetailModel userStorePatrolModel = new UserStorePatrolDetailModel();
            IList<UserStorePatrolDetailPartialModel> partialModel = this._invoicingEntities.Database.SqlQuery<UserStorePatrolDetailPartialModel>(@"
SELECT [UserStorePatrol].[Id]  AS [UserStorePatrolId]
      ,[UserStorePatrol].[Description]
	  ,[UserStorePatrol].[FilePath]
	  ,[UserStorePatrol].[AdminRate]
	  ,[UserStorePatrol].[AdminComment]
	  ,[StorePatrol].[Id]             AS [StorePatrolId]
	  ,[SystemConfig].[Value]           AS [ScenceName]
FROM [dbo].[UserTask]
JOIN [dbo].[Task]
	ON [Task].[Id] = [UserTask].[TaskId]
JOIN [dbo].[StorePatrol]
	ON [StorePatrol].[TaskId] = [Task].[Id]
JOIN [dbo].[UserStorePatrol]
	ON [UserStorePatrol].[StorePatrolId] = [StorePatrol].[Id]
JOIN [dbo].[SystemConfig]
	ON [SystemConfig].[Seq] = [StorePatrol].[PatrolCategory]
WHERE [UserTask].[Id] = @userTaskId
	AND [Task].[IsActive] = 1
	AND [UserTask].[IsActive] = 1
	AND [StorePatrol].[IsActive] = 1
	AND [UserStorePatrol].[IsActive] = 1
	AND [SystemConfig].[IsActive] = 1
	AND [SystemConfig].[Type] = N'寻店'", new SqlParameter("@userTaskId", userTaskId)).ToList();
           UserTask userTask=this._invoicingEntities.UserTask.FirstOrDefault(p => p.Id == userTaskId);
            User user=this._invoicingEntities.User.FirstOrDefault(p=>p.UserUniqueId==userTask.UserUniqueId);
            userStorePatrolModel.UserName = user.Name;
            userStorePatrolModel.UserTaskStatus = userTask.Status;
            userStorePatrolModel.PatialModel = partialModel;
            return userStorePatrolModel;
            //TODO: [SystemConfig].[Type] = N'寻店'
        }

        public IList<UserStorePatrolPreviewModel> GetStorePatrolList(int pageIndex, int pageSize, string userName, DateTime? startFrom, DateTime? endTo, out int totalCount)
        {
            var result =
                this._invoicingEntities.spGetStorePatrolList(pageIndex, pageSize, userName, startFrom, endTo).ToList();

            var firstResult = result.FirstOrDefault();

            if (firstResult == null)
            {
                totalCount = 0;
                return null;
            }
            else
            {
                totalCount = firstResult.TotalCount.Value;

                return
                    result.Select(p => new UserStorePatrolPreviewModel()
                    {
                        TaskId = p.TaskId,
                        Title = p.Title,
                        Description = p.Description,
                        StartFrom = p.StartFrom,
                        EndTo = p.EndTo,
                        Point = p.Point,
                        OverduePoint = p.OverduePoint,
                        BelongTo = p.BelongTo,
                        IsStarted = DateTime.UtcNow > p.StartFrom,
                        IsPass = p.Status == 3,
                        UserName = p.UserName,
                        UserTaskId=p.UserTaskId
                    }).ToList();
            }
        }

        public ResultBase UpLoadStorePatrol(string userUniqueId, IEnumerable<UserStorePatrolModel> userStorePatrols)
        {
            //TODO: Add Transaction

            foreach (var userStorePatrolModel in userStorePatrols)
            {
                var oldUserStorePatrolEntity =
                    this._invoicingEntities.UserStorePatrol.FirstOrDefault(
                        p =>
                            p.IsActive && p.UserUniqueId == userUniqueId &&
                            p.StorePatrolId == userStorePatrolModel.StorePatrolId);

                if (oldUserStorePatrolEntity == null)
                {
                    var userStorePatrolEntity = this._invoicingEntities.UserStorePatrol.Create();

                    userStorePatrolEntity.UserUniqueId = userUniqueId;
                    userStorePatrolEntity.StorePatrolId = userStorePatrolModel.StorePatrolId;
                    userStorePatrolEntity.FilePath = userStorePatrolModel.PicPath;
                    userStorePatrolEntity.Description = userStorePatrolModel.Desc;
                    userStorePatrolEntity.LastUpdateTime = DateTime.UtcNow;

                    userStorePatrolEntity.IsActive = true;

                    this._invoicingEntities.UserStorePatrol.Add(userStorePatrolEntity);
                }
                else if (!oldUserStorePatrolEntity.AdminRate.HasValue)
                {
                    oldUserStorePatrolEntity.FilePath = userStorePatrolModel.PicPath;
                    oldUserStorePatrolEntity.Description = userStorePatrolModel.Desc;
                    oldUserStorePatrolEntity.LastUpdateTime = DateTime.UtcNow;
                }
                else
                {
                    //TODO: 如果寻店不通过是可以重新上传的
                    return new ResultBase()
                    {
                        IsSuccess = false,
                        Message = "已经打过分的寻店任务，不允许修改。",
                        StatusCode = (int)StatusCode.DuplicateUploadStorePatrol
                    };
                }
            }

            this._invoicingEntities.SaveChanges();

            return new ResultBase()
            {
                IsSuccess = true,
                StatusCode = (int)StatusCode.Success
            };
        }

        public ResultBase PointStorePatrol(bool isPass, IEnumerable<UserStorePatrolModel> userStorePatrols)
        {
            this._invoicingEntities.spPointStorePatrol(isPass,
                Utilities.GetXElementFromObject(userStorePatrols.ToList()).ToString());

            return new ResultBase()
            {
                IsSuccess = true,
                StatusCode = (int)StatusCode.Success
            };
        }

        //public UserStorePatrolDetailsModel GetStorePatrolDetail(int taskId, string userUniqueId)
        //{
        //    //TODO: Type == "寻店"??
        //    var storePatrolCategories =
        //        this._invoicingEntities.SystemConfig.Where(p => p.IsActive && p.Type == "寻店").ToList();
        //    var taskEntity = this._invoicingEntities.Task.FirstOrDefault(p => p.IsActive && p.Id == taskId);

        //    if (taskEntity == null) return null;

        //    var userTaskEntity =
        //        this._invoicingEntities.UserTask.FirstOrDefault(
        //            p => p.IsActive && p.TaskId == taskId && p.UserUniqueId == userUniqueId);

        //    var result = new UserStorePatrolDetailsModel()
        //    {
        //        TaskId = taskEntity.Id,
        //        Title = taskEntity.Title,
        //        Description = taskEntity.Description,
        //        TaskCategory = taskEntity.TaskCategory,
        //        StartFrom = taskEntity.StartFrom,
        //        EndTo = taskEntity.EndTo,
        //        Point = taskEntity.Point,
        //        OverduePoint = taskEntity.OverduePoint,
        //        BelongTo = taskEntity.BelongTo,
        //        IsStarted = DateTime.UtcNow > taskEntity.StartFrom
        //    };

        //    if (userTaskEntity != null)
        //    {
        //        result.IsPass = userTaskEntity.Status == 3;
        //    }

        //    var userStorePatrolEntities =
        //        this._invoicingEntities.UserStorePatrol
        //        .Include(p => p.StorePatrol)
        //        .Where(
        //            p =>
        //                p.UserUniqueId == userUniqueId && p.IsActive && p.StorePatrol.IsActive &&
        //                p.StorePatrol.TaskId == taskId);

        //    result.UserStorePatrols = userStorePatrolEntities.AsEnumerable().Select(p =>
        //    {
        //        var scene = storePatrolCategories.FirstOrDefault(o => o.Seq == p.StorePatrol.PatrolCategory);

        //        return new UserStorePatrolModel()
        //        {
        //            UserStorePatrolId = p.Id,
        //            StorePatrolId = p.StorePatrolId,
        //            SceneName = scene != null ? scene.Value : string.Empty,
        //            PicPath = p.FilePath,
        //            Desc = p.Description,
        //            AdminComment = p.AdminComment,
        //            AdminRate = p.AdminRate ?? 0
        //        };
        //    }).ToList();

        //    return result;
        //}

        public IList<StorePatrolSceneModel> GetStorePatrolScenes()
        {
          //  var sceneName = SystemConfigKeys.StorePatrol.GetDescription();
            var sceneName =EnumHelper.GetEnumDescription(SystemConfigKeys.StorePatrol);

            return
                this._invoicingEntities.SystemConfig.Where(p => p.IsActive && p.Type == sceneName)
                    .Select(p => new StorePatrolSceneModel()
                    {
                        SceneId = p.Id,
                        SceneName = p.Value
                    })
                    .ToList();
        }
    }
}
