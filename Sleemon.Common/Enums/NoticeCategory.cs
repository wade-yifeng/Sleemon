using System.ComponentModel;
namespace Sleemon.Common
{
    public enum NoticeType
    {
        [Description("资讯")]
        Information = 1,
        [Description("推广")]
        Activity = 2
    }

    public enum NoticeCategory
    {
        [Description("置顶 (加载到资讯列表最顶端)")]
        Top = 1,
        [Description("滚动 (首页顶部滚动图片)")]
        Scroll = 2,
        [Description("普通 (加载到资讯列表)")]
        Normal = 3
    }
}
