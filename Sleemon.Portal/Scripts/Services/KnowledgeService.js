SleemonPortal.service('KnowledgeService', function ($http, Promise, HttpGet, HttpPost) {
    return {
        GetKnowledgeList: function () {
            return Promise(function (defer) {
                HttpGet('/Knowledge/GetKnowledgeList', null, defer);
            });
        },
        GetKnowledgeDetailById: function (knowledgeBaseId) {
            return Promise(function (defer) {
                HttpGet('/Knowledge/GetKnowledgeDetailById', { knowledgeBaseId: knowledgeBaseId }, defer);
            });
        },
        GetKeyWordsList: function () {
            return Promise(function (defer) {
                HttpGet('/Knowledge/GetKeyWordsList', null, defer);
            });
        },
        KnowledgeCreate: function (knowledge) {
            return Promise(function (defer) {
                HttpPost('/Knowledge/KnowledgeCreate', { knowledge: knowledge }, defer);
            });
        }
    }
});