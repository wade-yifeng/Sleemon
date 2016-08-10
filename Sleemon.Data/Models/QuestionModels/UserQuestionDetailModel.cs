namespace Sleemon.Data
{
    public class UserQuestionDetailModel: UserQuestionBasicModel
    {
        public int QuestionId { get; set; }

        public string User { get; set; }

        public string UserName { get; set; }

        public string UserAvatar { get; set; }

        public string Replier { get; set; }

        public byte Status { get; set; }
    }
}
