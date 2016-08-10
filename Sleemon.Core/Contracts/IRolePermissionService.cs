namespace Sleemon.Core
{
    using System.Collections.Generic;

    using Sleemon.Data;

    public interface IRolePermissionService
    {
        ResultBase AddRolePermission(string roleName, string permissions, string currentUserUniqueId);
        ResultBase UpdateRolePermission(int roleid, string roleName, string permissions, string currentUserUniqueId);
        string GetRolePermission(int roleid);
    }
}