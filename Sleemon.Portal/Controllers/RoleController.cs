using Sleemon.Core;
using Sleemon.Data;

namespace Sleemon.Portal.Controllers
{
    using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

    public class RoleController : BaseController
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Index()
        {
            ViewBag.pageIndex = 1;
            return PartialView("RoleList");
        }
        
        [HttpGet]
        public ActionResult RolePermission(int roleId,string roleName,string rolePermissions)
        {
            ViewBag.RoleId = roleId;
            ViewBag.RoleName = roleName;
            ViewBag.RolePermissions = rolePermissions;
            return PartialView("RolePermission");
        }
        
        [HttpGet]
        public ActionResult GetRoleList(string roleName,int pageIndex,int pageSize)
        {
            ViewBag.pageIndex = pageIndex;
            int totalCount = 0;
            RoleListModel list = new RoleListModel();
            var roleList =
                ServiceClient.Request<IRoleService, IList<Role>>(
                    service => service.GetRoleList(roleName, pageIndex, pageSize, out totalCount));
            List<RoleExtend> roleExtendList = new List<RoleExtend>();
            for (int i = 0; i < roleList.Count; i++)
            {
                RoleExtend roleExtend = new RoleExtend();
                roleExtend.role = roleList[i];
                roleExtend.permissions =
                    ServiceClient.Request<IRolePermissionService, string>(
                        service => service.GetRolePermission(roleList[i].Id));
                roleExtendList.Add(roleExtend);
            }
            list.listRole = roleExtendList;
            list.PageIndex = pageIndex;
            list.PageSize = pageSize;
            list.TotalCount = totalCount;

            return PartialView("RoleList", list);
        }
        
        [HttpGet]
        public ActionResult DeleteRole(int roleid)
        {
            var result = ServiceClient.Request<IRoleService, ResultBase>(
                        service => service.DeleteRole(roleid, UserUniqueId));
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        
        [HttpPost]
        public ActionResult UpdateRole(int roleid,string roleName,string permissions)
        {
            ResultBase result=new ResultBase();
            if (string.IsNullOrEmpty(roleName))
            {
                return Json(new ResultBase() {
                 IsSuccess=false,
                  Message="角色名称不能为空"

                });
            }
            if (roleid <= 0)
            {
                result = ServiceClient.Request<IRolePermissionService, ResultBase>(
                        service => service.AddRolePermission(roleName, permissions, UserUniqueId));
            }
            else
            {
                result = ServiceClient.Request<IRolePermissionService, ResultBase>(
                    service => service.UpdateRolePermission(roleid, roleName, permissions, UserUniqueId));
            }
            return Json(result);
        }
        
        //[HttpGet]
        //public ActionResult GetPermissionByParentId(int parentid)
        //{
        //    var list = ServiceClient.Request<IPermissionService, IList<Permission>>(
        //                service => service.GetPermissionByParentId(parentid));
        //    return Json(list, JsonRequestBehavior.AllowGet);
        //}
        //public ActionResult GetAllPermissionByParentId(int parentid)
        //{
        //    var list = ServiceClient.Request<IPermissionService, IList<Permission>>(
        //                service => service.GetAllPermissionByParentId(parentid));
        //    return Json(list, JsonRequestBehavior.AllowGet);
        //}
    }
}