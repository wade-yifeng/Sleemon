namespace Sleemon.WebApi
{
    using System.Collections.Generic;

    using Sleemon.Data;

    public class CommitQuestionnaireModel
    {
        public string UserId { get; set; }

        public int TaskId { get; set; }

        public IList<QuestionnaireAnswerModel> Answers { get; set; }
    }
}
