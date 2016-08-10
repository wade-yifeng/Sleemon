namespace Sleemon.Data
{
    using Newtonsoft.Json;

    public class UserListModel 
    {
        public User User { get; set; }
        public string DepartmentName { get; set; }
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public int pageIndex { get; set; }
        public int pageSize { get; set; }
        public int totalCount { get; set; }
    }
}
