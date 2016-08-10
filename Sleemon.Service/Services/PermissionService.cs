namespace Sleemon.Service
{
    using System.Linq;
    using System.Collections.Generic;

    using Sleemon.Core;
    using Sleemon.Data;

    public class PermissionService : IPermissionService
    {
        private readonly ISleemonEntities _invoicingEntities;
        public PermissionService()
        {
            this._invoicingEntities = new SleemonEntities();
        }

        public IList<Permission> GetAllPermission()
        {
            var list = from p in this._invoicingEntities.Permission
                       where p.IsActive == true && p.IsMenu == true
                       select p;

            return list.ToList();
        }

        public IList<Permission> GetPermissionByUser(string userUnqiueId)
        {
            return (from role in this._invoicingEntities.UserRole
                        join rolePermission in this._invoicingEntities.RolePermission on role.Id equals rolePermission.RoleId
                            join permission in this._invoicingEntities.Permission on rolePermission.PermissionId equals permission.Id
                                where role.UserUniqueId == userUnqiueId && role.IsActive == true && rolePermission.IsActive == true && permission.IsActive == true
                                    select permission).ToList();
        }
    }
}
