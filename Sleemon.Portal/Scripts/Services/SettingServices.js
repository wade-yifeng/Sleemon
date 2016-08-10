//
SleemonPortal.service('Contacts', function ($http, Promise, HttpGet, HttpPost) {
    return {
        GetDepartment: function () {
            return Promise(function (defer) {
                HttpGet('/Setting/GetDepartment', null, defer);
            });
        },
        GetUserListByDepartment: function (departmentId, query, start, length, order) {
            return Promise(function (defer) {
                HttpGet('/Setting/GetUserListByDepartment', { departmentId: departmentId, query: query, start: start, length: length, order: order }, defer);
            });
        },
        SetAdmin: function (token, userUniqueId) {
            return Promise(function (defer) {
                HttpPost('/Setting/SetAdmin', { userUniqueId: userUniqueId, antiForgeryToken: token }, defer);
            });
        }
    };
});

SleemonPortal.service('Admin', function ($http, Promise, HttpGet, HttpPost) {
    return {
        GetAdminListWithRoles: function () {
            return Promise(function (defer) {
                HttpGet('/Setting/GetAdminListWithRoles', null, defer);
            });
        },
        GetAllRoleList: function () {
            return Promise(function (defer) {
                HttpGet('/Setting/GetAllRoleList', null, defer);
            });
        },
        SubmitAdminRoles: function(token, admins, adminRoles) {
            return Promise(function (defer) {
                HttpPost('/Setting/SubmitAdminRoles', {
                    admins: admins,
                    adminRoles: adminRoles,
                    antiForgeryToken: token
                }, defer);
            });
        },
        UpdateAdminRoles: function (token, admin, roles) {
            return Promise(function (defer) {
                HttpPost('/Setting/UpdateAdminRoles', {
                    admin: admin,
                    roles: roles,
                    antiForgeryToken: token
                }, defer);
            });
        }
    };
});