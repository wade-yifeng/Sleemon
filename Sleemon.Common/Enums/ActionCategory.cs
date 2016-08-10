namespace Sleemon.Common
{
    using System.ComponentModel;

    public enum ActionCategory
    {
        [Description("无")]
        None = 0,

        [Description("已保存")]
        Save = 1,

        [Description("已发布")]
        Publish = 2,

        [Description("已确认报名")]
        Confirmed = 3
    }
}
