namespace Sleemon.Data
{
    using System;

    public class UserQuestionBasicModel
    {
        public string Title { get; set; }

        public string Question { get; set; }

        public DateTime? AskTime { get; set; }

        public string ReplierAvater { get; set; }

        public string ReplierName { get; set; }

        public DateTime? AnswerTime { get; set; }

        public string Answer { get; set; }
    }
}
