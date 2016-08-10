namespace Sleemon.Common
{
    using System.ComponentModel;

    public enum MsgDispatchType
    {
        [Description("无")]
        None = 0,

        [Description("立即发送")]
        Immediate = 1,

        [Description("延迟发送")]
        Delayed = 2
    }
}
