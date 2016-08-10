namespace Sleemon.Common
{
    using System.ComponentModel;

    public enum KnowledgeCategory
    {
        [Description("无")]
        None = 0,

        [Description("陈列")]
        Exhibition = 1,

        [Description("销售")]
        Sales = 2
    }
}
