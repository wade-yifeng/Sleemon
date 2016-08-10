SleemonPortal.service('TaskService', function ($http, Promise, HttpGet, HttpPost) {
    return {
        GetTaskList: function () {
            return Promise(function (defer) {
                HttpGet('/Task/GetTaskList', null, defer);
            });
        },
        GetTaskDetail: function (taskId) {
            return Promise(function (defer) {
                HttpGet('/Task/GetTaskDetail', { taskId: taskId }, defer);
            });
        },
        TaskCreate: function (task) {
            return Promise(function (defer) {
                HttpPost('/Task/TaskCreate', { task: task }, defer);
            });
        },
        GetTaskItemList: function (category) {
            return Promise(function (defer) {
                HttpGet('/Task/GetTaskItemList', { category: category }, defer);
            });
        }
    };
});