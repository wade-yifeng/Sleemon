SleemonPortal.service('ExamService', function ($http, Promise, HttpGet, HttpPost) {
    return {
        GetExamList: function () {
            return Promise(function (defer) {
                HttpGet('/Exam/GetExamList', null, defer);
            });
        },
        GetExamDetail: function (examId) {
            return Promise(function (defer) {
                HttpGet('/Exam/GetExamDetail', { examId: examId }, defer);
            });
        },
        ExamCreate: function (exam) {
            return Promise(function (defer) {
                HttpPost('/Exam/ExamCreate', { exam: exam }, defer);
            });
        }
    };
});