namespace Sleemon.Service
{
    using System;
    using System.Linq;
    using System.Data.Entity;
    using System.Data.SqlClient;
    using System.Collections.Generic;

    using Sleemon.Core;
    using Sleemon.Data;
    using Sleemon.Common;
    using System.Transactions;
    public class UserService : IUserService
    {
        private readonly ISleemonEntities _invoicingEntities;

        public UserService()
        {
            this._invoicingEntities = new SleemonEntities();
        }

        public SyncResult SyncUser(IEnumerable<UserProfile> users)
        {
            var userList = users.ToList();
            var result = new SyncResult()
            {
                IsSuccess = true,
                Quantity = userList.Count,
                StatusCode = (int)StatusCode.Success
            };

            var userDepartment = (from user in userList
                                  from department in user.Department
                                  select new UserDepartmentSyncModel() { DepartmentId = department, UserId = user.UserId }).ToList();

            this._invoicingEntities.spSyncUser(Utilities.GetXElementFromObject(userList).ToString());

            this._invoicingEntities.spSyncUserDepartment(Utilities.GetXElementFromObject(userDepartment).ToString());

            return result;
        }

        public UserViewModel GetUserInfoById(string userId)
        {
            var user = this._invoicingEntities.User.FirstOrDefault(p => p.IsActive && p.UserUniqueId.Equals(userId));

            if (user == null) return null;

            return new UserViewModel()
            {
                UserId = user.UserUniqueId,
                Name = user.Name,
                Avatar = user.Avatar,
                Mobile = user.Mobile,
                Grade = user.Grade,
                Point = user.Point,
                ProductAbility = user.ProductAbility,
                SalesAbility = user.SalesAbility,
                ExhibitAbility = user.ExhibitAbility,
                IsAdmin = user.IsAdmin,
                IsSuperAdmin = user.IsSuperAdmin
            };
        }

        public User GetUserById(string userId)
        {
            return this._invoicingEntities.User.FirstOrDefault(p => p.IsActive && p.UserUniqueId == userId);
        }

        public IList<User> GetUsersForDepartment(int departmentId, string query, int start, int length, string order, out int total)
        {
            var queryString = string.IsNullOrWhiteSpace(query) ? "" : string.Format(@"AND ([User].[UserUniqueId] LIKE '%{0}%' OR [User].[Name] LIKE '%{0}%' OR [User].[Mobile] LIKE '%{0}%' OR [User].[Position] LIKE '%{0}%')", query);
            total = this._invoicingEntities.Database.SqlQuery<int>(string.Format(@"
WITH [Departments] AS
(
	SELECT [Department].[ParentId]
          ,[Department].[UniqueId]
	FROM [dbo].[Department]
	WHERE [Department].[IsActive] = 1
		AND [Department].[UniqueId] = @departmentId

	UNION ALL

	SELECT [Department].[ParentId]
          ,[Department].[UniqueId]
	FROM [dbo].[Department]
	JOIN [Departments]
		ON [Departments].[UniqueId] = [Department].[ParentId]
	WHERE [Department].[IsActive] = 1
)  
	SELECT 1
	FROM [dbo].[UserDepartment]
	JOIN [Departments]
		ON [Departments].[UniqueId] = [UserDepartment].[DepartmentUniqueId]
	JOIN [dbo].[User]
		ON [User].[UserUniqueId] = [UserDepartment].[UserUniqueId]
	WHERE [User].[IsActive] = 1 {0}
", queryString), new SqlParameter("@departmentId", departmentId)).Count();

            return this._invoicingEntities.Database.SqlQuery<User>(string.Format(@"
WITH [Departments] AS
(
	SELECT [Department].[ParentId]
          ,[Department].[UniqueId]
	FROM [dbo].[Department]
	WHERE [Department].[IsActive] = 1
		AND [Department].[UniqueId] = @departmentId

	UNION ALL

	SELECT [Department].[ParentId]
          ,[Department].[UniqueId]
	FROM [dbo].[Department]
	JOIN [Departments]
		ON [Departments].[UniqueId] = [Department].[ParentId]
	WHERE [Department].[IsActive] = 1
)
SELECT *
FROM (    
	SELECT [User].*, ROW_NUMBER() OVER(ORDER BY [User].{0}, [User].[LastUpdateTime] DESC) AS [Row]
	FROM [dbo].[UserDepartment]
	JOIN [Departments]
		ON [Departments].[UniqueId] = [UserDepartment].[DepartmentUniqueId]
	JOIN [dbo].[User]
		ON [User].[UserUniqueId] = [UserDepartment].[UserUniqueId]
	WHERE [User].[IsActive] = 1 {1}
) AS [Result] WHERE [Row] BETWEEN @start AND @start + @length", order, queryString),
new SqlParameter("@departmentId", departmentId),
new SqlParameter("@start", start),
new SqlParameter("@length", length)).ToList();
        }

        public UserDailySignInModel UserDailySignIn(string userId)
        {
            var signPoint = 0; //TODO: Get sign point from SystemConfig
            var currentPoint = 0; //TODO: Get user current total point.

            var dailySignInModel = new UserDailySignInModel()
            {
                UserId = userId,
                Point = currentPoint,
                SignInDate = DateTime.Now
            };

            if (this._invoicingEntities.Database.SqlQuery<UserDailySignIn>(@"
SELECT *
FROM [dbo].[UserDailySignIn]
WHERE [UserDailySignIn].[UserUniqueId] = @userId
    AND [UserDailySignIn].[SignInDate] = @currentDate
    AND [UserDailySignIn].[IsActive] = 1",
    new SqlParameter("@userId", userId),
    new SqlParameter("currentDate", DateTime.Now.ToString("yyyy-MM-dd"))).Any())
            {
                dailySignInModel.StatusCode = (int)StatusCode.SignedIn;
                dailySignInModel.Message = "Oops, you already signed in today.";
            }
            else
            {
                var entity = this._invoicingEntities.UserDailySignIn.Create();

                entity.UserUniqueId = userId;
                entity.SignInDate = DateTime.Now;
                entity.LastUpdateTime = DateTime.Now;
                entity.IsActive = true;

                this._invoicingEntities.UserDailySignIn.Add(entity);

                this._invoicingEntities.SaveChanges();

                dailySignInModel.Point += signPoint;
                dailySignInModel.StatusCode = (int)StatusCode.Success;
                dailySignInModel.Message = string.Format("congragulations! you got {0} points", signPoint);
            }

            return dailySignInModel;
        }

        public User GetUserByUniqueId(string userUniqueId)
        {
            return this._invoicingEntities.User.FirstOrDefault(p => p.IsActive && p.UserUniqueId == userUniqueId);
        }

        public User Login(string userUniqueId, string password)
        {
            return
                this._invoicingEntities.User.FirstOrDefault(
                    p => p.IsActive && p.UserUniqueId == userUniqueId && p.Password == password);
        }

        public bool ResetPassword(string userId, string password)
        {
            var entity = this._invoicingEntities.User.FirstOrDefault(p => p.IsActive && p.UserUniqueId == userId);

            if (entity == null) return false;

            entity.Password = password;
            entity.IsOriginalPassword = false;

            this._invoicingEntities.SaveChanges();

            return true;
        }

        public IList<AdminRolesModel> GetAdminListWithRoles()
        {
            //var adminList = 
            return null;
                        //.ToLookup(t => t.User).Select(columnMap => 
                        //    new AdminRolesModel
                        //    {
                        //        Avatar = columnMap.Key.Avatar,
                        //        Mobile = columnMap.Key.Mobile,
                        //        Name = columnMap.Key.Name,
                        //        Position = columnMap.Key.Position,
                        //        Status = columnMap.Key.Status,
                        //        UserUniqueId = columnMap.Key.UserUniqueId,
                        //        Roles = string.Join(" | ", columnMap.Select(r => r.Role != null ? r.Role.Name : string.Empty)),
                        //        RoleIds = string.Join(",", columnMap.Select(r => r.Role != null ? r.Role.Id.ToString(): string.Empty))
                        //    }).ToList();
        }

        public ResultBase SetUserRole(string currentUserId, string userUniqueid, int roleid)
        {
            var result = new ResultBase();

            var role = from r in this._invoicingEntities.Role
                       where r.Id == roleid
                       select r;
            if (role == null)
            {
                LogHelper<UserService>.WriteInfo("权限不存在，权限Id:" + roleid);
                result.IsSuccess = false;
                return result;
            }
            var userRoleByUser = from ur in this._invoicingEntities.UserRole
                                 where ur.UserUniqueId == userUniqueid
                                 select ur;

            var userRole = this._invoicingEntities.UserRole.Create();
            userRole.IsActive = true;
            userRole.UserUniqueId = userUniqueid;
            userRole.RoleId = roleid;
            userRole.LastUpdateTime = DateTime.UtcNow;
            userRole.LastUpdateUser = currentUserId;
            if (userRoleByUser.ToList().Count > 0)
            {
                this._invoicingEntities.UserRole.Remove(userRoleByUser.ToList()[0]);
            }

            this._invoicingEntities.UserRole.Add(userRole);

            int res = this._invoicingEntities.SaveChanges();
            result.IsSuccess = res > 0;
            return result;
        }

        public ResultBase UpdateUserProfile(UserProfile userProfile)
        {
            var result = new ResultBase()
            {
                IsSuccess = true,
                StatusCode = (int)StatusCode.Success
            };

            var userEntity =
                this._invoicingEntities.User.FirstOrDefault(p => p.IsActive && p.UserUniqueId == userProfile.UserId);

            if (userEntity != null)
            {
                userEntity.Name = userProfile.Name;
                userEntity.Position = userProfile.Position;
                userEntity.Mobile = userProfile.Mobile;
                userEntity.Gender = userProfile.Gender == 1;
                userEntity.Email = userProfile.Email;
                userEntity.WeixinId = userProfile.WeixinId;
                userEntity.Avatar = userProfile.Avatar;
                userEntity.Country = userProfile.Country;
                userEntity.Province = userProfile.Province;
                userEntity.City = userProfile.City;
                userEntity.District = userProfile.District;

                this._invoicingEntities.SaveChanges();
            }
            else
            {
                result.IsSuccess = false;
                result.Message = string.Format("Cannot find user by UserId: {0}", userProfile.UserId);
                result.StatusCode = (int)StatusCode.Failed;
            }

            return result;
        }

        public bool IsOriginalPassword(string userId)
        {
            return
                this._invoicingEntities.User.FirstOrDefault(p => p.IsActive && p.UserUniqueId == userId)
                    .IsOriginalPassword;
        }

        public bool SetAdmin(string userUniqueId)
        {
            var user = this._invoicingEntities.User.FirstOrDefault(u => u.UserUniqueId == userUniqueId && u.IsActive);
            if (user == null) {
                return false;
            }

            user.IsAdmin = true;
            user.LastUpdateTime = DateTime.Now;
            return this._invoicingEntities.SaveChanges() > 0;
        }

        public bool UpdateAdminRoles(IEnumerable<AdminRoleModel> adminRoles)
        {
            return this._invoicingEntities.spUpdateAdminRoles(Utilities.GetXElementFromObject(adminRoles.ToList()).ToString());
        }

        public bool UpdateAdminRoles(AdminRoleModel admin, IEnumerable<int> roles)
        {
            var adminRoles = from ur in this._invoicingEntities.UserRole
                                 where ur.UserUniqueId == admin.UserUniqueId && ur.IsActive == true
                                 select ur;

            foreach (var role in adminRoles)
            {
                role.IsActive = false;
            }

            this._invoicingEntities.UserRole.AddRange(roles.Select(r => new UserRole
            {
                IsActive = true,
                LastUpdateTime = DateTime.Now,
                LastUpdateUser = admin.LastUpdateUser,
                RoleId = r,
                UserUniqueId = admin.UserUniqueId
            }));

            return this._invoicingEntities.SaveChanges() > 0;
        }

        public IList<UserPointRecordPreviewModel> GetUserPointRecordList(string userId, int pageIndex, int pageSize)
        {
            return
                this._invoicingEntities.UserPointRecord.Where(p => p.UserUniqueId == userId)
                    .OrderByDescending(p => p.LastUpdateTime)
                    .Skip((pageIndex - 1) * pageSize)
                    .Take(pageIndex)
                    .Select(p => new UserPointRecordPreviewModel()
                    {
                        Description = p.Description,
                        Time = p.LastUpdateTime,
                        Operator = p.Operator,
                        Point = p.Point
                    })
                    .ToList();
        }

        public IList<UserMomentPreviewModel> GetUserMomentsList(string userId, int pageIndex, int pageSize)
        {
            return this._invoicingEntities.UserMoments.Where(p => p.IsActive && p.UserUniqueId == userId)
                .OrderByDescending(p => p.PostTime)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageIndex)
                .Select(p => new UserMomentPreviewModel()
                {
                    Moment = p.Moment,
                    PostTime = p.PostTime,
                    Category = p.Category,
                    RefId = p.RefId
                })
                .ToList();
        }
    }
}
