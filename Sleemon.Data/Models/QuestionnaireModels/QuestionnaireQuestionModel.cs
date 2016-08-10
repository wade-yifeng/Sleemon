namespace Sleemon.Data
{
    using System.Collections.Generic;

    public class QuestionnaireQuestionModel
    {
        public int QuestionnaireItemId { get; set; }

        public int No { get; set; }

        public string Question { get; set; }

        public string Image { get; set; }

        public double Rate { get; set; }

        public byte Category { get; set; }

        public IList<QuestionnaireChoiceModel> Choices { get; set; }
    }
}
