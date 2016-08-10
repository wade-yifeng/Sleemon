SleemonPortal.service('QuestionnaireService', ['$http', 'Promise', 'HttpGet', 'HttpPost', function ($http, Promise, HttpGet, HttpPost) {
    return {
        GetQuestionnaireList: function (pageIndex, title) {
            return Promise(function (defer) {
                HttpGet('/Questionnaire/GetQuestionnaireList', {
                    pageIndex: pageIndex,
                    pageSize: config.PAGE_SIZE,
                    questionnaireTitle: title
                }, defer);
            });
        },
        GetQuestionnaire: function (questionnaireId) {
            return Promise(function (defer) {
                HttpGet('/Questionnaire/GetQuestionnaire/' + questionnaireId, null, defer);
            });
        },
        CreateQuestionnaire: function (questionnaire) {
            return Promise(function (defer) {
                HttpPost('/Questionnaire/CreateQuestionnaire', {
                    questionnaire: questionnaire
                }, defer);
            });
        },
        DeleteQuestionnaire: function (questionnaireId) {
            return Promise(function (defer) {
                HttpPost('/Questionnaire/DeleteQuestionnaire', {
                    id: questionnaireId
                }, defer);
            });
        }
    };
}]);