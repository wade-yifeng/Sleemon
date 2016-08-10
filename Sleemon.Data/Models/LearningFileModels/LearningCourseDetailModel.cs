namespace Sleemon.Data
{
    using System;
    using System.Collections.Generic;

    public class LearningCourseDetailModel : LearningCourseBasicModel
    {
        public List<LearningChapterDetailModel> Chapters { get; set; }
    }

    public class LearningChapterDetailModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public int No { get; set; }

        public string LastUpdateUserName { get; set; }

        public string LastUpdateUser { get; set; }

        public DateTime LastUpdateTime { get; set; }

        public List<LearningFileModel> Files { get; set; }
    }
}
