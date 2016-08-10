namespace Sleemon.Common
{
    using System.ComponentModel;

    public enum QuestionnaireQuestionCategory
    {
        [Description("单选题")]
        SingleOption = 0,

        [Description("多选题")]
        MultiOptions = 1,
                
        [Description("打分题")]
        Grade = 2
    }
}
