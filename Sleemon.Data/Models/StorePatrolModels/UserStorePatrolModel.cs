namespace Sleemon.Data
{
    public class UserStorePatrolModel
    {
        public int UserStorePatrolId { get; set; }

        public int StorePatrolId { get; set; }

        public int SceneId { get; set; }

        public string SceneName { get; set; }

        public string PicPath { get; set; }

        public string Desc { get; set; }

        public double AdminRate { get; set; }

        public string AdminComment { get; set; }
    }
}
