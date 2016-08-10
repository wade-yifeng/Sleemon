namespace Sleemon.Core
{
    using System.Collections.Generic;

    using Sleemon.Data;

    public interface IPermissionService
    {
        IList<Permission> GetAllPermission();
        IList<Permission> GetPermissionByUser(string userUnqiueId);
    }
}