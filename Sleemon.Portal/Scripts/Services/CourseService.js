SleemonPortal.service('CourseService', function ($http, Promise, HttpGet, HttpPost) {
    return {
        GetCourseList: function () {
            return Promise(function (defer) {
                HttpGet('/Course/GetCourseList', null, defer);
            });
        },
        GetCourseDetail: function (courseId) {
            return Promise(function (defer) {
                HttpGet('/Course/GetCourseDetail', { courseId: courseId }, defer);
            });
        },
        CourseCreate: function (course) {
            return Promise(function (defer) {
                HttpPost('/Course/CourseCreate', { course: course }, defer);
            });
        }
    };
});