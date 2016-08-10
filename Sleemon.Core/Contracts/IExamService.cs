namespace Sleemon.Core
{
    using Sleemon.Data;
    using System.Collections.Generic;

    public interface IExamService
    {
        IList<ExamListModel> GetExamList();

        ExamDetailModel GetExamDetailById(int id);

        ResultBase SaveExamDetail(ExamDetailModel model);

        ResultBase DeleteExamById(int id);
    }
}