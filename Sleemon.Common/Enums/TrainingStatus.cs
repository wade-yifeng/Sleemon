namespace Sleemon.Common
{
    using System.ComponentModel;

    public enum TrainingStatus
    {
        [Description("无")]
        None = 0,

        [Description("未发布")]
        NotPublished = 1,

        [Description("已发布")]
        Published = 2,

        [Description("已确认")]
        Confirmed = 3
    }
}
