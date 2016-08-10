SleemonPortal.controller('KnowledgeDetailController', function ($scope, $state, $stateParams, KnowledgeService) {
    var knowledgeId = $stateParams.knowledgeId;
    if (knowledgeId) {
        KnowledgeService.GetKnowledgeDetailById(knowledgeId).then(function (result) {
            $scope.knowledge = result;
        });
    }
});