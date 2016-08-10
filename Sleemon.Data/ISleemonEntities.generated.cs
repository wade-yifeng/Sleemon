// Context interface. This file is auto-generated, do not modify this file.
using System;
using System.Data.Entity;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;

namespace Sleemon.Data
{
    public partial interface ISleemonEntities : IDisposable
    {
    	bool CanPreCompile { get; }
    	void Detach(object entity);
    	int SaveChanges();
    	void DiscardChanges();
        #region DbSet Properties
        DbSet<C__RefactorLog> C__RefactorLog { get; set; }
        DbSet<ConsEnterpriseNotice> ConsEnterpriseNotice { get; set; }
        DbSet<Department> Department { get; set; }
        DbSet<DepartmentEnterpriseNotice> DepartmentEnterpriseNotice { get; set; }
        DbSet<EnterpriseNotice> EnterpriseNotice { get; set; }
        DbSet<Exam> Exam { get; set; }
        DbSet<ExamChoice> ExamChoice { get; set; }
        DbSet<ExamQuestion> ExamQuestion { get; set; }
        DbSet<GroupSubTask> GroupSubTask { get; set; }
        DbSet<GroupTask> GroupTask { get; set; }
        DbSet<IllegalCharacters> IllegalCharacters { get; set; }
        DbSet<Keyword> Keyword { get; set; }
        DbSet<KnowledgeBase> KnowledgeBase { get; set; }
        DbSet<KnowledgeBaseKeyword> KnowledgeBaseKeyword { get; set; }
        DbSet<LearningChapter> LearningChapter { get; set; }
        DbSet<LearningCourse> LearningCourse { get; set; }
        DbSet<LearningFile> LearningFile { get; set; }
        DbSet<MessageDispatch> MessageDispatch { get; set; }
        DbSet<MessageReceiver> MessageReceiver { get; set; }
        DbSet<OrderShowFile> OrderShowFile { get; set; }
        DbSet<Permission> Permission { get; set; }
        DbSet<ProsComments> ProsComments { get; set; }
        DbSet<ProsEnterpriseNotice> ProsEnterpriseNotice { get; set; }
        DbSet<Questionnaire> Questionnaire { get; set; }
        DbSet<QuestionnaireChoice> QuestionnaireChoice { get; set; }
        DbSet<QuestionnaireItem> QuestionnaireItem { get; set; }
        DbSet<Role> Role { get; set; }
        DbSet<RolePermission> RolePermission { get; set; }
        DbSet<SleemonExceptionLog> SleemonExceptionLog { get; set; }
        DbSet<StorePatrol> StorePatrol { get; set; }
        DbSet<SystemConfig> SystemConfig { get; set; }
        DbSet<Task> Task { get; set; }
        DbSet<TaskExam> TaskExam { get; set; }
        DbSet<TaskLearning> TaskLearning { get; set; }
        DbSet<TaskQuestionnaire> TaskQuestionnaire { get; set; }
        DbSet<Training> Training { get; set; }
        DbSet<TrainingTask> TrainingTask { get; set; }
        DbSet<User> User { get; set; }
        DbSet<UserComments> UserComments { get; set; }
        DbSet<UserDailySignIn> UserDailySignIn { get; set; }
        DbSet<UserDepartment> UserDepartment { get; set; }
        DbSet<UserExamAnswer> UserExamAnswer { get; set; }
        DbSet<UserMoments> UserMoments { get; set; }
        DbSet<UserOrderShow> UserOrderShow { get; set; }
        DbSet<UserPointRecord> UserPointRecord { get; set; }
        DbSet<UserQuestion> UserQuestion { get; set; }
        DbSet<UserQuestionnaireAnswer> UserQuestionnaireAnswer { get; set; }
        DbSet<UserRole> UserRole { get; set; }
        DbSet<UserStorePatrol> UserStorePatrol { get; set; }
        DbSet<UserTask> UserTask { get; set; }
        DbSet<UserTraining> UserTraining { get; set; }

        #endregion

        #region Function Imports
        IEnumerable<spCommitEntireExam_Result> spCommitEntireExam(Nullable<int> userTaskId, string userId);
    
    	void spDeleteCourseById(Nullable<int> courseId);
    
    	void spDeleteExamById(Nullable<int> examId);
    
    	void spDeleteQuestionnaireById(Nullable<int> questionnaireId);
    
        IEnumerable<spGetBroadcastMessage_Result> spGetBroadcastMessage(Nullable<int> maxCount);
    
        IEnumerable<spGetStorePatrolList_Result> spGetStorePatrolList(Nullable<int> pageIndex, Nullable<int> pageSize, string userName, Nullable<System.DateTime> startFrom, Nullable<System.DateTime> endTo);
    
    	void spPointStorePatrol(Nullable<bool> isPass, string userStorePatrol);
    
    	void spSaveTrainingDetail(string training);
    
    	void spSyncDepartment(string department);

        void spSyncUser(string users);

        bool spUpdateAdminRoles(string adminRoles);

        void spSyncUserDepartment(string userDepartment);
    
    	void spUpdateTrainingUsersJoinState(Nullable<int> trainingId, string userJoinStatusEntities, string lastUpdateUser);

        #endregion

        Database Database { get; }
    
        void SetEntryState<T>(T entry, EntityState state) where T : Entity;
    }
}

