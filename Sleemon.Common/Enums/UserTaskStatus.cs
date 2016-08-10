namespace Sleemon.Common
{
    using System.ComponentModel;

    public enum UserTaskStatus
    {
        [Description("未开始")]
        NotStarted = 1,

        [Description("进行中")]
        Ongoing = 2,

        [Description("已提交待审核")]
        Submitted = 3,

        [Description("已结束")]
        Completed = 4,

        [Description("未合格")]
        NotPassed = 5
    }
}
