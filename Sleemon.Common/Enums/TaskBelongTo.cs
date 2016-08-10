namespace Sleemon.Common
{
    using System.ComponentModel;

    public enum TaskBelongTo
    {
        [Description("无")]
        None = 0,

        [Description("单项任务")]
        SingleTask = 1,

        [Description("培训任务")]
        TrainingTask = 2,

        [Description("任务组任务")]
        GroupTask = 3
    }
}
