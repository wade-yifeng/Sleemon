namespace Sleemon.Core
{
    using System.Collections.Generic;

    using Sleemon.Data;

    public interface ITaskService
    {
        IList<UserTaskModel> SearchUserTaskList(string userUnqiueId, byte listType, string input);

        IEnumerable<UserTaskInfo> GetUserTaskList(string userId, byte listType, int pageIndex, int pageSize);

        IList<TaskListModel> GetTaskList(TaskSearchContext search);

        IList<TaskListModel> GetTaskList(int belongTo);

        TaskDetailsModel GetTaskDetailById(int taskId);

        UserTaskDetailsModel GetUserTaskDetail(int userTaskId);

        ExamPreviewModel GetExamTaskOtherInfo(int taskId);

        IList<ExamQuestionModel> GetExamQuestions(int taskId);

        IList<ExamAnswerModel> GetUserExamAnswers(int taskId, string userUniqueId);

        ResultBase CommitSingleExamQuestion(int taskId, string userUniqueId, int examQuestionId, string myAnswer);

        ExamResultModel CommitEntireExam(string userUniqueId, int userTaskId);

        ResultBase SaveTaskDetail(TaskDetailsModel taskModel);

        ResultBase DeleteTaskById(int taskId);

        int GetQuestionnaireQuestionCount(int taskId);

        ResultBase UpdateUserTaskStatus(int userTaskId, byte userTaskCategory);

        byte GetUserTaskStatus(int userTaskId);
    }
}
