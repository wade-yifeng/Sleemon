namespace Sleemon.Core
{
    using System.Collections.Generic;

    using Sleemon.Data;

    public interface IRoleService
    {
        IList<UserRole> GetRoleListByUserUniqueId(string userUniqueId);
        IList<Role> GetAllRoleList();
        IList<Role> GetRoleList(string roleName, int pageIndex, int pageSize, out int totalCount);
        ResultBase AddRole(Role role);
        ResultBase UpdateRole(Role role);
        ResultBase DeleteRole(int roleid, string userUniqueId);
       }
}