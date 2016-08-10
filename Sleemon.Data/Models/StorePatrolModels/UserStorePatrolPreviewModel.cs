namespace Sleemon.Data
{
    public class UserStorePatrolPreviewModel: TaskBasicModel
    {
        public bool IsStarted { get; set; }

        public bool IsPass { get; set; }

        public string UserName { get; set; }
        public int UserTaskId { get; set; }
    }
}
