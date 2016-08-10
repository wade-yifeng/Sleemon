namespace Sleemon.Core
{
    using System.Collections.Generic;

    using Sleemon.Data;

    public interface ILearningFileService
    {
        IList<LearningCourseListModel> GetCourseList(int pageIndex, int pageSize, string courseTitle);

        IList<LearningCourseDetailModel> GetCourseList();

        LearningCourseDetailModel GetCourseDetailById(int courseId);

        ResultBase SaveCourseDetail(LearningCourseDetailModel course);

        ResultBase DeleteCourseById(int courseId);

        IList<LearningFileListModel> GetLearningFileList(int pageIndex, int pageSize, string fileTitle);

        IList<LearningFileListModel> GetLearningFileList();

        LearningFileModel GetLearningFileById(int fileId);

        IList<CoursePreviewModel> GetCoursesList();

        CourseDetailModel GetCourseDetail(int courseId, string keyword);

        CourseLearningFileModel GetCourseLearningFile(int learningFileId, int taskId);

        ResultBase FinishCourseLearningTask(string userUniqueId, int taskId);
    }
}
