namespace Sleemon.Common
{
    using System.ComponentModel;

    public enum UserQuestionStatus
    {
        [Description("未解决")]
        NotResolved = 1,

        [Description("已解决")]
        Resolved = 2
    }
}
