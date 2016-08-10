namespace Sleemon.Common
{
    using System.ComponentModel;

    public enum FileType
    {
        [Description("无")]
        None = 0,

        [Description("Word")]
        Word = 1,

        [Description("视频")]
        Video = 2,

        [Description("PPT")]
        PPT = 3,

        [Description("Pdf")]
        Pdf = 4,

        [Description("Excel")]
        Excel = 5
    }

    public enum FileTypeOption
    {
        [Description("Word")]
        Word = 1,

        [Description("视频")]
        Video = 2
    }
}
