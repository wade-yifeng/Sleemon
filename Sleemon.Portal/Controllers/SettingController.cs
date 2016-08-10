using Sleemon.Core;
using Sleemon.Data;
using Sleemon.Portal.Common;
using Sleemon.Portal.Core;
using Sleemon.Portal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Sleemon.Portal.Controllers
{
    public class SettingController : BaseController
    {
        [HttpGet]
        public ActionResult Connect()
        {
            return View();
        }

        [HttpGet]
        public JsonContentAction GetDepartment()
        {
            var departments = base.ServiceClient.Request<IDepartmentService, IList<Data.DepartmentModel>>((service) => service.GetAllActivedDepartment());

            return new JsonContentAction(
                   new { records = departments, rootIds = SiteConfiguration.RootDepartments });
        }

        [HttpGet]
        public JsonContentAction GetUserListByDepartment(int departmentId, string query, int start, int length, string order)
        {
            int total = 0;
            var userList = base.ServiceClient.Request<IUserService, IList<Data.User>>((service) => service.GetUsersForDepartment(departmentId, query, start, length, order, out total));

            return new JsonContentAction(new { total = total, records = userList });
        }

        [HttpGet]
        public ActionResult Admin()
        {
            return View();
        }

        [HttpGet]
        public JsonContentAction GetAdminListWithRoles()
        {
            var userList = base.ServiceClient.Request<IUserService, IList<Data.AdminRolesModel>>((service) => service.GetAdminListWithRoles());

            return new JsonContentAction(userList);
        }

        public JsonContentAction GetAllRoleList()
        {
            var roleList = base.ServiceClient.Request<IRoleService, IList<Data.Role>>((service) => service.GetAllRoleList());

            return new JsonContentAction(roleList.Select(r => new Option { value = r.Id.ToString(), label = r.Name }));
        }

        [HttpPost]
        [ValidateJsonAntiForgeryToken]
        public ActionResult SetAdmin(string userUniqueId)
        {
            return new JsonContentAction(new
            {
                success = base.ServiceClient.Request<IUserService, bool>((service) => service.SetAdmin(userUniqueId))
            });
        }

        [HttpPost]
        [ValidateJsonAntiForgeryToken]
        public ActionResult SubmitAdminRoles(IEnumerable<Data.AdminRolesModel> admins, IEnumerable<Option> adminRoles)
        {
            var userRoles = admins.SelectMany(admin => adminRoles, (admin, role) => new AdminRoleModel
            {
                LastUpdateUser = UserUniqueId,
                RoleId = Convert.ToInt32(role.value),
                UserUniqueId = admin.UserUniqueId
            });

            return new JsonContentAction(new { success = base.ServiceClient.Request<IUserService, bool>((service) => service.UpdateAdminRoles(userRoles)) });
        }

        [HttpPost]
        [ValidateJsonAntiForgeryToken]
        public ActionResult UpdateAdminRoles(AdminRoleModel admin, IEnumerable<int> roles)
        {
            admin.LastUpdateUser = UserUniqueId;
            return new JsonContentAction(new { success = base.ServiceClient.Request<IUserService, bool>((service) => service.UpdateAdminRoles(admin, roles)) });
        }
    }
}