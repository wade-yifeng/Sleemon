namespace Sleemon.Service
{
    using System;
    using System.Linq;
    using System.Data.Entity;
    using System.Collections.Generic;

    using Microsoft.Practices.Unity;

    using Sleemon.Core;
    using Sleemon.Data;
    using Sleemon.Common;

    public class LearningFileService : ILearningFileService
    {
        private readonly ISleemonEntities _invoicingEntities;

        public LearningFileService()
        {
            this._invoicingEntities = new SleemonEntities();
        }

        public IList<LearningFileListModel> GetLearningFileList(int pageIndex, int pageSize, string fileTitle)
        {
            var results = new List<LearningFile>();
            var userEntities = this._invoicingEntities.User.Where(p => p.IsActive).ToList();

            if (string.IsNullOrEmpty(fileTitle))
            {
                results = this._invoicingEntities.LearningFile.Where(p => p.IsActive).ToList();
            }
            else
            {
                results =
                    this._invoicingEntities.LearningFile.Where(p => p.IsActive && p.Subject.Contains(fileTitle)).ToList();
            }

            return
                results.OrderByDescending(p => p.LastUpdateTime)
                    .ThenBy(p => p.Id)
                    .Skip((pageIndex - 1) * pageSize)
                    .Take(pageIndex * pageSize)
                    .Select(p =>
                    {
                        var lastUpdateUser = userEntities.FirstOrDefault(o => o.UserUniqueId == p.LastUpdateUser);
                        return new LearningFileListModel()
                        {
                            Id = p.Id,
                            Subject = p.Subject,
                            Content = p.Content,
                            LastUpdateUser = p.LastUpdateUser,
                            LastUpdateUserName = lastUpdateUser == null ? string.Empty : lastUpdateUser.Name,
                            LastUpdateTime = p.LastUpdateTime,
                            TotalCount = results.Count
                        };
                    })
                    .ToList();
        }

        public IList<LearningFileListModel> GetLearningFileList()
        {
            var results = this._invoicingEntities.LearningFile.Where(p => p.IsActive).ToList();
            var userEntities = this._invoicingEntities.User.Where(p => p.IsActive).ToList();
            
            return
                results
                    .Select(p =>
                    {
                        var lastUpdateUser = userEntities.FirstOrDefault(o => o.UserUniqueId == p.LastUpdateUser);
                        return new LearningFileListModel()
                        {
                            Id = p.Id,
                            Subject = p.Subject,
                            Content = p.Content,
                            LastUpdateUser = p.LastUpdateUser,
                            LastUpdateUserName = lastUpdateUser == null ? string.Empty : lastUpdateUser.Name,
                            LastUpdateTime = p.LastUpdateTime,
                            TotalCount = results.Count
                        };
                    })
                    .ToList();
        }

        public LearningFileModel GetLearningFileById(int fileId)
        {
            var learningEntity = this._invoicingEntities.LearningFile.FirstOrDefault(p => p.IsActive && p.Id == fileId);

            if (learningEntity == null) return null;

            var lastUpdateUser =
                this._invoicingEntities.User.FirstOrDefault(
                    p => p.IsActive && p.UserUniqueId == learningEntity.LastUpdateUser);

            return new LearningFileModel()
            {
                Id = learningEntity.Id,
                Subject = learningEntity.Subject,
                Description = learningEntity.Description,
                Content = learningEntity.Content,
                FileType = learningEntity.FileType,
                LastUpdateUser = lastUpdateUser != null ? lastUpdateUser.UserUniqueId : string.Empty,
                LastUpdateUserName = lastUpdateUser != null ? lastUpdateUser.Name : string.Empty,
                LastUpdateTime = learningEntity.LastUpdateTime
            };
        }

        public IList<CoursePreviewModel> GetCoursesList()
        {
            return
                this._invoicingEntities.LearningCourse.Where(p => p.IsActive)
                    .Select(p => new CoursePreviewModel()
                    {
                        CourseId = p.Id,
                        Subject = p.Subject
                    })
                    .ToList();
        }

        public CourseDetailModel GetCourseDetail(int courseId, string keyword)
        {
            var result = this._invoicingEntities
                .LearningCourse
                .Include("LearningChapter.LearningFile")
                .FirstOrDefault(p => p.IsActive && p.Id == courseId);

            if (result == null) return null;

            return new CourseDetailModel()
            {
                Subject = result.Subject,
                Description = result.Description,
                Star = result.Star ?? 0,
                ForLevel = result.ForLevel ?? 0,
                Chapters = result.LearningChapter
                    .Where(p => p.IsActive && (string.IsNullOrEmpty(keyword) || p.Title.Contains(keyword)))
                    .Select(p => new ChapterPreviewModel()
                    {
                        Title = p.Title,
                        LearningFiles = p.LearningFile
                            .Where(o => p.IsActive && (string.IsNullOrEmpty(keyword) || o.Subject.Contains(keyword)))
                            .Select(o => new LearningFilePreviewModel()
                            {
                                LearningFileId = o.Id,
                                Subject = o.Subject
                            })
                            .ToList()
                    })
                    .ToList()
            };
        }

        public CourseLearningFileModel GetCourseLearningFile(int learningFileId, int taskId)
        {
            if (learningFileId > 0)
            {
                var result = this._invoicingEntities.LearningFile.FirstOrDefault(p => p.IsActive && p.Id == learningFileId);

                if (result == null) return null;

                return new CourseLearningFileModel()
                {
                    Subject = result.Subject,
                    Description = result.Description,
                    FileType = result.FileType,
                    FilePath = result.Content
                };
            }
            else
            {
                var result =
                    this._invoicingEntities.TaskLearning.Include(p => p.LearningFile)
                        .FirstOrDefault(p => p.IsActive && p.TaskId == taskId);

                if (result == null || result.LearningFile == null || !result.LearningFile.IsActive) return null;

                return new CourseLearningFileModel()
                {
                    Subject = result.LearningFile.Subject,
                    Description = result.LearningFile.Description,
                    FileType = result.LearningFile.FileType,
                    FilePath = result.LearningFile.Content
                };
            }
        }

        public ResultBase FinishCourseLearningTask(string userUniqueId, int taskId)
        {
            var userTask =
                this._invoicingEntities.UserTask.Include(p => p.Task).FirstOrDefault(
                    p => p.IsActive && p.UserUniqueId == userUniqueId && p.TaskId == taskId);

            if (userTask == null) return new ResultBase()
            {
                IsSuccess = false,
                Message = string.Format("Cannot find user task by userId: {0} and taskId: {1}", userUniqueId, taskId),
                StatusCode = (int)StatusCode.Failed
            };

            var userPoint = userTask.Task.EndTo.HasValue
                ? (userTask.Task.EndTo.Value > DateTime.UtcNow ? userTask.Task.Point : userTask.Task.OverduePoint)
                : userTask.Task.Point;

            userTask.CompleteTime = DateTime.UtcNow;
            userTask.Status = (byte) UserTaskStatus.Completed;
            userTask.Point = userPoint;

            var userPointRecordEntity = this._invoicingEntities.UserPointRecord.Create();

            userPointRecordEntity.UserUniqueId = userUniqueId;
            userPointRecordEntity.Point = userPoint;
            userPointRecordEntity.Operator = true;
            userPointRecordEntity.Description = string.Format("完成课程学习，获得积分: {0}", userPoint);
            userPointRecordEntity.LastUpdateTime = DateTime.UtcNow;

            this._invoicingEntities.UserPointRecord.Add(userPointRecordEntity);

            var userMomentEntity = this._invoicingEntities.UserMoments.Create();

            userMomentEntity.UserUniqueId = userUniqueId;
            userMomentEntity.Moment = string.Format("完成课程学习");
            userMomentEntity.Category = (byte)UserMomentTypes.LearningTask;
            userMomentEntity.PostTime = DateTime.UtcNow;
            userMomentEntity.IsActive = true;

            this._invoicingEntities.UserMoments.Add(userMomentEntity);

            this._invoicingEntities.SaveChanges();

            return new ResultBase()
            {
                IsSuccess = true,
                StatusCode = (int)StatusCode.Success
            };
        }

        public IList<LearningCourseListModel> GetCourseList(int pageIndex, int pageSize, string courseTitle)
        {
            var results = new List<LearningCourse>();
            var userEntities = this._invoicingEntities.User.Where(p => p.IsActive).ToList();

            if (string.IsNullOrEmpty(courseTitle))
            {
                results = this._invoicingEntities.LearningCourse.Where(p => p.IsActive).ToList();
            }
            else
            {
                results =
                    this._invoicingEntities.LearningCourse.Where(p => p.IsActive && p.Subject.Contains(courseTitle)).ToList();
            }

            return
                results.OrderByDescending(p => p.LastUpdateTime)
                    .ThenBy(p => p.Id)
                    .Skip((pageIndex - 1) * pageSize)
                    .Take(pageIndex * pageSize)
                    .Select(p =>
                    {
                        var lastUpdateUser = userEntities.FirstOrDefault(o => o.UserUniqueId == p.LastUpdateUser);
                        return new LearningCourseListModel()
                        {
                            Id = p.Id,
                            Subject = p.Subject,
                            Description = p.Description,
                            Star = p.Star ?? 0,
                            ForLevel = p.ForLevel ?? 0,
                            Status = p.Status,
                            LastUpdateUser = p.LastUpdateUser,
                            LastUpdateUserName = lastUpdateUser == null ? string.Empty : lastUpdateUser.Name,
                            LastUpdateTime = p.LastUpdateTime,
                            TotalCount = results.Count
                        };
                    })
                    .ToList();
        }

        public IList<LearningCourseDetailModel> GetCourseList()
        {
            var results = 
                this._invoicingEntities.LearningCourse
                .Include("LearningChapter.LearningFile")
                .Where(p => p.IsActive).ToList();
            var userEntities = this._invoicingEntities.User.Where(p => p.IsActive);

            return
                results
                    .Select(p =>
                    {
                        var lastUpdateUser = userEntities.FirstOrDefault(o => o.UserUniqueId == p.LastUpdateUser);
                        return new LearningCourseDetailModel()
                        {
                            Id = p.Id,
                            Subject = p.Subject,
                            Description = p.Description,
                            Star = p.Star ?? 0,
                            ForLevel = p.ForLevel ?? 0,
                            Status = p.Status,
                            LastUpdateUser = p.LastUpdateUser,
                            LastUpdateTime = p.LastUpdateTime,
                            LastUpdateUserName = lastUpdateUser == null ? string.Empty : lastUpdateUser.Name,
                            Chapters = p.LearningChapter.Where(c => c.IsActive).Select(c =>
                              {

                                  var chapterLastUpdateUser =
                                      userEntities.FirstOrDefault(o => o.IsActive && o.UserUniqueId == c.LastUpdateUser);
                                  return new LearningChapterDetailModel()
                                  {
                                      Id = c.Id,
                                      Title = c.Title,
                                      Description = c.Description,
                                      No = c.No,
                                      LastUpdateUserName = chapterLastUpdateUser == null ? string.Empty : chapterLastUpdateUser.Name,
                                      LastUpdateUser = p.LastUpdateUser,
                                      LastUpdateTime = p.LastUpdateTime,
                                      Files = c.LearningFile.Where(t => t.IsActive).Select(t =>
                                      {
                                          var learningFileLastUpdateUser =
                                              userEntities.FirstOrDefault(o => o.IsActive && o.UserUniqueId == t.LastUpdateUser);
                                          return new LearningFileModel()
                                          {
                                              Id = t.Id,
                                              Subject = t.Subject,
                                              Description = t.Description,
                                              Content = t.Content,
                                              FileType = t.FileType,
                                              No = t.No,
                                              LastUpdateUser = t.LastUpdateUser,
                                              LastUpdateUserName = learningFileLastUpdateUser == null ? string.Empty : learningFileLastUpdateUser.Name,
                                              LastUpdateTime = t.LastUpdateTime
                                          };
                                      }).ToList()
                                  };
                              }).ToList()
                        };
                    }).ToList();
        }

        public LearningCourseDetailModel GetCourseDetailById(int courseId)
        {
            var result =
                this._invoicingEntities.LearningCourse
                    .Include("LearningChapter.LearningFile")
                    .FirstOrDefault(p => p.IsActive && p.Id == courseId);

            if (result == null) return null;

            var userEntities = this._invoicingEntities.User.Where(p => p.IsActive).ToList();

            var learningCourseLastUpdateUser = userEntities.FirstOrDefault(p => p.IsActive && p.UserUniqueId == result.LastUpdateUser);

            return new LearningCourseDetailModel()
            {
                Id = result.Id,
                Subject = result.Subject,
                Description = result.Description,
                Star = result.Star ?? 0,
                ForLevel = result.ForLevel ?? 0,
                Status = result.Status,
                LastUpdateUser = result.LastUpdateUser,
                LastUpdateTime = result.LastUpdateTime,
                LastUpdateUserName = learningCourseLastUpdateUser == null ? string.Empty : learningCourseLastUpdateUser.Name,
                Chapters = result.LearningChapter.Select(p =>
                {

                    var learningChapterLastUpdateUser =
                        userEntities.FirstOrDefault(o => o.IsActive && o.UserUniqueId == p.LastUpdateUser);
                    return new LearningChapterDetailModel()
                    {
                        Id = p.Id,
                        Title = p.Title,
                        Description = p.Description,
                        No = p.No,
                        LastUpdateUserName = learningChapterLastUpdateUser == null ? string.Empty : learningChapterLastUpdateUser.Name,
                        LastUpdateUser = p.LastUpdateUser,
                        LastUpdateTime = p.LastUpdateTime,
                        Files = p.LearningFile.Where(t => t.IsActive).Select(t =>
                        {
                            var learningFileLastUpdateUser =
                                userEntities.FirstOrDefault(o => o.IsActive && o.UserUniqueId == t.LastUpdateUser);
                            return new LearningFileModel()
                            {
                                Id = t.Id,
                                Subject = t.Subject,
                                Description = t.Description,
                                Content = t.Content,
                                FileType = t.FileType,
                                No = t.No,
                                LastUpdateUser = t.LastUpdateUser,
                                LastUpdateUserName = learningFileLastUpdateUser == null ? string.Empty : learningFileLastUpdateUser.Name,
                                LastUpdateTime = t.LastUpdateTime
                            };
                        }).ToList()
                    };
                }).ToList()
            };
        }

        public ResultBase SaveCourseDetail(LearningCourseDetailModel course)
        {
            //TODO: Add Transaction

            Guid? groupKey = null;
            var courseEntity =
                this._invoicingEntities.LearningCourse.FirstOrDefault(p => p.IsActive && p.Id == course.Id);

            if (courseEntity != null)
            {
                courseEntity.IsActive = false;
                courseEntity.LastUpdateTime = DateTime.UtcNow;
                courseEntity.LastUpdateUser = course.LastUpdateUser;

                if (courseEntity.GroupKey.HasValue)
                {
                    groupKey = courseEntity.GroupKey.Value;
                }
                else
                {
                    groupKey = courseEntity.GroupKey = Guid.NewGuid();
                }

                var chapterEntities =
                    this._invoicingEntities.LearningChapter.Where(p => p.IsActive && p.CourseId == courseEntity.Id)
                        .ToList();
                var chapterIds = chapterEntities.Select(p => p.Id).ToList();

                var learningFileEntities =
                    this._invoicingEntities.LearningFile.Where(p => p.IsActive && chapterIds.Any(o => o == p.ChapterId))
                        .ToList();

                foreach (var learningFileEntity in learningFileEntities)
                {
                    learningFileEntity.IsActive = false;
                    learningFileEntity.LastUpdateTime = DateTime.UtcNow;
                    learningFileEntity.LastUpdateUser = course.LastUpdateUser;
                }

                foreach (var learningFileEntity in learningFileEntities)
                {
                    learningFileEntity.IsActive = false;
                    learningFileEntity.LastUpdateTime = DateTime.UtcNow;
                    learningFileEntity.LastUpdateUser = course.LastUpdateUser;
                }

                this._invoicingEntities.SaveChanges();
            }

            this.CreateCourseDetail(course, groupKey);

            return new ResultBase()
            {
                IsSuccess = true,
                StatusCode = (int)StatusCode.Success
            };
        }

        public ResultBase DeleteCourseById(int courseId)
        {
            this._invoicingEntities.spDeleteCourseById(courseId);

            return new ResultBase()
            {
                IsSuccess = true,
                StatusCode = (int)StatusCode.Success
            };
        }

        private void CreateCourseDetail(LearningCourseDetailModel course, Guid? groupKey)
        {
            var courseEntity = this._invoicingEntities.LearningCourse.Create();

            courseEntity.Subject = course.Subject;
            courseEntity.Description = course.Description;
            courseEntity.Star = course.Star;
            courseEntity.ForLevel = course.ForLevel;
            courseEntity.Status = course.Status;
            courseEntity.LastUpdateUser = course.LastUpdateUser;
            courseEntity.LastUpdateTime = DateTime.UtcNow;
            courseEntity.GroupKey = groupKey;
            courseEntity.IsActive = true;

            if (course.Chapters != null && course.Chapters.Any())
            {
                foreach (var learningChapterDetailModel in course.Chapters)
                {
                    var chapterEntity = this._invoicingEntities.LearningChapter.Create();

                    chapterEntity.Title = learningChapterDetailModel.Title;
                    chapterEntity.Description = learningChapterDetailModel.Description;
                    chapterEntity.No = learningChapterDetailModel.No;
                    chapterEntity.LastUpdateUser = learningChapterDetailModel.LastUpdateUser;
                    chapterEntity.LastUpdateTime = DateTime.UtcNow;
                    chapterEntity.IsActive = true;

                    if (learningChapterDetailModel.Files != null && learningChapterDetailModel.Files.Any())
                    {
                        foreach (var learningFileDetailModel in learningChapterDetailModel.Files)
                        {
                            var learningFileEntity = this._invoicingEntities.LearningFile.Create();

                            learningFileEntity.Subject = learningFileDetailModel.Subject;
                            learningFileEntity.Description = learningFileDetailModel.Description;
                            learningFileEntity.Content = learningFileDetailModel.Content;
                            learningFileEntity.FileType = learningFileDetailModel.FileType;
                            learningFileEntity.No = learningFileDetailModel.No;
                            learningFileEntity.LastUpdateUser = learningFileDetailModel.LastUpdateUser;
                            learningFileEntity.LastUpdateTime = DateTime.UtcNow;
                            learningFileEntity.IsActive = true;

                            chapterEntity.LearningFile.Add(learningFileEntity);
                        }
                    }

                    courseEntity.LearningChapter.Add(chapterEntity);
                }
            }

            this._invoicingEntities.LearningCourse.Add(courseEntity);
            this._invoicingEntities.SaveChanges();
        }
    }
}
