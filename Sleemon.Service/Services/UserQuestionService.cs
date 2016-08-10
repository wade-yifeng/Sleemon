using Sleemon.Common;
using Sleemon.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using Sleemon.Core;

namespace Sleemon.Service
{
    public class UserQuestionService : IUserQuestionService
    {
        private readonly ISleemonEntities _invoicingEntities;

        public UserQuestionService()
        {
            this._invoicingEntities = new SleemonEntities();
        }

        public IList<UserQuestionDetailModel> GetAllUserQuestionsList()
        {
            var entities = this._invoicingEntities.UserQuestion
                .Where(p => p.IsActive)
                .OrderBy(p => p.Status)
                .ThenByDescending(p => p.AskTime).ToList();
            var userEntities = this._invoicingEntities.User.Where(p => p.IsActive).ToList();

            return entities
                .Select(p =>
                {
                    var userName = string.Empty;
                    var asker = userEntities.FirstOrDefault(o => o.UserUniqueId == p.UserUniqueId);
                    if (asker != null)
                    {
                        userName = asker.Name;
                    }

                    var replierName = string.Empty;
                    var replier = userEntities.FirstOrDefault(o => o.UserUniqueId == p.Replier);
                    if (replier != null)
                    {
                        replierName = replier.Name;
                    }

                    return new UserQuestionDetailModel()
                    {
                        QuestionId = p.Id,
                        User = p.UserUniqueId,
                        UserName = userName,
                        Title = p.Title,
                        Question = p.Question,
                        AskTime = p.AskTime,
                        Replier = p.Replier,
                        ReplierName = replierName,
                        Answer = p.Answer,
                        AnswerTime = p.AnswerTime,
                        Status = p.Status
                    };
                })
                .ToList();
        }

        public UserQuestionDetailModel GetUserQuestionById(int questionId)
        {
            var result = this._invoicingEntities.UserQuestion.FirstOrDefault(p => p.IsActive && p.Id == questionId);

            if (result == null) return null;

            var userEntities = this._invoicingEntities.User.Where(p => p.IsActive).ToList();
            
            var asker = userEntities.FirstOrDefault(p => p.UserUniqueId == result.UserUniqueId);
            var replier = userEntities.FirstOrDefault(p => p.UserUniqueId == result.Replier);

            return new UserQuestionDetailModel()
            {
                QuestionId = result.Id,
                User = result.UserUniqueId,
                UserName = asker == null ? string.Empty : asker.Name,
                UserAvatar = asker == null ? string.Empty : asker.Avatar,
                Title = result.Title,
                Question = result.Question,
                AskTime = result.AskTime,
                Replier = result.Replier,
                ReplierName = replier == null ? string.Empty : replier.Name,
                ReplierAvater = replier == null ? string.Empty : replier.Avatar,
                Answer = result.Answer,
                AnswerTime = result.AnswerTime,
                Status = result.Status
            };
        }

        public ResultBase SaveReplyAnswer(UserQuestionDetailModel answer)
        {
            var result = new ResultBase()
            {
                IsSuccess = true,
                StatusCode = (int) StatusCode.Success
            };

            var questionId = answer.QuestionId;
            var questionEntity = this._invoicingEntities.UserQuestion.FirstOrDefault(p => p.IsActive && p.Id == questionId);

            if (questionEntity != null)
            {
                questionEntity.Replier = answer.Replier;
                questionEntity.Answer = answer.Answer;
                questionEntity.AnswerTime = DateTime.UtcNow;
                questionEntity.Status = answer.Status;

                this._invoicingEntities.SaveChanges();
            }
            else
            {
                result.IsSuccess = false;
                result.StatusCode = (int)StatusCode.Failed;
            }

            return result;
        }

        public IList<UserQuestionPreviewModel> GetUserQuestionList(string userId, int pageIndex, int pageSize)
        {
            return this._invoicingEntities.UserQuestion
                .Where(p => p.IsActive && p.UserUniqueId == userId)
                .OrderByDescending(p => p.AskTime)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageIndex * pageSize)
                .Select(p => new UserQuestionPreviewModel()
                {
                    UserQuestionId = p.Id,
                    Title = p.Title,
                    Question = p.Question
                })
                .ToList();
        }

        public UserQuestionBasicModel GetUserQuestionReplyDetail(string userId, int questionId)
        {
            var result = this._invoicingEntities.UserQuestion
                .FirstOrDefault(p => p.IsActive && p.UserUniqueId == userId && p.Id == questionId);

            if (result == null) return null;

            var userEntities = this._invoicingEntities.User.Where(p => p.IsActive).ToList();

            var replier = userEntities.FirstOrDefault(p => p.UserUniqueId == result.Replier);

            return new UserQuestionBasicModel()
            {
                Title = result.Title,
                Question = result.Question,
                AskTime = result.AskTime,
                ReplierAvater = replier != null ? replier.Avatar : string.Empty,
                ReplierName = replier != null ? replier.Name : string.Empty,
                AnswerTime = result.AnswerTime,
                Answer = result.Answer
            };
        }

        public ResultBase CommitUserQuestion(string userId, string title, string question)
        {
            var userQuestionEntity = this._invoicingEntities.UserQuestion.Create();

            if (userQuestionEntity == null)
            {
                return new ResultBase()
                {
                    IsSuccess = false,
                    Message = "Create User Question Entity failed",
                    StatusCode = (int)StatusCode.Failed
                };
            }

            userQuestionEntity.UserUniqueId = userId;
            userQuestionEntity.Title = title;
            userQuestionEntity.Question = question;
            userQuestionEntity.AskTime = DateTime.UtcNow;

            userQuestionEntity.Status = (byte)UserQuestionStatus.NotResolved;
            userQuestionEntity.IsActive = true;

            this._invoicingEntities.UserQuestion.Add(userQuestionEntity);
            this._invoicingEntities.SaveChanges();

            return new ResultBase()
            {
                IsSuccess = true,
                StatusCode = (int)StatusCode.Success
            };
        }
    }
}
