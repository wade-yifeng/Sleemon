namespace Sleemon.Common
{
    using System.ComponentModel;

    public enum ExamQuestionCategory
    {
        [Description("单选题")]
        SingleOption = 0,

        [Description("多选题")]
        MultiOptions = 1,

        [Description("主观题")]
        Subjective = 2
    }
}
