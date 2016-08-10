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

    public class QuestionnaireService : IQuestionnaireService
    {
        private readonly ISleemonEntities _invoicingEntities;

        public QuestionnaireService()
        {
            this._invoicingEntities = new SleemonEntities();
        }

        public IList<QuestionnaireListModel> GetQuestionnaireList(int pageIndex, int pageSize, string questionnaireTitle)
        {
            return
                this._invoicingEntities.Database.SqlQuery<QuestionnaireListModel>(@"
WITH [QuestionnaireWithRowNumber] AS
(
    SELECT [Questionnaire].[Id]
          ,[Questionnaire].[Title]
          ,[Questionnaire].[Description]
          ,[User].[Name]                                                                                                        AS [LastUpdateUserName]
          ,[Questionnaire].[LastUpdateUser]
          ,[Questionnaire].[LastUpdateTime]
          ,[Questionnaire].[Status]
          ,ROW_NUMBER() OVER(ORDER BY [Questionnaire].[Status], [Questionnaire].[LastUpdateTime] DESC, [Questionnaire].[Id])    AS [Row]
          ,COUNT([Questionnaire].[Id]) OVER()                                                    AS [TotalCount]
    FROM [dbo].[Questionnaire]
    LEFT JOIN [dbo].[User]
        ON [User].[UserUniqueId] = [Questionnaire].[LastUpdateUser] AND [User].[IsActive] = 1
    WHERE [Questionnaire].[Title] LIKE CONCAT('%', ISNULL(@questionnaireTitle, N''), '%')
          AND [Questionnaire].[IsActive] = 1
)
SELECT [QuestionnaireWithRowNumber].[Id]
      ,[QuestionnaireWithRowNumber].[Title]
      ,[QuestionnaireWithRowNumber].[Description]
      ,[QuestionnaireWithRowNumber].[LastUpdateUserName]
      ,[QuestionnaireWithRowNumber].[LastUpdateUser]
      ,[QuestionnaireWithRowNumber].[LastUpdateTime]
      ,[QuestionnaireWithRowNumber].[Status]
      ,[QuestionnaireWithRowNumber].[TotalCount]
FROM [QuestionnaireWithRowNumber]
WHERE [QuestionnaireWithRowNumber].[Row] BETWEEN (@pageIndex -1) * @pageSize + 1 AND @pageSize * @pageIndex",
                    new SqlParameter("@pageIndex", pageIndex),
                    new SqlParameter("@pageSize", pageSize),
                    new SqlParameter("@questionnaireTitle", questionnaireTitle ?? string.Empty)).ToList();
        }

        public IList<QuestionnaireListModel> GetQuestionnaireList()
        {
            var results = this._invoicingEntities.Questionnaire.Where(p => p.IsActive).ToList();
            var userEntities = this._invoicingEntities.User.Where(p => p.IsActive).ToList();

            return
                results
                    .Select(p =>
                    {
                        var lastUpdateUser = userEntities.FirstOrDefault(o => o.UserUniqueId == p.LastUpdateUser);
                        return new QuestionnaireListModel()
                        {
                            Id = p.Id,
                            Title = p.Title,
                            Description = p.Description,
                            Status = p.Status,
                            LastUpdateUser = p.LastUpdateUser,
                            LastUpdateUserName = lastUpdateUser == null ? string.Empty : lastUpdateUser.Name,
                            LastUpdateTime = p.LastUpdateTime
                        };
                    })
                    .ToList();
        }

        public QuestionnaireDetailModel GetQuestionnaireDetailById(int questionnaireId)
        {
            var result =
                this._invoicingEntities.Questionnaire.Include("QuestionnaireItem.QuestionnaireChoice")
                    .FirstOrDefault(p => p.IsActive && p.Id == questionnaireId);

            if (result == null) return null;

            var lastUpdateUser = this._invoicingEntities.User.FirstOrDefault(p => p.IsActive && p.UserUniqueId == result.LastUpdateUser);

            return new QuestionnaireDetailModel()
            {
                Id = result.Id,
                Title = result.Title,
                Description = result.Description,
                LastUpdateTime = result.LastUpdateTime,
                LastUpdateUser = result.LastUpdateUser,
                LastUpdateUserName = lastUpdateUser == null ? string.Empty : lastUpdateUser.Name,
                Status = result.Status,
                Questions = result.QuestionnaireItem.Where(p => p.IsActive).Select(p => new QuestionnaireItemModel()
                {
                    No = p.No,
                    Question = p.Question,
                    Image = p.Image,
                    Category = p.Category,
                    Rate = p.Rate,
                    Choices = p.QuestionnaireChoice.Where(o => o.IsActive).Select(o => new QuestionnaireChoiceModel()
                    {
                        Choice = o.Choice,
                        Description = o.Description,
                        Image = o.Image
                    }).ToList()
                }).ToList()
            };
        }

        public ResultBase SaveQuestionnaireDetail(QuestionnaireDetailModel questionnaire)
        {
            //TODO: Refactor this Function

            var uniqueIdentifierId = questionnaire.Status == (byte)ActionCategory.Publish ? Guid.NewGuid() : Guid.Empty;

            var questionnaireEntity = this._invoicingEntities.Questionnaire.FirstOrDefault(p => p.IsActive && p.Id == questionnaire.Id);
            if (questionnaireEntity != null)
            {
                if (questionnaireEntity.GroupKey.HasValue)
                {
                    uniqueIdentifierId = questionnaireEntity.GroupKey.Value;
                }
                this._invoicingEntities.spDeleteQuestionnaireById(questionnaireEntity.Id);
            }

            var newquestionnaireEntity = this._invoicingEntities.Questionnaire.Create();

            newquestionnaireEntity.Title = questionnaire.Title;
            newquestionnaireEntity.Description = questionnaire.Description;
            newquestionnaireEntity.LastUpdateTime = DateTime.UtcNow;
            newquestionnaireEntity.LastUpdateUser = questionnaire.LastUpdateUser;
            newquestionnaireEntity.Status = questionnaire.Status;
            if (uniqueIdentifierId != Guid.Empty)
            {
                newquestionnaireEntity.GroupKey = uniqueIdentifierId;
            }
            newquestionnaireEntity.IsActive = true;

            foreach (var questionnaireItemModel in questionnaire.Questions)
            {
                var questionnaireItemEntity = this._invoicingEntities.QuestionnaireItem.Create();

                questionnaireItemEntity.IsActive = true;
                questionnaireItemEntity.LastUpdateUser = questionnaire.LastUpdateUser;
                questionnaireItemEntity.LastUpdateTime = DateTime.UtcNow;

                questionnaireItemEntity.No = questionnaireItemModel.No;
                questionnaireItemEntity.Question = questionnaireItemModel.Question;
                questionnaireItemEntity.Image = questionnaireItemModel.Image;
                questionnaireItemEntity.Category = questionnaireItemModel.Category;
                questionnaireItemEntity.Rate = questionnaireItemModel.Rate;

                if (questionnaireItemModel.Category != (byte)QuestionnaireQuestionCategory.Grade)
                {
                    foreach (var questionnaireChoiceModel in questionnaireItemModel.Choices)
                    {
                        var questionnaireChoiceEntity = this._invoicingEntities.QuestionnaireChoice.Create();

                        questionnaireChoiceEntity.IsActive = true;
                        questionnaireChoiceEntity.LastUpdateUser = questionnaire.LastUpdateUser;
                        questionnaireChoiceEntity.LastUpdateTime = DateTime.UtcNow;

                        questionnaireChoiceEntity.Choice = questionnaireChoiceModel.Choice;
                        questionnaireChoiceEntity.Description = questionnaireChoiceModel.Description;
                        questionnaireChoiceEntity.Image = questionnaireChoiceModel.Image;

                        questionnaireItemEntity.QuestionnaireChoice.Add(questionnaireChoiceEntity);
                    }
                }

                newquestionnaireEntity.QuestionnaireItem.Add(questionnaireItemEntity);
            }

            this._invoicingEntities.Questionnaire.Add(newquestionnaireEntity);
            this._invoicingEntities.SaveChanges();

            return new ResultBase()
            {
                IsSuccess = true,
                StatusCode = (int)StatusCode.Success
            };
        }

        public ResultBase DeleteQuestionnaireById(int questionnaireId)
        {
            this._invoicingEntities.spDeleteQuestionnaireById(questionnaireId);

            return new ResultBase()
            {
                IsSuccess = true,
                StatusCode = (int)StatusCode.Success
            };
        }

        public int GetQuestionnaireQuestionCount(int taskId)
        {
            return
                this._invoicingEntities.TaskQuestionnaire
                    .Include(p => p.Questionnaire)
                    .Where(p => p.IsActive && p.TaskId == taskId && p.Questionnaire.IsActive)
                    .Select(p => p.Questionnaire)
                    .Count();
        }

        public IList<QuestionnaireQuestionModel> GetQuestionnaireQuestions(int taskId)
        {
            var results = new List<QuestionnaireQuestionModel>();

            var queryResult =
                this._invoicingEntities.TaskQuestionnaire
                    .Include("Questionnaire.QuestionnaireItem.QuestionnaireChoice")
                    .Where(p => p.IsActive && p.TaskId == taskId && p.Questionnaire.IsActive)
                    .ToList();

            if (queryResult.Count == 0) return results;

            foreach (var taskQuestionnaire in queryResult)
            {
                foreach (var questionnaireItem in taskQuestionnaire.Questionnaire.QuestionnaireItem)
                {
                    if (questionnaireItem.IsActive)
                    {
                        results.Add(new QuestionnaireQuestionModel()
                        {
                            QuestionnaireItemId = questionnaireItem.Id,
                            No = questionnaireItem.No,
                            Question = questionnaireItem.Question,
                            Image = questionnaireItem.Image,
                            Rate = questionnaireItem.Rate,
                            Category = questionnaireItem.Category,
                            Choices = questionnaireItem.QuestionnaireChoice.Where(p => p.IsActive).Select(p => new QuestionnaireChoiceModel()
                            {
                                Choice = p.Choice,
                                Description = p.Description,
                                Image = p.Image
                            }).ToList()
                        });
                    }
                }
            }

            return results;
        }

        public TaskAbilityModel CommitQuestionnaire(string userUniqueId, int taskId, IList<QuestionnaireAnswerModel> context)
        {
            foreach (var questionnaireAnswerModel in context)
            {
                var userQuestionnaireAnswerEntity =
                    this._invoicingEntities.UserQuestionnaireAnswer.FirstOrDefault(
                        p =>
                            p.IsActive &&
                            p.TaskId == taskId && p.UserUniqueId == userUniqueId &&
                            p.QuestionnaireItemId == questionnaireAnswerModel.QuestionnaireItemId);

                if (userQuestionnaireAnswerEntity != null)
                {
                    userQuestionnaireAnswerEntity.MyAnswer = questionnaireAnswerModel.MyAnswer;
                    userQuestionnaireAnswerEntity.LastUpdateTime = DateTime.UtcNow;
                }
                else
                {
                    var newUserQuestionnaireAnswerEntity = this._invoicingEntities.UserQuestionnaireAnswer.Create();

                    newUserQuestionnaireAnswerEntity.TaskId = taskId;
                    newUserQuestionnaireAnswerEntity.UserUniqueId = userUniqueId;
                    newUserQuestionnaireAnswerEntity.QuestionnaireItemId = questionnaireAnswerModel.QuestionnaireItemId;
                    newUserQuestionnaireAnswerEntity.MyAnswer = questionnaireAnswerModel.MyAnswer;
                    newUserQuestionnaireAnswerEntity.LastUpdateTime = DateTime.UtcNow;
                    newUserQuestionnaireAnswerEntity.IsActive = true;

                    this._invoicingEntities.UserQuestionnaireAnswer.Add(newUserQuestionnaireAnswerEntity);
                }
            }

            var userTaskEntity =
                this._invoicingEntities
                    .UserTask
                    .Include(p => p.Task)
                    .FirstOrDefault(
                        p => p.IsActive && p.UserUniqueId == userUniqueId && p.TaskId == taskId);

            var userPoint = 0;

            if (userTaskEntity != null)
            {
                var taskEntity = userTaskEntity.Task;

                userPoint = taskEntity.EndTo.HasValue
                    ? (taskEntity.EndTo.Value < DateTime.UtcNow ? taskEntity.OverduePoint : taskEntity.Point)
                    : taskEntity.Point;

                userTaskEntity.Status = (byte)UserTaskStatus.Completed;
                userTaskEntity.CompleteTime = DateTime.UtcNow;
                userTaskEntity.Point = userPoint;
            }

            var userPointRecordEntity = this._invoicingEntities.UserPointRecord.Create();

            userPointRecordEntity.UserUniqueId = userUniqueId;
            userPointRecordEntity.Point = userPoint;
            userPointRecordEntity.Operator = true;
            userPointRecordEntity.Description = string.Format("完成问卷调查获得积分: {0}", userPoint);
            userPointRecordEntity.LastUpdateTime = DateTime.UtcNow;

            this._invoicingEntities.UserPointRecord.Add(userPointRecordEntity);

            var userMomentEntity = this._invoicingEntities.UserMoments.Create();

            userMomentEntity.UserUniqueId = userUniqueId;
            userMomentEntity.Moment = string.Format("完成问卷调查");
            userMomentEntity.Category = (byte)UserMomentTypes.Questionnaire;
            userMomentEntity.PostTime = DateTime.UtcNow;
            userMomentEntity.IsActive = true;

            this._invoicingEntities.UserMoments.Add(userMomentEntity);

            this._invoicingEntities.SaveChanges();

            return userTaskEntity == null ? null : new TaskAbilityModel()
            {
                UserPoint = userPoint,
                ProductAbility = userTaskEntity.Task.ProductAbility,
                SalesAbility = userTaskEntity.Task.SalesAbility,
                ExhibitAbility = userTaskEntity.Task.ExhibitAbility
            };
        }

        public IList<QuestionnaireQuestionPreviewModel> GetQuestionnaireStatistics(string userUniqueId, int taskId)
        {
            //var taskEntities =
            //    this._invoicingEntities.UserTask.Include("").Where(
            //        p => p.IsActive && p.TaskId == taskId && p.UserUniqueId == userUniqueId);

            throw new NotImplementedException();
        }
    }
}

