namespace Sleemon.Data
{
    using System.Collections.Generic;

    public class CourseDetailModel
    {
        public string Subject { get; set; }

        public string Description { get; set; }

        public int Star { get; set; }

        public int ForLevel { get; set; }

        public IList<ChapterPreviewModel> Chapters { get; set; }
    }

    public class ChapterPreviewModel
    {
        public string Title { get; set; }

        public IList<LearningFilePreviewModel> LearningFiles { get; set; }
    }

    public class LearningFilePreviewModel
    {
        public int LearningFileId { get; set; }

        public string Subject { get; set; }
    }
}
