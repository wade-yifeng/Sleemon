namespace Sleemon.Data
{
    using System.Collections.Generic;

    public class QuestionnaireQuestionPreviewModel
    {
        public int ExamQuestionId { get; set; }

        public IList<QuestionnaireQuestionChoicePreviewModel> Choices { get; set; }
    }

    public class QuestionnaireQuestionChoicePreviewModel
    {
        public int Choice { get; set; }

        public int Count { get; set; }
    }
}
