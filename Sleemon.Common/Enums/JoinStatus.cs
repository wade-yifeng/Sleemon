namespace Sleemon.Common
{
    using System.ComponentModel;

    public enum JoinStatus
    {
        None = 0,

        [Description("报名中")]
        Joining = 1,

        [Description("报名通过")]
        Approved = 2,

        [Description("报名驳回")]
        Rejected = 3
    }
}
