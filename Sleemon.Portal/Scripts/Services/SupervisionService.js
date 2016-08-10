SleemonPortal.service('SupervisionService', function ($http, Promise, HttpGet, HttpPost) {
    return {
        GetIlegalCommentList: function () {
            return Promise(function (defer) {
                HttpGet('/Supervision/GetIlegalCommentList', null, defer);
            });
        },
        UpdateCommentLegalStatus: function (commentId, legalStatus) {
            return Promise(function (defer) {
                HttpPost('/Supervision/UpdateCommentLegalStatus', { commentId: commentId, legalStatus: legalStatus }, defer);
            });
        }
    };
});