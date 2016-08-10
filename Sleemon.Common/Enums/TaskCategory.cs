namespace Sleemon.Common
{
    using System.ComponentModel;

    public enum TaskCategory
    {
        [Description("无")]
        None = 0,

        [Description("签到")]
        CheckIn = 1,

        [Description("课程")]
        Learning = 2,

        [Description("试卷")]
        Exam = 3,

        [Description("巡店")]
        StorePatrol = 4,

        [Description("问卷")]
        Questionnaire = 5
    }

    public enum SingleTaskCategory
    {
        [Description("课程学习")]
        Learning = 2,

        [Description("考试")]
        Exam = 3,

        [Description("巡店")]
        StorePatrol = 4,

        [Description("问卷调查")]
        Questionnaire = 5
    }
}
