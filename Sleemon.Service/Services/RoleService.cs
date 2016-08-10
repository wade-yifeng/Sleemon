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

    public class RoleService : IRoleService
    {
        private readonly ISleemonEntities _invoicingEntities;

        public RoleService()
        {
            this._invoicingEntities = new SleemonEntities();
        }

        /// <summary>
        /// 获取用户角色列表
        /// </summary>
        /// <param name="userUniqueId"></param>
        /// <returns></returns>
        public IList<UserRole> GetRoleListByUserUniqueId(string userUniqueId)
        {
            var list = from ur in this._invoicingEntities.UserRole
                       where ur.UserUniqueId == userUniqueId &&ur.IsActive==true
                       select ur;
            return list.ToList();
        }
        /// <summary>
        /// 获取用户角色列表
        /// </summary>
        /// <param name="userUniqueId"></param>
        /// <returns></returns>
        public IList<Role> GetAllRoleList()
        {
            var list = from r in this._invoicingEntities.Role
                       where  r.IsActive == true
                       select r;
           
            return list.ToList();
        }
        public IList<Role> GetRoleList(string roleName,int pageIndex,int pageSize,out int totalCount)
        {
            totalCount = 0;
            var list = from r in this._invoicingEntities.Role
                       where r.IsActive == true
                       select r;
            if (!string.IsNullOrEmpty(roleName))
            {
                list = list.Where(p => p.Name.Contains(roleName));
            }
            if (list == null)
            {
                return null;
            }
            totalCount = list.ToList().Count;
            list = list.OrderByDescending(p => p.LastUpdateTime).Take(pageSize * pageIndex).Skip(pageSize * (pageIndex - 1));
            return list.ToList();
        }
        /// <summary>
        /// add Role
        /// </summary>
        /// <param name="userUniqueId"></param>
        /// <returns></returns>
        public ResultBase AddRole(Role role)
        {
            ResultBase result = new ResultBase();
           Role newRole=this._invoicingEntities.Role.Add(role);
           int res= this._invoicingEntities.SaveChanges();
            if (res > 0)
            {
                result.IsSuccess = true;
                result.Message = "添加成功";
            }
            else
            {
                result.IsSuccess = false;
                result.Message = "添加失败";
            }
            return result;
        }
        /// <summary>
        /// update Role
        /// </summary>
        /// <param name="userUniqueId"></param>
        /// <returns></returns>
        public ResultBase UpdateRole(Role role)
        {
            ResultBase result = new ResultBase();
            var roleEntity = this._invoicingEntities.Role.FirstOrDefault(p => p.IsActive && p.Id == role.Id);
            if (roleEntity == null)
            {
                result.IsSuccess = false;
                result.Message = "更新失败";
                return result;
            }
            roleEntity.IsActive = true;
            roleEntity.LastUpdateTime = role.LastUpdateTime;
            roleEntity.LastUpdateUser = role.LastUpdateUser;
            roleEntity.Name = role.Name;
            roleEntity.RolePermission = role.RolePermission;
            int res = this._invoicingEntities.SaveChanges();
            if (res > 0)
            {
                result.IsSuccess = true;
                result.Message = "更新成功";
            }
            else
            {
                result.IsSuccess = false;
                result.Message = "更新失败";
            }
            return result;
        }
        /// <summary>
        /// delete Role
        /// </summary>
        /// <param name="userUniqueId"></param>
        /// <returns></returns>
        public ResultBase DeleteRole(int roleid,string userUniqueId)
        {
            //删除role
            var roleEntity = this._invoicingEntities.Role.FirstOrDefault(p => p.IsActive && p.Id == roleid);

            if (roleEntity == null)
                return new ResultBase()
                {
                    IsSuccess = false,
                    Message = "删除失败"
                };

            roleEntity.IsActive = false;
            roleEntity.LastUpdateTime = DateTime.UtcNow;
            roleEntity.LastUpdateUser = userUniqueId; 

            //删除rolepermission
            var rolePermission = this._invoicingEntities.RolePermission.Where(p => p.RoleId == roleid).ToList();
            for (int i = 0; i < rolePermission.Count; i++)
            {
                rolePermission[i].IsActive = false;
                rolePermission[i].LastUpdateTime = DateTime.UtcNow;
                rolePermission[i].LastUpdateUser = userUniqueId; 
            }

            this._invoicingEntities.SaveChanges();

            return new ResultBase()
            {
                IsSuccess = true,
                Message = "删除成功",
            };
        }

    }
}
