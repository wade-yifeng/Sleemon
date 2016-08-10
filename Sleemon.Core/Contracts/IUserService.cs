namespace Sleemon.Core
{
    using System.Collections.Generic;

    using Sleemon.Data;

    public interface IUserService
    {
        SyncResult SyncUser(IEnumerable<UserProfile> users);

        UserViewModel GetUserInfoById(string userId);

        User GetUserById(string userId);

        IList<User> GetUsersForDepartment(int departmentId, string query, int start, int length, string order, out int total);

        IList<AdminRolesModel> GetAdminListWithRoles();

        UserDailySignInModel UserDailySignIn(string userId);

        User GetUserByUniqueId(string userUniqueId);

        User Login(string userUniqueId, string password);

        bool ResetPassword(string userId, string password);

        ResultBase SetUserRole(string currentUserId, string userUniqueid, int roleid);

        ResultBase UpdateUserProfile(UserProfile userProfile);

        bool SetAdmin(string userUniqueId);

        bool UpdateAdminRoles(IEnumerable<AdminRoleModel> adminRoles);

        bool UpdateAdminRoles(AdminRoleModel admin, IEnumerable<int> roles);

        bool IsOriginalPassword(string userId);

        IList<UserPointRecordPreviewModel> GetUserPointRecordList(string userId, int pageIndex, int pageSize);

        IList<UserMomentPreviewModel> GetUserMomentsList(string userId, int pageIndex, int pageSize);
    }
}