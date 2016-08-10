namespace Sleemon.Data
{
    using System.Collections.Generic;

    public class QuestionnaireDetailModel : QuestionnaireBasicModel
    {
        public List<QuestionnaireItemModel> Questions { get; set; }
    }

    public class QuestionnaireItemModel
    {
        public short No { get; set; }

        public string Question { get; set; }

        public string Image { get; set; }

        public byte Category { get; set; }

        public double Rate { get; set; }

        public List<QuestionnaireChoiceModel> Choices { get; set; }
    }

    public class QuestionnaireChoiceModel
    {
        public byte Choice { get; set; }

        public string Description { get; set; }

        public string Image { get; set; }
    }
}
