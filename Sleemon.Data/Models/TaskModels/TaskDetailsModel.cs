namespace Sleemon.Data
{
    using System;
    using System.Collections.Generic;

    public class TaskDetailsModel : TaskBasicModel
    {
        public List<string> UserIds { get; set; }

        public string DispatchSubject { get; set; }

        public byte DispatchPriority { get; set; }

        public byte DispatchType { get; set; }

        public DateTime? DispatchTime { get; set; }

        public List<ExamDetailModel> Exams { get; set; }

        public List<QuestionnaireDetailModel> Questionnaires { get; set; }

        public List<LearningFileModel> LearningFiles { get; set; }

        public List<int> SceneIds { get; set; }
    }
}