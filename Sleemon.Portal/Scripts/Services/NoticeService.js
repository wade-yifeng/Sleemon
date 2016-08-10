SleemonPortal.service('NoticeService', ['$http', 'Promise', 'HttpGet', 'HttpPost', function ($http, Promise, HttpGet, HttpPost) {
    return {
        GetNoticeList: function (pageIndex, title) {
            return Promise(function (defer) {
                HttpGet('/EnterpriseNotice/GetNoticeList', {
                    pageIndex: pageIndex,
                    pageSize: config.PAGE_SIZE,
                    noticeTitle: title
                }, defer);
            });
        },
        GetNotice: function (noticeId) {
            return Promise(function (defer) {
                HttpGet('/EnterpriseNotice/GetEnterpriseNotice/' + noticeId, null, defer);
            });
        },
        SubmitNotice: function (notice) {
            return Promise(function (defer) {
                HttpPost('/EnterpriseNotice/SubmitEnterpriseNotice', {
                    notice: notice
                }, defer);
            });
        }
    };
}]);