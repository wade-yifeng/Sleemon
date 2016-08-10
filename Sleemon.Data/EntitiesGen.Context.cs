

// This file is auto-generated, do not modify this file.
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;

namespace Sleemon.Data
{
    public sealed class SleemonEntities : DbContext, ISleemonEntities
    {
        public const string ConnectionString = "name=SleemonEntities";
        public const string ContainerName = "SleemonEntities";
    
    	public void Detach(object obj)
    	{
    		Entry(obj).State = EntityState.Detached;
    	}
    	
    	public void DiscardChanges()
    	{
    		_Context.DiscardChanges();
    	}
    
    	
    	private ObjectContext _Context
    	{
    		get { return ((IObjectContextAdapter)this).ObjectContext; }
    	}
    	
    
        #region Constructors
    
        public SleemonEntities()
            : this(ConnectionString)
        {
        }
    
        public SleemonEntities(string connectionString, int? commandTimeout = null)
            : base(connectionString)
        {
            Configuration.LazyLoadingEnabled = false;
    		_Context.CommandTimeout = commandTimeout ?? 120;
    		Configuration.ValidateOnSaveEnabled = false; 
        }
    
        #endregion
    	
    	bool ISleemonEntities.CanPreCompile
    	{
    		get { return true; }
    	}
    
        #region DbSet Properties
        public DbSet<C__RefactorLog> C__RefactorLog { get; set; }
        public DbSet<ConsEnterpriseNotice> ConsEnterpriseNotice { get; set; }
        public DbSet<Department> Department { get; set; }
        public DbSet<DepartmentEnterpriseNotice> DepartmentEnterpriseNotice { get; set; }
        public DbSet<EnterpriseNotice> EnterpriseNotice { get; set; }
        public DbSet<Exam> Exam { get; set; }
        public DbSet<ExamChoice> ExamChoice { get; set; }
        public DbSet<ExamQuestion> ExamQuestion { get; set; }
        public DbSet<GroupSubTask> GroupSubTask { get; set; }
        public DbSet<GroupTask> GroupTask { get; set; }
        public DbSet<IllegalCharacters> IllegalCharacters { get; set; }
        public DbSet<Keyword> Keyword { get; set; }
        public DbSet<KnowledgeBase> KnowledgeBase { get; set; }
        public DbSet<KnowledgeBaseKeyword> KnowledgeBaseKeyword { get; set; }
        public DbSet<LearningChapter> LearningChapter { get; set; }
        public DbSet<LearningCourse> LearningCourse { get; set; }
        public DbSet<LearningFile> LearningFile { get; set; }
        public DbSet<MessageDispatch> MessageDispatch { get; set; }
        public DbSet<MessageReceiver> MessageReceiver { get; set; }
        public DbSet<OrderShowFile> OrderShowFile { get; set; }
        public DbSet<Permission> Permission { get; set; }
        public DbSet<ProsComments> ProsComments { get; set; }
        public DbSet<ProsEnterpriseNotice> ProsEnterpriseNotice { get; set; }
        public DbSet<Questionnaire> Questionnaire { get; set; }
        public DbSet<QuestionnaireChoice> QuestionnaireChoice { get; set; }
        public DbSet<QuestionnaireItem> QuestionnaireItem { get; set; }
        public DbSet<Role> Role { get; set; }
        public DbSet<RolePermission> RolePermission { get; set; }
        public DbSet<SleemonExceptionLog> SleemonExceptionLog { get; set; }
        public DbSet<StorePatrol> StorePatrol { get; set; }
        public DbSet<SystemConfig> SystemConfig { get; set; }
        public DbSet<Task> Task { get; set; }
        public DbSet<TaskExam> TaskExam { get; set; }
        public DbSet<TaskLearning> TaskLearning { get; set; }
        public DbSet<TaskQuestionnaire> TaskQuestionnaire { get; set; }
        public DbSet<Training> Training { get; set; }
        public DbSet<TrainingTask> TrainingTask { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<UserComments> UserComments { get; set; }
        public DbSet<UserDailySignIn> UserDailySignIn { get; set; }
        public DbSet<UserDepartment> UserDepartment { get; set; }
        public DbSet<UserExamAnswer> UserExamAnswer { get; set; }
        public DbSet<UserMoments> UserMoments { get; set; }
        public DbSet<UserOrderShow> UserOrderShow { get; set; }
        public DbSet<UserPointRecord> UserPointRecord { get; set; }
        public DbSet<UserQuestion> UserQuestion { get; set; }
        public DbSet<UserQuestionnaireAnswer> UserQuestionnaireAnswer { get; set; }
        public DbSet<UserRole> UserRole { get; set; }
        public DbSet<UserStorePatrol> UserStorePatrol { get; set; }
        public DbSet<UserTask> UserTask { get; set; }
        public DbSet<UserTraining> UserTraining { get; set; }

        #endregion

    
        #region Function Imports
    
    	public ObjectResult<spCommitEntireExam_Result> spCommitEntireExam(Nullable<int> userTaskId, string userId)
    	{
    		ObjectParameter userTaskIdParameter = userTaskId.HasValue ? new ObjectParameter("userTaskId", userTaskId) : new ObjectParameter("userTaskId", typeof(int));
    		ObjectParameter userIdParameter = userId != null ? new ObjectParameter("userId", userId) : new ObjectParameter("userId", typeof(string));
    		return _Context.ExecuteFunction<spCommitEntireExam_Result>("spCommitEntireExam", userTaskIdParameter, userIdParameter);
    	}
     
    	IEnumerable<spCommitEntireExam_Result>  ISleemonEntities.spCommitEntireExam(Nullable<int> userTaskId, string userId)
    	{
     		return  this.spCommitEntireExam(userTaskId, userId);	
    	}
    
    	public void spDeleteCourseById(Nullable<int> courseId)
    	{
    		ObjectParameter courseIdParameter = courseId.HasValue ? new ObjectParameter("courseId", courseId) : new ObjectParameter("courseId", typeof(int));
    		_Context.ExecuteFunction("spDeleteCourseById", courseIdParameter);
    	}
    
    	public void spDeleteExamById(Nullable<int> examId)
    	{
    		ObjectParameter examIdParameter = examId.HasValue ? new ObjectParameter("examId", examId) : new ObjectParameter("examId", typeof(int));
    		_Context.ExecuteFunction("spDeleteExamById", examIdParameter);
    	}
    
    	public void spDeleteQuestionnaireById(Nullable<int> questionnaireId)
    	{
    		ObjectParameter questionnaireIdParameter = questionnaireId.HasValue ? new ObjectParameter("questionnaireId", questionnaireId) : new ObjectParameter("questionnaireId", typeof(int));
    		_Context.ExecuteFunction("spDeleteQuestionnaireById", questionnaireIdParameter);
    	}
    
    	public ObjectResult<spGetBroadcastMessage_Result> spGetBroadcastMessage(Nullable<int> maxCount)
    	{
    		ObjectParameter maxCountParameter = maxCount.HasValue ? new ObjectParameter("maxCount", maxCount) : new ObjectParameter("maxCount", typeof(int));
    		return _Context.ExecuteFunction<spGetBroadcastMessage_Result>("spGetBroadcastMessage", maxCountParameter);
    	}
     
    	IEnumerable<spGetBroadcastMessage_Result>  ISleemonEntities.spGetBroadcastMessage(Nullable<int> maxCount)
    	{
     		return  this.spGetBroadcastMessage(maxCount);	
    	}
    
    	public ObjectResult<spGetStorePatrolList_Result> spGetStorePatrolList(Nullable<int> pageIndex, Nullable<int> pageSize, string userName, Nullable<System.DateTime> startFrom, Nullable<System.DateTime> endTo)
    	{
    		ObjectParameter pageIndexParameter = pageIndex.HasValue ? new ObjectParameter("pageIndex", pageIndex) : new ObjectParameter("pageIndex", typeof(int));
    		ObjectParameter pageSizeParameter = pageSize.HasValue ? new ObjectParameter("pageSize", pageSize) : new ObjectParameter("pageSize", typeof(int));
    		ObjectParameter userNameParameter = userName != null ? new ObjectParameter("userName", userName) : new ObjectParameter("userName", typeof(string));
    		ObjectParameter startFromParameter = startFrom.HasValue ? new ObjectParameter("startFrom", startFrom) : new ObjectParameter("startFrom", typeof(System.DateTime));
    		ObjectParameter endToParameter = endTo.HasValue ? new ObjectParameter("endTo", endTo) : new ObjectParameter("endTo", typeof(System.DateTime));
    		return _Context.ExecuteFunction<spGetStorePatrolList_Result>("spGetStorePatrolList", pageIndexParameter, pageSizeParameter, userNameParameter, startFromParameter, endToParameter);
    	}
     
    	IEnumerable<spGetStorePatrolList_Result>  ISleemonEntities.spGetStorePatrolList(Nullable<int> pageIndex, Nullable<int> pageSize, string userName, Nullable<System.DateTime> startFrom, Nullable<System.DateTime> endTo)
    	{
     		return  this.spGetStorePatrolList(pageIndex, pageSize, userName, startFrom, endTo);	
    	}
    
    	public void spPointStorePatrol(Nullable<bool> isPass, string userStorePatrol)
    	{
    		ObjectParameter isPassParameter = isPass.HasValue ? new ObjectParameter("isPass", isPass) : new ObjectParameter("isPass", typeof(bool));
    		ObjectParameter userStorePatrolParameter = userStorePatrol != null ? new ObjectParameter("userStorePatrol", userStorePatrol) : new ObjectParameter("userStorePatrol", typeof(string));
    		_Context.ExecuteFunction("spPointStorePatrol", isPassParameter, userStorePatrolParameter);
    	}
    
    	public void spSaveTrainingDetail(string training)
    	{
    		ObjectParameter trainingParameter = training != null ? new ObjectParameter("training", training) : new ObjectParameter("training", typeof(string));
    		_Context.ExecuteFunction("spSaveTrainingDetail", trainingParameter);
    	}
    
    	public void spSyncDepartment(string department)
    	{
    		ObjectParameter departmentParameter = department != null ? new ObjectParameter("department", department) : new ObjectParameter("department", typeof(string));
    		_Context.ExecuteFunction("spSyncDepartment", departmentParameter);
    	}
    
    	public void spSyncUser(string users)
    	{
    		ObjectParameter usersParameter = users != null ? new ObjectParameter("users", users) : new ObjectParameter("users", typeof(string));
            _Context.ExecuteFunction("spSyncUser", usersParameter);
    	}

        //public IEnumerable<spGetStorePatrolList_Result> spGetAdminsWithRoles(string adminRoles)
        //{
        //    return _Context.ExecuteFunction("spGetAdminsWithRoles");
        //}

        public bool spUpdateAdminRoles(string adminRoles)
        {
            return _Context.ExecuteFunction("spUpdateAdminRoles", new ObjectParameter("adminRoles", adminRoles)) > 0;
        }

        public void spSyncUserDepartment(string userDepartment)
    	{
    		ObjectParameter userDepartmentParameter = userDepartment != null ? new ObjectParameter("userDepartment", userDepartment) : new ObjectParameter("userDepartment", typeof(string));
    		_Context.ExecuteFunction("spSyncUserDepartment", userDepartmentParameter);
    	}
    
    	public void spUpdateTrainingUsersJoinState(Nullable<int> trainingId, string userJoinStatusEntities, string lastUpdateUser)
    	{
    		ObjectParameter trainingIdParameter = trainingId.HasValue ? new ObjectParameter("trainingId", trainingId) : new ObjectParameter("trainingId", typeof(int));
    		ObjectParameter userJoinStatusEntitiesParameter = userJoinStatusEntities != null ? new ObjectParameter("userJoinStatusEntities", userJoinStatusEntities) : new ObjectParameter("userJoinStatusEntities", typeof(string));
    		ObjectParameter lastUpdateUserParameter = lastUpdateUser != null ? new ObjectParameter("lastUpdateUser", lastUpdateUser) : new ObjectParameter("lastUpdateUser", typeof(string));
    		_Context.ExecuteFunction("spUpdateTrainingUsersJoinState", trainingIdParameter, userJoinStatusEntitiesParameter, lastUpdateUserParameter);
    	}

        #endregion

        public void SetEntryState<T>(T entry, EntityState state) where T : Entity
        {
            this.Entry(entry).State = state;
        }
    }
}



