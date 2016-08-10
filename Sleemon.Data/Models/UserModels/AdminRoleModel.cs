namespace Sleemon.Data
{
    public class AdminRoleModel
    {
        public string UserUniqueId { get; set; }

        public int RoleId { get; set; }

        public bool IsActive { get; set; }

        public string LastUpdateUser { get; set; }
    }
}
