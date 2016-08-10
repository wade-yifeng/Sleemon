using Sleemon.Core;

namespace Sleemon.Portal.Controllers
{
    using Microsoft.Practices.Unity;
    /**
    *add by wolfgump 20160521
*/
    
    using System;
    using System.Web.Mvc;
    using System.Collections.Generic;
    using Sleemon.Data;
    using System.Linq;

    public class UserController : BaseController
    {
        /// <summary>
        /// 用户列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public PartialViewResult Index(int pageIndex, int pageSize, string departmentName,string userName)
        {
            ViewBag.departmentName= departmentName;
            ViewBag.userName = userName;
            ViewBag.pageIndex = pageIndex;
            ViewData["RoleList"] = GetRoleList();
            var userList = ServiceClient.Request<IUserService, IList<UserListModel>>(
                service => service.GetUserList(pageIndex, pageSize, departmentName, userName));
            return PartialView("UserList", userList);
        }
        /// <summary>
        /// 设置用户角色
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SetUserRole(string userUniqueid,int roleid)
        {
            if (this.User == null)
            {
                return Content("用户未登录");
            }
            if (roleid == 0)
            {
                return Content("请选择角色");
            }
            var res= ServiceClient.Request<IUserService, ResultBase>(
                service => service.SetUserRole(this.UserUniqueId, userUniqueid, roleid));
            if (res.IsSuccess)
            {
                return Content("设置成功！");
            }
            else
            {
                return Content("设置失败！请联系管理员");
            }
        }

        public List<SelectListItem> GetRoleList()
        {
            var roleSelect = new List<SelectListItem>();
            var dic = new Dictionary<int, string>();
            dic.Add(0, "--请选择角色--");
            var roleList= ServiceClient.Request<IRoleService, IList<Role>>(
                service => service.GetAllRoleList());

            for (var i = 0; i < roleList.Count; i++)
            {
                dic.Add(roleList[i].Id, roleList[i].Name);
            }   
            
            foreach (var item in dic)
            {
                roleSelect.Add(new SelectListItem
                {
                    Text = item.Value.ToString(),
                    Value = item.Key.ToString()
                });
            }
           
            return roleSelect;
        }
    }
}