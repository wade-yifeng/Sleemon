namespace Sleemon.Service
{
    using System;
    using System.Linq;
    using System.Text;
    using System.Data.Entity;
    using System.Data.SqlClient;
    using System.Collections.Generic;

    using Microsoft.Practices.Unity;

    using Sleemon.Core;
    using Sleemon.Data;
    using Sleemon.Common;

    public class TrainingService : ITrainingService
    {
        private readonly ISleemonEntities _invoicingEntities;

        public TrainingService()
        {
            this._invoicingEntities = new SleemonEntities();
        }

        public IList<UserTrainingPreviewModel> GetUserTrainingList(bool isAll, string userId, int pageIndex, int pageSize)
        {
            if (isAll)
            {
                //TODO: UserId
                var results = this._invoicingEntities.Training.Where(p => p.IsActive).ToList();

                return results.OrderBy(p => p.Status)
                        .ThenByDescending(p => p.LastUpdateTime)
                        .Skip((pageIndex - 1) * pageSize)
                        .Take(pageIndex * pageSize)
                        .Select(p => new UserTrainingPreviewModel()
                        {
                            TrainingId = p.Id,
                            Subject = p.Subject,
                            Avatar = p.Avatar,
                            StartFrom = p.StartFrom,
                            Status = p.Status
                        })
                        .ToList();
            }
            else
            {
                var results = this._invoicingEntities.UserTraining.Include(t => t.Training)
                        .Where(p => p.IsActive && p.Training.IsActive && p.UserUniqueId == userId)
                        .ToList();

                return results.OrderBy(p => p.Training.Status)
                        .ThenByDescending(p => p.Training.LastUpdateTime)
                        .Skip((pageIndex - 1) * pageSize)
                        .Take(pageIndex * pageSize)
                        .Select(p => new UserTrainingPreviewModel()
                        {
                            TrainingId = p.TrainingId,
                            Subject = p.Training.Subject,
                            Avatar = p.Training.Avatar,
                            StartFrom = p.Training.StartFrom,
                            Status = p.Training.Status,
                            JoinStatus = p.JoinStatus.GetValueOrDefault()
                        }).ToList();
            }
        }

        public UserTrainingDetailModel GetUserTrainingDetail(bool isAll, string userId, int trainingId)
        {
            #region 用户培训详情

            if (!isAll)
            {
                var result =
                    this._invoicingEntities
                        .UserTraining
                        .Include("Training.TrainingTask")
                        .FirstOrDefault(
                            p => p.IsActive && p.Training.IsActive && p.UserUniqueId == userId && p.TrainingId == trainingId);

                if (result == null) return null;

                var userTaskEntity =
                    this._invoicingEntities
                        .UserTask
                        .Include(p => p.Task)
                        .Where(p => p.Task.IsActive && p.IsActive && p.UserUniqueId == userId)
                        .ToList();

                var taskIds = result.Training.TrainingTask.Select(p => p.TaskId).ToList();

                return new UserTrainingDetailModel()
                {
                    Subject = result.Training.Subject,
                    JoinTime = result.JoinTime,
                    JoinStatus = result.JoinStatus,
                    Avatar = result.Training.Avatar,
                    Description = result.Training.Description,
                    Location = result.Training.Location,
                    StartFrom = result.Training.StartFrom,
                    EndTo = result.Training.EndTo,
                    MaxParticipant = result.Training.MaxParticipant,
                    JoinDeadline = result.Training.JoinDeadline,
                    IsCheckInNeeded = result.Training.IsCheckInNeeded,
                    IsCheckInDone = result.IsCheckInDone,
                    UserTasks = userTaskEntity.Where(p => taskIds.Any(o => o == p.TaskId)).Select(p => new UserTaskModel()
                    {
                        UserTaskId = p.Id,
                        Title = p.Task.Title,
                        TaskCategory = p.Task.TaskCategory,
                        UserTaskStatus = p.Status,
                    }).ToList()
                };
            }

            #endregion

            #region 培训详情

            else
            {
                var training =
                    this._invoicingEntities
                        .Training
                        .Include("TrainingTask.Task")
                        .FirstOrDefault(p => p.IsActive && p.Id == trainingId);

                if (training == null) return null;

                var userTraining =
                    this._invoicingEntities
                        .UserTraining
                        .Include("Training.TrainingTask")
                        .FirstOrDefault(
                            p => p.IsActive && p.Training.IsActive && p.UserUniqueId == userId && p.TrainingId == trainingId);

                IEnumerable<UserTask> userTaskEntity = null;
                IEnumerable<int> taskIds = null;
                if (userTraining != null)
                {
                    userTaskEntity = this._invoicingEntities
                        .UserTask
                        .Include(p => p.Task)
                        .Where(p => p.Task.IsActive && p.IsActive && p.UserUniqueId == userId)
                        .ToList();
                    taskIds = userTraining.Training.TrainingTask.Select(p => p.TaskId).ToList();
                }

                return new UserTrainingDetailModel()
                {
                    Subject = training.Subject,
                    JoinTime = null,
                    JoinStatus = userTraining != null ? userTraining.JoinStatus : null,
                    TrainingStatus = training.Status,
                    Avatar = training.Avatar,
                    Description = training.Description,
                    Location = training.Location,
                    StartFrom = training.StartFrom,
                    EndTo = training.EndTo,
                    MaxParticipant = training.MaxParticipant,
                    JoinDeadline = training.JoinDeadline,
                    IsCheckInNeeded = training.IsCheckInNeeded,
                    IsCheckInDone = false,
                    UserTasks = userTraining == null ?
                                training.TrainingTask.Select(p => new UserTaskModel()
                                {
                                    UserTaskId = p.Id,
                                    Title = p.Task.Title,
                                    TaskCategory = p.Task.TaskCategory,
                                    UserTaskStatus = 0,
                                }).ToList() :
                                userTaskEntity.Where(p => taskIds.Any(o => o == p.TaskId)).Select(p => new UserTaskModel()
                                {
                                    UserTaskId = p.Id,
                                    Title = p.Task.Title,
                                    TaskCategory = p.Task.TaskCategory,
                                    UserTaskStatus = p.Status,
                                }).ToList()
                };
            }

            #endregion
        }

        public IList<UserViewModel> GetTrainingParticipants(int trainingId)
        {
            return this._invoicingEntities.Database.SqlQuery<UserViewModel>(@"
SELECT [User].[Name]
	  ,[User].[Avatar]
FROM [dbo].[UserTraining]
JOIN [dbo].[User]
    ON [User].[UserUniqueId] = [UserTraining].[UserUniqueId]
JOIN [dbo].[Training]
    ON [Training].[Id] = [UserTraining].[TrainingId]
WHERE [UserTraining].[IsActive] = 1
    AND [Training].[IsActive] = 1
    AND [User].[IsActive] = 1
    AND [Training].[Id] = @trainingId", new SqlParameter("@trainingId", trainingId)).ToList();
        }

        public ResultBase JoinTraining(string userId, int trainingId)
        {
            var trainingEntity = this._invoicingEntities.Training.FirstOrDefault(p => p.IsActive && p.Id == trainingId);

            if (trainingEntity == null)
            {
                return new ResultBase()
                {
                    IsSuccess = false,
                    Message = string.Format("Cannot find training by id: {0}", trainingId),
                    StatusCode = (int)StatusCode.Failed
                };
            }

            var userTrainingEntities =
                this._invoicingEntities.UserTraining.Where(p => p.IsActive && p.TrainingId == trainingId).ToList();

            if (trainingEntity.MaxParticipant <= userTrainingEntities.Count)
            {
                return new ResultBase()
                {
                    IsSuccess = false,
                    Message = "人数超过上限",
                    StatusCode = (int)StatusCode.BeyondMaxParticipantLimit
                };
            }

            var userTrainingEntity = this._invoicingEntities.UserTraining.Create();

            userTrainingEntity.UserUniqueId = userId;
            userTrainingEntity.TrainingId = trainingId;
            userTrainingEntity.JoinTime = DateTime.UtcNow;
            userTrainingEntity.JoinStatus = (int)JoinStatus.Joining;
            userTrainingEntity.IsCheckInDone = false;
            userTrainingEntity.IsActive = true;

            this._invoicingEntities.UserTraining.Add(userTrainingEntity);

            this._invoicingEntities.SaveChanges();

            //TODO: 如果通过了，是否需要向UserTask表中加入UserTask记录
            //不需要，仅当管理员确认报名后，才添加UserTask记录

            return new ResultBase()
            {
                IsSuccess = true,
                StatusCode = (int)StatusCode.Success
            };
        }

        public IList<TrainingListModel> GetTrainingList(TrainingSearchContext search)
        {
            var userEntities = this._invoicingEntities.User.Where(p => p.IsActive).ToList();

            var results =
                this._invoicingEntities.Training.Where(search.GenerateSearchConditions())
                    .OrderBy(p => p.Status)
                    .ThenByDescending(p => p.LastUpdateTime)
                    .ThenBy(p => p.Id)
                    .Skip((search.PageIndex - 1) * search.PageSize)
                    .Take(search.PageIndex * search.PageSize)
                    .ToList();

            var totalCount = results.Count;

            return results.Select(p =>
            {
                var lastUpdateUser = userEntities.FirstOrDefault(t => t.UserUniqueId == p.LastUpdateUser);

                return new TrainingListModel()
                {
                    TrainingId = p.Id,
                    Subject = p.Subject,
                    Location = p.Location,
                    StartFrom = p.StartFrom,
                    EndTo = p.EndTo,
                    MaxParticipant = p.MaxParticipant,
                    IsPublic = p.IsPublic,
                    Status = p.Status,
                    LastUpdateUserName = lastUpdateUser == null ? string.Empty : lastUpdateUser.Name,
                    LastUpdateUser = p.LastUpdateUser,
                    LastUpdateTime = p.LastUpdateTime,
                    PageIndex = search.PageIndex,
                    PageSize = search.PageSize,
                    TotalCount = totalCount
                };
            }).ToList();
        }

        public ResultBase SaveTrainingDetail(TrainingDetailModel training)
        {
            var trainingEntity = Utilities.GetXElementFromObject(training).ToString();

            this._invoicingEntities.spSaveTrainingDetail(trainingEntity);

            return new ResultBase()
            {
                IsSuccess = true,
                StatusCode = (int)StatusCode.Success
            };
        }

        public ResultBase DeleteTrainingById(int trainingId)
        {
            var trainingEntity = this._invoicingEntities.Training.FirstOrDefault(p => p.IsActive && p.Id == trainingId);

            if (trainingEntity == null) return new ResultBase()
            {
                IsSuccess = false,
                Message = string.Format("Cannot find Training by id: {0}", trainingId),
                StatusCode = (int)StatusCode.Failed
            };

            trainingEntity.IsActive = false;
            trainingEntity.LastUpdateTime = DateTime.UtcNow;
            //TODO: LastUpdateUser

            this._invoicingEntities.SaveChanges();

            return new ResultBase()
            {
                IsSuccess = true,
                StatusCode = (int)StatusCode.Success
            };
        }

        public ResultBase UpdateTrainingUsersJoinState(int trainingId, IDictionary<JoinStatus, IList<string>> joinStatusUsers)
        {
            var sb = new StringBuilder();
            var userJoinStatusEntities = @"<UserJoinStatusEntities>{0}</UserJoinStatusEntities>";

            foreach (var joinStatus in joinStatusUsers)
            {
                foreach (var userUniqueId in joinStatus.Value)
                {
                    sb.AppendLine(string.Format(@"<UserJoinStatusEntity JoinStatus=""{0}"" UserUniqueId=""{1}""></UserJoinStatusEntity>", (int)joinStatus.Key, userUniqueId));
                }
            }

            //TODO: LastUpdateUser
            this._invoicingEntities.spUpdateTrainingUsersJoinState(trainingId, userJoinStatusEntities, string.Empty);

            return new ResultBase()
            {
                IsSuccess = true,
                StatusCode = (int)StatusCode.Success
            };
        }

        public TrainingDetailModel GetTrainingDetailById(int trainingId)
        {
            var trainingEntity = this._invoicingEntities.Training.FirstOrDefault(p => p.IsActive && p.Id == trainingId);

            if (trainingEntity == null) return null;

            var userEntities = this._invoicingEntities.User.Where(p => p.IsActive).ToList();
            var trainingTaskEntities =
                this._invoicingEntities
                    .TrainingTask
                    .Include(p => p.Task)
                    .Include(p => p.Task.StorePatrol)
                    .Include(p => p.Task.TaskExam)
                    .Include(p => p.Task.TaskQuestionnaire)
                    .Include(p => p.Task.TaskLearning)
                    .Where(p => p.TrainingId == trainingId && p.IsActive && p.Task.IsActive)
                    .ToList();

            var lastUpdateUser = userEntities.FirstOrDefault(p => p.UserUniqueId == trainingEntity.LastUpdateUser);
            var taskEntities = trainingTaskEntities.Where(p => p.Task.IsActive).Select(p => p.Task).ToList();

            var result = new TrainingDetailModel()
            {
                TrainingId = trainingEntity.Id,
                Subject = trainingEntity.Subject,
                Avatar = trainingEntity.Avatar,
                Description = trainingEntity.Description,
                Location = trainingEntity.Location,
                StartFrom = trainingEntity.StartFrom,
                EndTo = trainingEntity.EndTo,
                MaxParticipant = trainingEntity.MaxParticipant,
                JoinDeadline = trainingEntity.JoinDeadline,
                IsPublic = trainingEntity.IsPublic,
                IsCheckInNeeded = trainingEntity.IsCheckInNeeded,
                CheckInQRCode = trainingEntity.CheckInQRCode,
                Status = trainingEntity.Status,
                LastUpdateTime = trainingEntity.LastUpdateTime,
                LastUpdateUser = trainingEntity.LastUpdateUser,
                LastUpdateUserName = lastUpdateUser == null ? string.Empty : lastUpdateUser.Name
            };

            if (trainingTaskEntities.Any())
            {
                result.Tasks = taskEntities.Select(p =>
                {
                    var taskLastUpdateUser = userEntities.FirstOrDefault(o => o.UserUniqueId == p.LastUpdateUser);
                    return new TaskDetailsModel()
                    {
                        TaskId = p.Id,
                        Title = p.Title,
                        Description = p.Description,
                        TaskCategory = p.TaskCategory,
                        StartFrom = p.StartFrom,
                        EndTo = p.EndTo,
                        Point = p.Point,
                        OverduePoint = p.OverduePoint,
                        ProductAbility = p.ProductAbility,
                        SalesAbility = p.SalesAbility,
                        ExhibitAbility = p.ExhibitAbility,
                        BelongTo = p.BelongTo,
                        Status = p.Status,
                        LastUpdateUser = p.LastUpdateUser,
                        LastUpdateUserName = taskLastUpdateUser == null ? string.Empty : taskLastUpdateUser.Name,
                        LastUpdateTime = p.LastUpdateTime,
                        Exams = p.TaskExam.Where(o => o.IsActive && o.Exam.IsActive).Select(o => new ExamDetailModel()
                        {
                            Id = o.ExamId,
                            Title = o.Exam.Title
                        }).ToList(),
                        Questionnaires = p.TaskQuestionnaire.Where(o => o.IsActive && o.Questionnaire.IsActive).Select(o => new QuestionnaireDetailModel()
                        {
                            Id = o.QuestionnaireId,
                            Title = o.Questionnaire.Title
                        }).ToList(),
                        LearningFiles = p.TaskLearning.Where(o => o.IsActive && o.LearningFile.IsActive).Select(o => new LearningFileModel()
                        {
                            Id = o.LearningFileId,
                            Subject = o.LearningFile.Subject
                        }).ToList()
                    };
                }).ToList();
            }

            return result;
        }
    }
}
