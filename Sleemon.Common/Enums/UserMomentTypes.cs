namespace Sleemon.Common
{
    using System.ComponentModel;

    public enum UserMomentTypes
    {
        [Description("任务")]
        Task = 1,

        [Description("培训")]
        Training = 2,

        [Description("问卷调查")]
        Questionnaire = 3,

        [Description("考试")]
        Exam = 4,

        [Description("课程学习")]
        LearningTask = 4,
    }
}
