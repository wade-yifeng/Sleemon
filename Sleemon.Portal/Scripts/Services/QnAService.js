SleemonPortal.service('QnAService', function ($http, Promise, HttpGet, HttpPost) {
    return {
        GetQnAList: function () {
            return Promise(function (defer) {
                HttpGet('/QnA/GetQnAList', null, defer);
            });
        },
        GetUserQuestionById: function (questionId) {
            return Promise(function (defer) {
                HttpGet('/QnA/GetUserQuestionById', { questionId: questionId }, defer);
            })
        },
        SaveReplyAnswer: function (answer) {
            return Promise(function (defer) {
                HttpPost('/QnA/SaveReplyAnswer', { answer: answer }, defer);
            });
        }
    }
});