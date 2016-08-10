namespace Sleemon.Service
{
    using System;
    using System.Linq;
    using System.Data.SqlClient;
    using System.Collections.Generic;

    using Microsoft.Practices.Unity;

    using Sleemon.Core;
    using Sleemon.Data;
    using Sleemon.Common;
    using System.Transactions;

    public class RolePermissionService : IRolePermissionService
    {
        private readonly ISleemonEntities _invoicingEntities;
        private readonly IRoleService roleService;
        public RolePermissionService([Dependency] IRoleService roleService)
        {
            this._invoicingEntities = new SleemonEntities();
            this.roleService = roleService;
        }

        /// <summary>
        /// 添加角色权限
        /// </summary>
        /// <param name="userUniqueId"></param>
        /// <returns></returns>
        public ResultBase AddRolePermission(string roleName, string permissions, string currentUserUniqueId)
        {
            var result = new ResultBase()
            {
                IsSuccess = true,
                StatusCode = (int)StatusCode.Success,
                Message="添加权限成功"
            };
             //add role
            Role role = new Role();
            role.IsActive = true;
            role.LastUpdateTime = DateTime.UtcNow;
            role.LastUpdateUser = currentUserUniqueId;
            role.Name = roleName;
            Role newRole=this._invoicingEntities.Role.Add(role);
            //add rolepermission
            string[] permissionArray = permissions.Split(',');
            for (int i = 0; i < permissionArray.Length; i++)
            {
                if (!string.IsNullOrEmpty(permissionArray[i]))
                {
                    RolePermission rp=this._invoicingEntities.RolePermission.Create();
                    rp.RoleId =newRole.Id;
                    rp.PermissionId=Convert.ToInt32(permissionArray[i]);
                    rp.IsActive = true;
                    rp.LastUpdateTime = DateTime.UtcNow;
                    rp.LastUpdateUser = currentUserUniqueId;
                    this._invoicingEntities.RolePermission.Add(rp);
                }
            }
           int resultDb=this._invoicingEntities.SaveChanges();
           if (resultDb <= 0)
           {
               result.IsSuccess = false;
               result.Message="添加权限失败";
           }
           return result;   
        }

        public ResultBase UpdateRolePermission(int roleid, string roleName, string permissions, string currentUserUniqueId)
        {
              var result = new ResultBase()
            {
                IsSuccess = true,
                StatusCode = (int)StatusCode.Success,
                Message="更新权限成功"
            };
            int resultDb=0;
            //更新角色表
            Role role = this._invoicingEntities.Role.FirstOrDefault(p=>p.Id==roleid);
            role.IsActive = true;
            role.LastUpdateTime = DateTime.UtcNow;
            role.LastUpdateUser = currentUserUniqueId;
            role.Name = roleName;
           //先删除角色权限关系表
            var rolePermission = this._invoicingEntities.RolePermission.Where(p=>p.RoleId==roleid).ToList();
            for (int i = 0; i < rolePermission.Count; i++)
            {
                this._invoicingEntities.RolePermission.Remove(rolePermission[i]);
            }
            //再添加删除角色权限关系表
            
            string[] permissionArray = permissions.Split(',');
            for (int i = 0; i < permissionArray.Length; i++)
            {
                if (!string.IsNullOrEmpty(permissionArray[i]))
                {
                    RolePermission rp = this._invoicingEntities.RolePermission.Create();
                    rp.RoleId = roleid;
                    rp.PermissionId = Convert.ToInt32(permissionArray[i]);
                    rp.IsActive = true;
                    rp.LastUpdateTime = DateTime.UtcNow;
                    rp.LastUpdateUser = currentUserUniqueId;
                    this._invoicingEntities.RolePermission.Add(rp);
                }
            }
            resultDb = this._invoicingEntities.SaveChanges();
            
            if (resultDb <= 0 )
            {
                result.IsSuccess = false;
                result.Message = "更新权限失败";
            }
            return result;
        }
        //获得一个角色的所有权限id,多个id用逗号隔开
        public string GetRolePermission(int roleid)
        {
            string permissions = "";
            var rolePermissions = from rp in this._invoicingEntities.RolePermission
                                  where rp.RoleId == roleid
                                  select rp.PermissionId;

            permissions = string.Join(",", rolePermissions);
            return ","+permissions+",";
        }
    }
}
