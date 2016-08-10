namespace Sleemon.Service
{
    using System;
    using System.Linq;
    using System.Data.Entity;
    using System.Data.SqlClient;
    using System.Collections.Generic;

    using Sleemon.Core;
    using Sleemon.Data;
    using Sleemon.Common;
    using System.Data;
    public class TaskService : ITaskService
    {
        private readonly ISleemonEntities _invoicingEntities;

        public TaskService()
        {
            this._invoicingEntities = new SleemonEntities();
        }

        public IList<UserTaskModel> SearchUserTaskList(string userUnqiueId, byte listType, string input)
        {
            return
                this._invoicingEntities.Database.SqlQuery<UserTaskModel>(@"
SELECT [UserTask].[Id]      AS [UserTaskId]
        ,[Task].[BelongTo]     AS [Type]
        ,[Task].[Title]       AS [Title]
        ,[Task].[StartFrom]   AS [StartFrom]
        ,[Task].[EndTo]        AS [EndTo]
        ,[Task].[Point]   AS [Point]
        ,[Task].[TaskCategory] AS [TaskCategory]
        ,[UserTask].[Status]  AS [UserTaskStatus]
        ,[StatusOrder].[Order]
FROM [dbo].[UserTask]
JOIN [dbo].[Task]
    ON [Task].[Id] = [UserTask].[TaskId]
JOIN (VALUES(2, 1), (5, 2), (1, 3), (3, 4), (4, 5)) [StatusOrder]([Status], [Order])
    ON [StatusOrder].[Status] = [UserTask].[Status]
WHERE [UserTask].[IsActive] = 1
    AND [Task].[IsActive] = 1
    AND [UserTask].[UserUniqueId] = @userUnqiueId
    AND (CASE WHEN ([UserTask].[Status] = 1 AND ISNULL([Task].[EndTo], '2099-09-09') >= GETDATE()) OR [UserTask].[Status] IN (2,3, 5) THEN 1
		          WHEN ([UserTask].[Status] = 1 AND ISNULL([Task].[EndTo], '2099-09-09') < GETDATE()) THEN 2
		          WHEN ([UserTask].[Status] = 4) THEN 3
		          ELSE 0 END) = @listType
    AND [Task].[BelongTo] = @belongTo
    AND [Task].[Title] LIKE @input
ORDER BY [StatusOrder].[Order], [Task].[StartFrom] DESC, [UserTask].[Id]",
                new SqlParameter("@userUnqiueId", userUnqiueId),
                new SqlParameter("@listType", listType),
                new SqlParameter("@input", string.Format("%{0}%", input)),
                new SqlParameter("@belongTo", (int)TaskBelongTo.SingleTask)).ToList();
        }

        public IEnumerable<UserTaskInfo> GetUserTaskList(string userUnqiueId, byte listType, int pageIndex, int pageSize)
        {
            var taskMap = this._invoicingEntities.Database.SqlQuery<UserTaskModel>(@"
WITH [PagedUserTask] AS
(
    SELECT [UserTask].[Id]       AS [UserTaskId]
          ,[Task].[BelongTo]     AS [Type]
          ,[Task].[Title]        AS [Title]
          ,[Task].[StartFrom]    AS [StartFrom]
		  ,[Task].[EndTo]        AS [EndTo]
          ,[Task].[Point]        AS [Point]
		  ,[Task].[TaskCategory] AS [TaskCategory]
          ,[UserTask].[Status]   AS [UserTaskStatus]
          ,[StatusOrder].[Order]
          ,ROW_NUMBER() OVER(ORDER BY [StatusOrder].[Order], [Task].[StartFrom] DESC, [UserTask].[Id]) AS [Row]
    FROM [dbo].[UserTask]
    JOIN [dbo].[Task]
        ON [Task].[Id] = [UserTask].[TaskId]
    JOIN (VALUES(2, 1), (5, 2), (1, 3), (3, 4), (4, 5)) [StatusOrder]([Status], [Order])
        ON [StatusOrder].[Status] = [UserTask].[Status]
    WHERE [UserTask].[IsActive] = 1
        AND [Task].[IsActive] = 1
        AND [UserTask].[UserUniqueId] = @userUnqiueId
        AND (CASE WHEN ([UserTask].[Status] = 1 AND ISNULL([Task].[EndTo], '2099-09-09') >= GETDATE()) OR [UserTask].[Status] IN (2,3, 5) THEN 1
		          WHEN ([UserTask].[Status] = 1 AND ISNULL([Task].[EndTo], '2099-09-09') < GETDATE()) THEN 2
		          WHEN ([UserTask].[Status] = 4) THEN 3
		          ELSE 0 END) = @listType
        AND [Task].[BelongTo] IN (1,3)
)
SELECT [PagedUserTask].[UserTaskId]
      ,[PagedUserTask].[Type]
      ,[PagedUserTask].[Title]
      ,[PagedUserTask].[StartFrom]
      ,[PagedUserTask].[EndTo]
      ,ISNULL([PagedUserTask].[Point], 0) AS [Point]
      ,[PagedUserTask].[UserTaskStatus]
	  ,[PagedUserTask].[TaskCategory]
      ,[GroupTask].[Id] AS GroupId
	  ,[GroupTask].[Title] AS GroupTitle
	  ,[GroupTask].[RequiredGrade]
FROM [PagedUserTask] LEFT JOIN [GroupSubTask]
	ON [GroupSubTask].TaskId = [PagedUserTask].[UserTaskId]
	LEFT JOIN [GroupTask] ON [GroupTask].Id = [GroupSubTask].[GroupTaskId]
WHERE ([GroupTask].[Status] IS NULL OR [GroupTask].[Status] = 1) AND [PagedUserTask].[Row] BETWEEN (@pageIndex - 1) * @pageSize + 1 AND @pageIndex * @pageSize",
                new SqlParameter("@userUnqiueId", userUnqiueId),
                new SqlParameter("@listType", listType),
                new SqlParameter("@pageIndex", pageIndex),
                new SqlParameter("@pageSize", pageSize)).ToLookup(t => t.GroupId == null ? t.UserTaskId : t.GroupId.Value);

            return from IGrouping<int, UserTaskModel> columnMap in taskMap
                   let firstTask = columnMap.First()
                   let type = (TaskBelongTo)firstTask.Type
                   select
                       new UserTaskInfo
                       {
                           Type = type,
                           GroupId = type == TaskBelongTo.GroupTask ? firstTask.GroupId.Value : 0,
                           RequiredGrade = type == TaskBelongTo.GroupTask ? firstTask.RequiredGrade.Value : 0,
                           Title = type == TaskBelongTo.GroupTask ? firstTask.GroupTitle : firstTask.Title,
                           SubTaskList = columnMap.Select(
                               t => new UserTaskModel
                               {
                                   Title = t.Title,
                                   TaskCategory = t.TaskCategory,
                                   Point = t.Point,
                                   StartFrom = t.StartFrom,
                                   EndTo = t.EndTo,
                                   UserTaskStatus = t.UserTaskStatus,
                                   Type = t.Type,
                                   UserTaskId = t.UserTaskId,
                                   GroupId = t.GroupId,
                                   GroupTitle = t.GroupTitle,
                                   RequiredGrade = t.RequiredGrade
                               }).ToList()
                       };
        }

        public IList<TaskListModel> GetTaskList(TaskSearchContext search)
        {
            var entities = this._invoicingEntities.Task.Where(search.GenerateSearchConditions()).OrderBy(p => p.Status).ThenByDescending(p => p.LastUpdateTime);
            var userEntities = this._invoicingEntities.User.Where(p => p.IsActive).ToList();

            var totalCount = entities.Count();

            return
                entities.Skip((search.PageIndex - 1) * search.PageSize)
                    .Take(search.PageSize)
                    .ToList()
                    .Select(p =>
                    {
                        var lastUpdateUserName = string.Empty;
                        var firstOrDefault = userEntities.FirstOrDefault(o => o.UserUniqueId == p.LastUpdateUser);
                        if (firstOrDefault != null)
                        {
                            lastUpdateUserName = firstOrDefault.Name;
                        }

                        return new TaskListModel()
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
                            LastUpdateUserName = lastUpdateUserName,
                            LastUpdateTime = p.LastUpdateTime,
                            PageIndex = search.PageIndex,
                            PageSize = search.PageSize,
                            TotalCount = totalCount
                        };
                    })
                    .ToList();
        }

        public IList<TaskListModel> GetTaskList(int belongTo)
        {
            var entities =
                this._invoicingEntities.Task.Where(p => p.BelongTo == belongTo && p.IsActive)
                    .OrderBy(p => p.Status)
                    .ThenByDescending(p => p.LastUpdateTime)
                    .ToList();
            var userEntities = this._invoicingEntities.User.Where(p => p.IsActive).ToList();

            return
                entities
                    .Select(p =>
                    {
                        var lastUpdateUserName = string.Empty;
                        var firstOrDefault = userEntities.FirstOrDefault(o => o.UserUniqueId == p.LastUpdateUser);
                        if (firstOrDefault != null)
                        {
                            lastUpdateUserName = firstOrDefault.Name;
                        }

                        return new TaskListModel()
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
                            LastUpdateUserName = lastUpdateUserName,
                            LastUpdateTime = p.LastUpdateTime
                        };
                    })
                    .ToList();
        }

        public TaskDetailsModel GetTaskDetailById(int taskId)
        {
            var taskEntity =
                this._invoicingEntities.Task.Include(p => p.StorePatrol)
                    .Include(p => p.TaskExam)
                    .Include(p => p.TaskQuestionnaire)
                    .Include(p => p.TaskLearning)
                    .FirstOrDefault(p => p.IsActive && p.Id == taskId);

            if (taskEntity == null) return null;

            var lastUpdateUser = this._invoicingEntities.User.FirstOrDefault(p => p.IsActive && p.UserUniqueId == taskEntity.LastUpdateUser);
            var userIds =
                this._invoicingEntities.UserTask.Where(p => p.IsActive && p.TaskId == taskId).Select(p => p.UserUniqueId).ToList();

            return new TaskDetailsModel()
            {
                Title = taskEntity.Title,
                Description = taskEntity.Description,
                TaskCategory = taskEntity.TaskCategory,
                StartFrom = taskEntity.StartFrom,
                EndTo = taskEntity.EndTo,
                Point = taskEntity.Point,
                OverduePoint = taskEntity.OverduePoint,
                ProductAbility = taskEntity.ProductAbility,
                SalesAbility = taskEntity.SalesAbility,
                ExhibitAbility = taskEntity.ExhibitAbility,
                BelongTo = taskEntity.BelongTo,
                Status = taskEntity.Status,
                LastUpdateUser = lastUpdateUser == null ? string.Empty : lastUpdateUser.Name,
                LastUpdateTime = taskEntity.LastUpdateTime,
                UserIds = userIds,
                Exams = taskEntity.TaskExam.Where(o => o.IsActive).Select(o => new ExamDetailModel()
                {
                    Id = o.ExamId
                }).ToList(),
                Questionnaires = taskEntity.TaskQuestionnaire.Where(o => o.IsActive).Select(o => new QuestionnaireDetailModel()
                {
                    Id = o.QuestionnaireId
                }).ToList(),
                LearningFiles = taskEntity.TaskLearning.Where(o => o.IsActive).Select(o => new LearningFileModel()
                {
                    Id = o.LearningFileId
                }).ToList()
            };
        }

        public UserTaskDetailsModel GetUserTaskDetail(int userTaskId)
        {
            return this._invoicingEntities.Database.SqlQuery<UserTaskDetailsModel>(@"
SELECT [Task].[Id] AS [TaskId]
      ,[Task].[Title]
      ,[Task].[Description]
      ,[Task].[TaskCategory]
      ,[Task].[StartFrom]
      ,[Task].[EndTo]
      ,[Task].[Point]
      ,[Task].[OverduePoint]
      ,[Task].[ProductAbility]
      ,[Task].[SalesAbility]
      ,[Task].[ExhibitAbility]
      ,[Task].[BelongTo]
      ,[Task].[Status] AS [TaskStatus]
      ,[UserTask].[AssignTime]
      ,ISNULL([UserTask].[Score], 0) AS [UserScore]
      ,ISNULL([UserTask].[Point], 0) AS [UserPoint]
      ,[UserTask].[Status] AS [UserTaskStatus]
FROM [dbo].[UserTask]
JOIN [dbo].[Task]
    ON [Task].[Id] = [UserTask].[TaskId]
WHERE [UserTask].[Id] = @userTaskId
    AND [UserTask].[IsActive] = 1
    AND [Task].[IsActive] = 1", new SqlParameter("@userTaskId", userTaskId)).FirstOrDefault();
        }

        public ExamPreviewModel GetExamTaskOtherInfo(int taskId)
        {
            //TODO: resitCount
            return this._invoicingEntities.Database.SqlQuery<ExamPreviewModel>(@"
SELECT [GroupExamQuestion].[QuestionCount]
      ,[Exam].[TotalScore]
      ,[Exam].[PassingScore]
      ,0                            AS [ResitCount]
FROM [dbo].[TaskExam]
JOIN [dbo].[Exam]
    ON [Exam].[Id] = [TaskExam].[ExamId]
JOIN (SELECT [ExamQuestion].[ExamId], COUNT(*) AS [QuestionCount] FROM [dbo].[ExamQuestion] WHERE [ExamQuestion].[IsActive] = 1 GROUP BY [ExamQuestion].[ExamId]) AS [GroupExamQuestion]
    ON [TaskExam].[ExamId] = [GroupExamQuestion].[ExamId]
WHERE [TaskExam].[IsActive] = 1
    AND [TaskExam].[TaskId] = @taskId", new SqlParameter("@taskId", taskId)).FirstOrDefault();
        }

        public IList<ExamQuestionModel> GetExamQuestions(int taskId)
        {
            var taskExamEntity = this._invoicingEntities.TaskExam.FirstOrDefault(p => p.IsActive && p.TaskId == taskId);

            if (taskExamEntity == null) return null;

            return
                this._invoicingEntities.ExamQuestion.Include(p => p.ExamChoice).Where(p => p.ExamId == taskExamEntity.ExamId && p.IsActive)
                    .Select(p => new ExamQuestionModel()
                    {
                        ExamQuestionId = p.Id,
                        No = p.No,
                        Question = p.Question,
                        Image = p.Image,
                        Category = p.Category,
                        CorrectAnswer = p.CorrectAnswer,
                        Score = p.Score,
                        Choices =
                            p.ExamChoice.Where(o => o.IsActive)
                                .Select(o => new ExamChoiceModel()
                                {
                                    Choice = o.Choice,
                                    Description = o.Description,
                                    Image = o.Image,
                                    IsAnswer = o.IsAnswer
                                }).ToList()
                    })
                    .OrderBy(p => p.ExamQuestionId)
                    .ToList();
        }

        public IList<ExamAnswerModel> GetUserExamAnswers(int taskId, string userUniqueId)
        {
            return
                this._invoicingEntities.UserExamAnswer.Where(p => p.IsActive && p.UserUniqueId == userUniqueId && p.TaskId == taskId)
                    .Select(p => new ExamAnswerModel()
                    {
                        ExamQuestionId = p.ExamQuestionId,
                        MyAnswer = p.MyAnswer
                    })
                    .OrderBy(p => p.ExamQuestionId)
                    .ToList();
        }

        public ResultBase CommitSingleExamQuestion(int taskId, string userUniqueId, int examQuestionId, string myAnswer)
        {
            var myAnswerEntity =
                this._invoicingEntities.UserExamAnswer.FirstOrDefault(
                    p => p.UserUniqueId == userUniqueId && p.IsActive && p.ExamQuestionId == examQuestionId && p.TaskId == taskId);

            //TODO: Refactor this logic
            if (myAnswerEntity == null && !string.IsNullOrEmpty(myAnswer))
            {
                return this.CreateSingleExamQuestion(taskId, userUniqueId, examQuestionId, myAnswer);
            }

            if (myAnswerEntity != null && string.IsNullOrEmpty(myAnswer))
            {
                this._invoicingEntities.UserExamAnswer.Remove(myAnswerEntity);
            }
            else if (myAnswerEntity != null)
            {
                myAnswerEntity.MyAnswer = myAnswer;
                myAnswerEntity.LastUpdateTime = DateTime.UtcNow;
            }
            else
            {
                //TODO: 
            }

            this._invoicingEntities.SaveChanges();

            return new ResultBase()
            {
                IsSuccess = true,
                StatusCode = (int)StatusCode.Success
            };
        }

        public ExamResultModel CommitEntireExam(string userUniqueId, int userTaskId)
        {
            return
                this._invoicingEntities.spCommitEntireExam(userTaskId, userUniqueId)
                    .Select(p => new ExamResultModel()
                    {
                        UserScore = p.UserScore ?? 0,
                        Point = p.Point ?? 0,
                        ProductAbility = p.ProductAbility,
                        SalesAbility = p.SalesAbility,
                        ExhibitAbility = p.ExhibitAbility
                    })
                    .FirstOrDefault();
        }

        public ResultBase SaveTaskDetail(TaskDetailsModel taskModel)
        {
            var taskId = taskModel.TaskId;
            var taskEntity = this._invoicingEntities.Task.FirstOrDefault(p => p.IsActive && p.Id == taskModel.TaskId);

            if (taskEntity == null)
            {
                taskId = this.CreateTask(taskModel);
            }
            else
            {
                taskEntity.Title = taskModel.Title;
                taskEntity.Description = taskModel.Description;
                taskEntity.TaskCategory = taskModel.TaskCategory;
                taskEntity.StartFrom = taskModel.StartFrom;
                taskEntity.EndTo = taskModel.EndTo;
                taskEntity.Point = taskModel.Point;
                taskEntity.OverduePoint = taskModel.OverduePoint;
                taskEntity.ProductAbility = taskModel.ProductAbility;
                taskEntity.SalesAbility = taskModel.SalesAbility;
                taskEntity.ExhibitAbility = taskModel.ExhibitAbility;
                taskEntity.BelongTo = taskModel.BelongTo;
                taskEntity.Status = taskModel.Status;
                taskEntity.LastUpdateUser = taskModel.LastUpdateUser;//TODO: Last update user unique id
                taskEntity.LastUpdateTime = DateTime.UtcNow;

                this._invoicingEntities.SaveChanges();
            }

            if (taskModel.Status == 2)
            {
                if (taskModel.UserIds != null)
                {
                    foreach (var userId in taskModel.UserIds)
                    {
                        var userTaskEntity = this._invoicingEntities.UserTask.Create();

                        userTaskEntity.UserUniqueId = userId;
                        userTaskEntity.TaskId = taskId;
                        userTaskEntity.AssignTime = DateTime.UtcNow;
                        userTaskEntity.Status = 0;
                        userTaskEntity.IsActive = true;

                        this._invoicingEntities.UserTask.Add(userTaskEntity);
                    }

                    this._invoicingEntities.SaveChanges();

                    var messageDispatchEntity = this._invoicingEntities.MessageDispatch.Create();

                    messageDispatchEntity.Subject = taskModel.DispatchSubject;
                    messageDispatchEntity.Priority = taskModel.DispatchPriority;
                    messageDispatchEntity.ToUsers = string.Join("|", taskModel.UserIds);
                    messageDispatchEntity.MessageType = taskModel.TaskCategory;
                    messageDispatchEntity.LinkedId = taskId;
                    messageDispatchEntity.DispatchType = taskModel.DispatchType;
                    messageDispatchEntity.DispatchTime = taskModel.DispatchTime;

                    messageDispatchEntity.Status = 1;
                    messageDispatchEntity.LastUpdateUser = taskModel.LastUpdateUser;//TODO: Last update user unique id
                    messageDispatchEntity.LastUpdateTime = DateTime.UtcNow;
                    messageDispatchEntity.IsActive = true;

                    this._invoicingEntities.MessageDispatch.Add(messageDispatchEntity);
                    this._invoicingEntities.SaveChanges();
                }

                if (taskModel.TaskCategory == (byte)TaskCategory.Learning)
                {
                    var count = taskModel.LearningFiles.Count;
                    for (var i = 0; i < count; i++)
                    {
                        var taskLearningEntity = this._invoicingEntities.TaskLearning.Create();

                        taskLearningEntity.TaskId = taskId;
                        taskLearningEntity.No = i + 1;
                        taskLearningEntity.LearningFileId = taskModel.LearningFiles[i].Id;
                        taskLearningEntity.IsActive = true;
                        taskLearningEntity.LastUpdateTime = DateTime.UtcNow;
                        taskLearningEntity.LastUpdateUser = taskModel.LastUpdateUser;//TODO: Last update user unique id

                        this._invoicingEntities.TaskLearning.Add(taskLearningEntity);
                    }
                    this._invoicingEntities.SaveChanges();
                }
                else if (taskModel.TaskCategory == (byte)TaskCategory.Exam)
                {
                    var count = taskModel.Exams.Count;
                    for (var i = 0; i < count; i++)
                    {
                        var taskExamEntity = this._invoicingEntities.TaskExam.Create();

                        taskExamEntity.TaskId = taskId;
                        taskExamEntity.No = i + 1;
                        taskExamEntity.ExamId = taskModel.Exams[i].Id;
                        taskExamEntity.IsActive = true;
                        taskExamEntity.LastUpdateTime = DateTime.UtcNow;
                        taskExamEntity.LastUpdateUser = taskModel.LastUpdateUser;//TODO: Last update user unique id

                        this._invoicingEntities.TaskExam.Add(taskExamEntity);
                    }

                    this._invoicingEntities.SaveChanges();
                }
                else if (taskModel.TaskCategory == (byte)TaskCategory.StorePatrol)
                {
                    foreach (var sceneId in taskModel.SceneIds)
                    {
                        var storePatrolEntity = this._invoicingEntities.StorePatrol.Create();

                        storePatrolEntity.TaskId = taskModel.TaskId;
                        storePatrolEntity.PatrolCategory = sceneId;

                        storePatrolEntity.LastUpdateUser = taskModel.LastUpdateUser;//TODO: Last update user unique id
                        storePatrolEntity.LastUpdateTime = DateTime.UtcNow;
                        storePatrolEntity.IsActive = true;

                        this._invoicingEntities.StorePatrol.Add(storePatrolEntity);
                        this._invoicingEntities.SaveChanges();

                        //foreach (var userId in taskModel.UserIds)
                        //{
                        //    var userStorePatrolEntity = this._invoicingEntities.UserStorePatrol.Create();

                        //    userStorePatrolEntity.UserUniqueId = userId;
                        //    userStorePatrolEntity.StorePatrolId = storePatrolEntity.Id;


                        //    this._invoicingEntities.UserStorePatrol.Add(userStorePatrolEntity);
                        //}

                        //this._invoicingEntities.SaveChanges();
                    }
                }
                else if (taskModel.TaskCategory == (byte)TaskCategory.Questionnaire)
                {
                    var count = taskModel.Questionnaires.Count;
                    for (var i = 0; i < count; i++)
                    {
                        var taskQuestionnaireEntity = this._invoicingEntities.TaskQuestionnaire.Create();

                        taskQuestionnaireEntity.TaskId = taskId;
                        taskQuestionnaireEntity.No = i + 1;
                        taskQuestionnaireEntity.QuestionnaireId = taskModel.Questionnaires[i].Id;
                        taskQuestionnaireEntity.IsActive = true;
                        taskQuestionnaireEntity.LastUpdateTime = DateTime.UtcNow;
                        taskQuestionnaireEntity.LastUpdateUser = taskModel.LastUpdateUser;//TODO: Last update user unique id

                        this._invoicingEntities.TaskQuestionnaire.Add(taskQuestionnaireEntity);
                        this._invoicingEntities.SaveChanges();
                    }
                }
            }

            return new ResultBase()
            {
                IsSuccess = true,
                StatusCode = (int)StatusCode.Success
            };
        }

        public ResultBase DeleteTaskById(int taskId)
        {
            var taskEntity =
                this._invoicingEntities.Task.FirstOrDefault(
                    p => p.IsActive && p.Id == taskId && p.Status == (byte)ActionCategory.Save);

            if (taskEntity == null) return new ResultBase()
            {
                IsSuccess = false,
                Message = string.Format("Cannot delete task by Id: {0}", taskId),
                StatusCode = (int)StatusCode.Failed
            };

            taskEntity.IsActive = false;
            taskEntity.LastUpdateTime = DateTime.UtcNow;

            this._invoicingEntities.SaveChanges();

            return new ResultBase()
            {
                IsSuccess = true,
                StatusCode = (int)StatusCode.Success
            };
        }

        public int GetQuestionnaireQuestionCount(int taskId)
        {
            return
                this._invoicingEntities.TaskQuestionnaire.Count(
                    p => p.IsActive && p.TaskId == taskId && p.Questionnaire.IsActive);
        }

        public ResultBase UpdateUserTaskStatus(int userTaskId, byte userTaskCategory)
        {
            var userTaskCategoryEntity = this._invoicingEntities.UserTask.FirstOrDefault(p => p.IsActive && p.Id == userTaskId);

            if (userTaskCategoryEntity == null)
                return new ResultBase()
                {
                    IsSuccess = false,
                    Message = string.Format("Cannot find User task by UserTaskId: {0}", userTaskId),
                    StatusCode = (int)StatusCode.Failed
                };

            userTaskCategoryEntity.Status = userTaskCategory;

            this._invoicingEntities.SaveChanges();

            return new ResultBase()
            {
                IsSuccess = true,
                StatusCode = (int)StatusCode.Success
            };
        }

        public byte GetUserTaskStatus(int userTaskId)
        {
            var result = this._invoicingEntities.UserTask.FirstOrDefault(p => p.IsActive && p.Id == userTaskId);

            if (result != null) return result.Status;

            return 0;
        }

        private int CreateTask(TaskDetailsModel model)
        {
            var taskEntity = this._invoicingEntities.Task.Create();

            taskEntity.Title = model.Title;
            taskEntity.Description = model.Description;
            taskEntity.TaskCategory = model.TaskCategory;
            taskEntity.StartFrom = model.StartFrom;
            taskEntity.EndTo = model.EndTo;
            taskEntity.Point = model.Point;
            taskEntity.OverduePoint = model.OverduePoint;
            taskEntity.ProductAbility = model.ProductAbility;
            taskEntity.SalesAbility = model.SalesAbility;
            taskEntity.ExhibitAbility = model.ExhibitAbility;
            taskEntity.BelongTo = model.BelongTo;
            taskEntity.Status = model.Status;
            taskEntity.LastUpdateUser = model.LastUpdateUser;//TODO: Last update user unique id
            taskEntity.LastUpdateTime = DateTime.UtcNow;

            taskEntity.IsActive = true;

            this._invoicingEntities.Task.Add(taskEntity);
            this._invoicingEntities.SaveChanges();

            return taskEntity.Id;
        }

        private ResultBase CreateSingleExamQuestion(int taskId, string userUniqueId, int examQuestionId, string myAnswer)
        {
            var userExamAnswerEntity = this._invoicingEntities.UserExamAnswer.Create();

            if (userExamAnswerEntity == null)
                return new ResultBase()
                {
                    IsSuccess = false,
                    Message = "Create UserExamAnswer Entity failed",
                    StatusCode = (int)StatusCode.Failed
                };

            userExamAnswerEntity.TaskId = taskId;
            userExamAnswerEntity.ExamQuestionId = examQuestionId;
            userExamAnswerEntity.UserUniqueId = userUniqueId;
            userExamAnswerEntity.MyAnswer = myAnswer;

            userExamAnswerEntity.LastUpdateTime = DateTime.UtcNow;
            userExamAnswerEntity.IsActive = true;

            this._invoicingEntities.UserExamAnswer.Add(userExamAnswerEntity);
            this._invoicingEntities.SaveChanges();

            return new ResultBase()
            {
                IsSuccess = true,
                StatusCode = (int)StatusCode.Success
            };
        }
    }
}
