namespace Sleemon.Core
{
    using System.Collections.Generic;
    using Sleemon.Data;

    public interface IUserQuestionService
    {
        IList<UserQuestionDetailModel> GetAllUserQuestionsList();

        UserQuestionDetailModel GetUserQuestionById(int questionId);

        ResultBase SaveReplyAnswer(UserQuestionDetailModel answer);

        IList<UserQuestionPreviewModel> GetUserQuestionList(string userId, int pageIndex, int pageSize);

        UserQuestionBasicModel GetUserQuestionReplyDetail(string userId, int questionId);

        ResultBase CommitUserQuestion(string userId, string title, string question);
    }
}
