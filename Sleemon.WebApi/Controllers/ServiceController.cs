namespace Sleemon.WebApi.Controllers
{
    #region Reference

    using Microsoft.Practices.Unity;
    using Newtonsoft.Json.Linq;
    using Sleemon.Common;
    using Sleemon.Core;
    using Sleemon.Data;
    using Sleemon.WebApi.Core;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Http;
    using Get = System.Web.Http.HttpGetAttribute;
    using Post = System.Web.Http.HttpPostAttribute;

    #endregion

    public class ServiceController : ApiController
    {
        private readonly ImplementServiceClient _serviceClient;

        public ServiceController([Dependency] ImplementServiceClient serviceClient)
        {
            this._serviceClient = serviceClient;
        }

        #region Job ...

        /// <summary>
        /// 获取待发送消息
        /// </summary>
        [Get]
        public IList<MessageViewModel> GetBroadcastMessage(int maxCount)
        {
            return _serviceClient.Request<IMessageService, IList<MessageViewModel>>(
                service => service.GetBroadcastMessage(maxCount));
        }

        /// <summary>
        /// 同步部门
        /// </summary>
        [Post]
        public SyncResult SyncDepartment(IEnumerable<DepartmentSyncModel> department)
        {
            return _serviceClient.Request<IDepartmentService, SyncResult>(
                service => service.SyncDepartment(department ?? new List<DepartmentSyncModel>()));
        }

        /// <summary>
        /// 同步用户
        /// </summary>
        [Post]
        public SyncResult SyncUser(IEnumerable<UserProfile> userlist)
        {
            return _serviceClient.Request<IUserService, SyncResult>(
                service => service.SyncUser(userlist ?? new List<UserProfile>()));
        }

        #endregion

        #region Training ...

        /// <summary>
        /// [培训] - 获取用户培训列表
        /// </summary>
        [Get]
        public IList<UserTrainingPreviewModel> GetUserTrainingList(bool isAll, string userId, int pageIndex, int pageSize)
        {
            return _serviceClient.Request<ITrainingService, IList<UserTrainingPreviewModel>>(
                service => service.GetUserTrainingList(isAll, userId, pageIndex, pageSize));
        }

        /// <summary>
        /// [培训] - 获取用户培训详情
        /// </summary>
        [Get]
        public UserTrainingDetailModel GetUserTrainingDetail(bool isAll, string userId, int trainingId)
        {
            return _serviceClient.Request<ITrainingService, UserTrainingDetailModel>(
                service => service.GetUserTrainingDetail(isAll, userId, trainingId));
        }

        /// <summary>
        /// [培训] - 获取培训报名用户
        /// </summary>
        [Get]
        public IList<UserViewModel> GetTrainingParticipants(int trainingId)
        {
            return _serviceClient.Request<ITrainingService, IList<UserViewModel>>(
                service => service.GetTrainingParticipants(trainingId));
        }

        /// <summary>
        /// [培训] - 培训报名
        /// </summary>
        [Post]
        public ResultBase JoinTraining(JObject param)
        {
            return _serviceClient.Request<ITrainingService, ResultBase>(
                service =>
                    service.JoinTraining(JObjectHelper.TryGetValue<string>(param, "userId"),
                        JObjectHelper.TryGetValue<int>(param, "trainingId")));
        }

        #endregion

        #region Task ...

        /// <summary>
        /// [任务] - 检索用户任务列表
        /// </summary>
        [Get]
        public IList<UserTaskModel> SearchUserTaskList(string userId, byte listType, string input)
        {
            return _serviceClient.Request<ITaskService, IList<UserTaskModel>>(
                service => service.SearchUserTaskList(userId, listType, input));
        }

        /// <summary>
        /// [任务] - 获取用户任务列表
        /// </summary>
        [Get]
        public IList<UserTaskInfo> GetUserTaskList(string userId, byte listType, int pageIndex, int pageSize)
        {
            return _serviceClient.Request<ITaskService, IList<UserTaskInfo>>(
                service => service.GetUserTaskList(userId, listType, pageIndex, pageSize).ToList());
        }

        /// <summary>
        /// [任务] - 获取用户任务详情
        /// </summary>
        [Get]
        public UserTaskDetailsModel GetUserTaskDetail(int userTaskId)
        {
            return _serviceClient.Request<ITaskService, UserTaskDetailsModel>(
                service => service.GetUserTaskDetail(userTaskId));
        }

        /// <summary>
        /// [任务] - 获取用户任务状态
        /// </summary>
        /// <param name="userTaskId"></param>
        /// <returns></returns>
        [Get]
        public byte GetUserTaskStatus(int userTaskId)
        {
            var userTask = _serviceClient.Request<ITaskService, UserTaskDetailsModel>(
                service => service.GetUserTaskDetail(userTaskId));

            return userTask == null ? (byte)0 : userTask.Status;
        }

        /// <summary>
        /// [任务] - 更新用户任务状态
        /// </summary>
        [Post]
        public ResultBase UpdateUserTaskStatus(JObject param)
        {
            return _serviceClient.Request<ITaskService, ResultBase>(
                service => service.UpdateUserTaskStatus(JObjectHelper.TryGetValue<int>(param, "userTaskId"),
                JObjectHelper.TryGetValue<byte>(param, "userTaskStatus")));
        }

        /// <summary>
        /// [任务] - [考试] - 获取考试任务其它信息
        /// </summary>
        [Get]
        public ExamPreviewModel GetExamTaskOtherInfo(int taskId)
        {
            return _serviceClient.Request<ITaskService, ExamPreviewModel>(
                service => service.GetExamTaskOtherInfo(taskId));
        }

        /// <summary>
        /// [任务] - [考试] - 获取考试题目
        /// </summary>
        [Get]
        public IList<ExamQuestionModel> GetExamQuestions(int taskId)
        {
            return _serviceClient.Request<ITaskService, IList<ExamQuestionModel>>(
                service => service.GetExamQuestions(taskId));
        }

        /// <summary>
        /// [任务] - [考试] - 获取用户考试答案
        /// </summary>
        [Get]
        public IList<ExamAnswerModel> GetUserExamAnswers(int taskId, string userUniqueId)
        {
            return _serviceClient.Request<ITaskService, IList<ExamAnswerModel>>(
                service => service.GetUserExamAnswers(taskId, userUniqueId));
        }

        /// <summary>
        /// [任务] - [考试] - 答题
        /// </summary>
        [Post]
        public ResultBase CommitSingleExamQuestion(JObject param)
        {
            return _serviceClient.Request<ITaskService, ResultBase>(
                service => service.CommitSingleExamQuestion(
                    JObjectHelper.TryGetValue<int>(param, "taskId"),
                    JObjectHelper.TryGetValue<string>(param, "userUniqueId"),
                    JObjectHelper.TryGetValue<int>(param, "examQuestionId"),
                    JObjectHelper.TryGetValue<string>(param, "myAnswer")));
        }

        /// <summary>
        /// [任务] - [考试] - 交卷
        /// </summary>
        [Post]
        public ExamResultModel CommitEntireExam(JObject param)
        {
            return
                _serviceClient.Request<ITaskService, ExamResultModel>(
                service => service.CommitEntireExam(
                    JObjectHelper.TryGetValue<string>(param, "userUniqueId"),
                    JObjectHelper.TryGetValue<int>(param, "userTaskId")));
        }

        #endregion

        #region StorePatrol ...

        [Get]
        public IList<int> GetTaskPatrolCategories(int taskId)
        {
            return _serviceClient.Request<IStorePatrolService, IList<int>>(
                service => service.GetTaskPatrolCategories(taskId));
        }

        [Get]
        public UserStorePatrolDetailModel GetUserStorePatrolDetails(int userTaskId)
        {
            return _serviceClient.Request<IStorePatrolService, UserStorePatrolDetailModel>(
                service => service.GetUserStorePatrolDetails(userTaskId));
        }

        [Get]
        public PagedData<UserStorePatrolPreviewModel> GetStorePatrolList(int pageIndex, int pageSize, string userName, DateTime? startFrom, DateTime? endTo)
        {
            var count = 0;
            var result = _serviceClient.Request<IStorePatrolService, IList<UserStorePatrolPreviewModel>>(
                service => service.GetStorePatrolList(pageIndex, pageSize, userName, startFrom, endTo, out count));

            return new PagedData<UserStorePatrolPreviewModel>()
            {
                Data = result.ToList(),
                TotalCount = count
            };
        }

        [Post]
        public ResultBase UploadStorePatrol(UploadStorePatrolContext context)
        {
            return _serviceClient.Request<IStorePatrolService, ResultBase>(
                service => service.UpLoadStorePatrol(context.UserUniqueId, context.UserStorePatrols));
        }

        [Post]
        public ResultBase PointStorePatrol(PointStorePatrolContext context)
        {
            return _serviceClient.Request<IStorePatrolService, ResultBase>(
                service => service.PointStorePatrol(context.IsPass, context.UserStorePatrols));
        }

        //TODO: Confirm this api can be remove?
        //[Get]
        //public UserStorePatrolDetailsModel GetStorePatrolDetail(int taskId, string userUniqueId)
        //{
        //    return this.storePatrolModelClient.GetStorePatrolDetail(taskId, userUniqueId);
        //}

        [Get]
        public IList<StorePatrolSceneModel> GetStorePatrolScenes()
        {
            return _serviceClient.Request<IStorePatrolService, IList<StorePatrolSceneModel>>(
                service => service.GetStorePatrolScenes());
        }

        #endregion

        #region Questionnaire ...

        /// <summary>
        /// [任务] - [问卷] - 获取问卷总题数
        /// </summary>
        [Get]
        public int GetQuestionnaireQuestionCount(int taskId)
        {
            return _serviceClient.Request<IQuestionnaireService, int>(
                service =>
                    service.GetQuestionnaireQuestionCount(taskId));
        }

        /// <summary>
        /// [任务] - [问卷] - 获取问卷题目
        /// </summary>
        [Get]
        public IList<QuestionnaireQuestionModel> GetQuestionnaireQuestions(int taskId)
        {
            return _serviceClient.Request<IQuestionnaireService, IList<QuestionnaireQuestionModel>>(
                service =>
                    service.GetQuestionnaireQuestions(taskId));
        }

        /// <summary>
        /// [任务] - [问卷] - 提交问卷
        /// </summary>
        [Post]
        public TaskAbilityModel CommitQuestionnaire(CommitQuestionnaireModel context)
        {
            if (context == null) return null;

            return _serviceClient.Request<IQuestionnaireService, TaskAbilityModel>(
                service =>
                    service.CommitQuestionnaire(context.UserId, context.TaskId, context.Answers));
        }

        #endregion

        #region Learning ...

        /// <summary>
        /// [课程] - 获取大课程列表
        /// </summary>
        [Get]
        public IList<CoursePreviewModel> GetCoursesList()
        {
            return _serviceClient.Request<ILearningFileService, IList<CoursePreviewModel>>(
                service => service.GetCoursesList());
        }

        /// <summary>
        /// [课程] - 获取/检索大课程详情
        /// </summary>
        [Get]
        public CourseDetailModel GetCourseDetail(int courseId, string keyword)
        {
            return _serviceClient.Request<ILearningFileService, CourseDetailModel>(
                service => service.GetCourseDetail(courseId, keyword));
        }

        /// <summary>
        /// [课程] - 获取原子小节详情
        /// </summary>
        [Get]
        public CourseLearningFileModel GetCourseLearningFile(int learningFileId, int taskId)
        {
            return _serviceClient.Request<ILearningFileService, CourseLearningFileModel>(
                service => service.GetCourseLearningFile(learningFileId, taskId));
        }

        /// <summary>
        /// [课程] - 完成课程任务
        /// </summary>
        [Post]
        public ResultBase FinishCourseLearningTask(JObject param)
        {
            return _serviceClient.Request<ILearningFileService, ResultBase>(
                service =>
                    service.FinishCourseLearningTask(JObjectHelper.TryGetValue<string>(param, "userUniqueId"),
                        JObjectHelper.TryGetValue<int>(param, "taskId")));
        }

        #endregion

        #region Notice ..

        /// <summary>
        /// [资讯] - 获取资讯详情
        /// </summary>
        [Get]
        public EnterpriseNoticeDetailModel GetNoticeById(int noticeId)
        {
            return _serviceClient.Request<IEnterpriseNoticeService, EnterpriseNoticeDetailModel>(
                service => service.GetEnterpriseNoticeById(noticeId));
        }

        /// <summary>
        /// [资讯] - 获取资讯列表
        /// </summary>
        [Get]
        public IList<EnterpriseNoticePreviewModel> GetNoticeList(int pageIndex, int pageSize)
        {
            return _serviceClient.Request<IEnterpriseNoticeService, IList<EnterpriseNoticePreviewModel>>(
                service => service.GetEnterpriseSummeryNotices(pageIndex, pageSize));
        }

        /// <summary>
        /// [资讯] - 获取滚动资讯
        /// </summary>
        [Get]
        public IList<EnterpriseNoticePreviewModel> GetSlideNoticeList(int topCount = 5)
        {
            return _serviceClient.Request<IEnterpriseNoticeService, IList<EnterpriseNoticePreviewModel>>(
                service => service.GetSlideEnterpriseSummeryNotices(topCount));
        }

        /// <summary>
        /// [资讯] - 获取热门评论
        /// </summary>
        [Get]
        public IList<UserComment> GetTopCommentsByLinkedId(byte category, int linkedId, string userId, int topCount = 3)
        {
            return _serviceClient.Request<ICommentService, IList<UserComment>>(
                service => service.GetTopCommentsByLinkedId(category, linkedId, userId, topCount));
        }

        /// <summary>
        /// [资讯] - 获取最新评论
        /// </summary>
        [Get]
        public PagedData<UserComment> GetPagedCommentsByLinkedId(byte category, int linkedId, string userId, int pageIndex, int pageSize)
        {
            var totalCount = 0;

            return new PagedData<UserComment>()
            {
                Data = _serviceClient.Request<ICommentService, IList<UserComment>>(
                    service => service.GetPagedCommentsByLinkedId(category, linkedId, userId, pageIndex, pageSize, out totalCount)),
                TotalCount = totalCount
            };
        }

        /// <summary>
        /// [资讯] - 顶资讯
        /// </summary>
        [Post]
        public ProConNoticeResult ProNoticeById(JObject param)
        {
            return _serviceClient.Request<IEnterpriseNoticeService, ProConNoticeResult>(
                service => service.ProNoticeById(
                    JObjectHelper.TryGetValue<int>(param, "noticeId"),
                    JObjectHelper.TryGetValue<string>(param, "userId")));
        }

        /// <summary>
        /// [资讯] - 踩资讯
        /// </summary>
        [Post]
        public ProConNoticeResult ConNoticeById(JObject param)
        {
            return _serviceClient.Request<IEnterpriseNoticeService, ProConNoticeResult>(
                service => service.ConNoticeById(
                    JObjectHelper.TryGetValue<int>(param, "noticeId"),
                    JObjectHelper.TryGetValue<string>(param, "userId")));
        }

        /// <summary>
        /// [资讯] - 赞评论
        /// </summary>
        [Post]
        public ProCommentResult ProCommentById(JObject param)
        {
            return _serviceClient.Request<ICommentService, ProCommentResult>(
                service => service.ProCommentById(
                    JObjectHelper.TryGetValue<int>(param, "commentId"),
                    JObjectHelper.TryGetValue<string>(param, "userId")));
        }

        /// <summary>
        /// [资讯] - 获取最新资讯
        /// </summary>
        [Get]
        public IList<EnterpriseNoticePreviewModel> GetLatestNotices(int previousLatestNoticeId)
        {
            return _serviceClient.Request<IEnterpriseNoticeService, IList<EnterpriseNoticePreviewModel>>(
                service => service.GetLatestNotices(previousLatestNoticeId));
        }

        /// <summary>
        /// [资讯] - 创建评论
        /// </summary>
        [Post]
        public NewCommentViewModel AddNewComment(JObject param)
        {
            return _serviceClient.Request<ICommentService, NewCommentViewModel>(
                service => service.AddNewComment(
                    JObjectHelper.TryGetValue<int>(param, "linkedId"),
                    JObjectHelper.TryGetValue<string>(param, "userId"),
                    JObjectHelper.TryGetValue<string>(param, "comment"),
                    JObjectHelper.TryGetValue<byte>(param, "category")));
        }

        #endregion

        #region User ...

        /// <summary>
        /// [我的] - 用户每日签到
        /// </summary>
        [Post]
        public UserDailySignInModel UserDailySignIn(JObject param)
        {
            return _serviceClient.Request<IUserService, UserDailySignInModel>(
                service => service.UserDailySignIn(JObjectHelper.TryGetValue<string>(param, "userId")));
        }

        /// <summary>
        /// [我的] - 根据微信ID获取用户信息
        /// </summary>
        [Get]
        public UserViewModel GetUserInfoById(string userId)
        {
            return _serviceClient.Request<IUserService, UserViewModel>(
                service => service.GetUserInfoById(userId));
        }

        /// <summary>
        /// [我的] - 更新用户信息
        /// </summary>
        [Post]
        public ResultBase UpdateUserProfile(UserProfile model)
        {
            return _serviceClient.Request<IUserService, ResultBase>(
                service => service.UpdateUserProfile(model));
        }

        /// <summary>
        /// [我的] - 获取用户积分记录
        /// </summary>
        [Get]
        public IList<UserPointRecordPreviewModel> GetUserPointRecordList(string userId, int pageIndex, int pageSize)
        {
            return _serviceClient.Request<IUserService, IList<UserPointRecordPreviewModel>>(
                service => service.GetUserPointRecordList(userId, pageIndex, pageSize));
        }

        /// <summary>
        /// [我的] - 获取用户工作动态
        /// </summary>
        [Get]
        public IList<UserMomentPreviewModel> GetUserMomentsList(string userId, int pageIndex, int pageSize)
        {
            return _serviceClient.Request<IUserService, IList<UserMomentPreviewModel>>(
                service => service.GetUserMomentsList(userId, pageIndex, pageSize));
        }

        /// <summary>
        /// [我的] - [问答] - 获取用户问题列表
        /// </summary>
        [Get]
        public IList<UserQuestionPreviewModel> GetUserQuestionList(string userId, int pageIndex, int pageSize)
        {
            return _serviceClient.Request<IUserQuestionService, IList<UserQuestionPreviewModel>>(
               service => service.GetUserQuestionList(userId, pageIndex, pageSize));
        }

        /// <summary>
        /// [我的] - [问答] - 获取用户问答详情
        /// </summary>
        [Get]
        public UserQuestionBasicModel GetUserQuestionReplyDetail(string userId, int questionId)
        {
            return _serviceClient.Request<IUserQuestionService, UserQuestionBasicModel>(
               service => service.GetUserQuestionReplyDetail(userId, questionId));
        }

        /// <summary>
        /// [我的] - [问答] - 答题
        /// </summary>
        [Post]
        public ResultBase CommitUserQuestion(JObject param)
        {
            return _serviceClient.Request<IUserQuestionService, ResultBase>(
                service => service.CommitUserQuestion(
                    JObjectHelper.TryGetValue<string>(param, "userId"),
                    JObjectHelper.TryGetValue<string>(param, "title"),
                    JObjectHelper.TryGetValue<string>(param, "question")));
        }
        #endregion

        #region Knowledge ...

        /// <summary>
        /// [我的] - [知识库] - 检索知识库列表
        /// </summary>
        [Get]
        public IList<KnowledgeBaseModel> SearchKBList(string input)
        {
            return _serviceClient.Request<IKnowledgeService, IList<KnowledgeBaseModel>>(
              service => service.SearchKBList(input));
        }

        /// <summary>
        /// [我的] - [知识库] - 根据分类获取知识库列表
        /// </summary>
        [Get]
        public IList<KnowledgeDetailModel> GetKBListByCategory(int knowledgeCategory)
        {
            return _serviceClient.Request<IKnowledgeService, IList<KnowledgeDetailModel>>(
             service => service.GetKBListByCategory(knowledgeCategory));
        }

        /// <summary>
        /// [我的] - [知识库] - 根据关键字获取知识库列表
        /// </summary>
        [Get]
        public IList<KnowledgeDetailModel> GetKBLisetByKeyword(string keyword)
        {
            return _serviceClient.Request<IKnowledgeService, IList<KnowledgeDetailModel>>(
              service => service.GetKBLisetByKeyword(keyword));
        }

        /// <summary>
        /// [我的] - [知识库] - 获取知识库详情
        /// </summary>
        [Get]
        public KnowledgeDetailModel GetKBDetail(int knowledgeBaseId)
        {
            return _serviceClient.Request<IKnowledgeService, KnowledgeDetailModel>(
             service => service.GetKBDetail(knowledgeBaseId));
        }

        #endregion

        #region Order Show
        /// <summary>
        /// [晒单] - 上传晒单数据
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [Post]
        public ResultBase PostOrderShowRequest(JObject param)
        {
            return _serviceClient.Request<IOrderShowService, ResultBase>(
                service => service.PostOrderShowRequest(
                     JObjectHelper.TryGetValue<string>(param, "userUniqueId"),
                     JObjectHelper.TryGetValue<string>(param, "filePath"),
                     JObjectHelper.TryGetValue<string>(param, "description")
            ));
        }

        [Get]
        public OrderShowResponse GetOrderShowList(int pageIndex, int pageSize, bool IsLegal, string userUniqueId, string userName, DateTime? startShowTime, DateTime? endShowTime)
        {
            return _serviceClient.Request<IOrderShowService, OrderShowResponse>(
                service => service.GetOrderShowList(pageIndex, pageSize, IsLegal, userUniqueId, userName, startShowTime, endShowTime));
        }

        [Post]
        public ResultBase DeleteOrderShowRequest(JObject param)
        {
            return _serviceClient.Request<IOrderShowService, ResultBase>(
                service => service.DeleteOrderShowRequest(JObjectHelper.TryGetValue<int>(param, "userOrderShowId")));
        }

        [Post]
        public ResultBase SetOrderShowIsLegal(JObject param)
        {
            return _serviceClient.Request<IOrderShowService, ResultBase>(
                service => service.SetOrderShowIsLegal(
                  JObjectHelper.TryGetValue<int>(param, "userOrderShowId"),
                  JObjectHelper.TryGetValue<bool>(param, "IsLegal")
            ));
        }

        #endregion
    }
}