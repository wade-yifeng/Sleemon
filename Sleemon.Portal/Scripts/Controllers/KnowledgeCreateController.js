SleemonPortal.controller('KnowledgeCreateController', function ($scope, $state, $stateParams, KnowledgeService) {
    $scope.error = '';
    $scope.knowledge = {};
    
    if ($stateParams.question) {
        $scope.knowledge.Title = $stateParams.question.Title;
        $scope.knowledge.Detail = $stateParams.question.Answer;
    }

    var knowledgeId = $stateParams.knowledgeId;
    if (knowledgeId) {
        KnowledgeService.GetKnowledgeDetailById(knowledgeId).then(function (result) {
            $scope.knowledge = result;
        });
    }

    KnowledgeService.GetKeyWordsList().then(function (result) {
        $scope.keywords = result;
    });

    $scope.submitKnowledge = function () {
        $scope.error = '';

        KnowledgeService.KnowledgeCreate($scope.knowledge).then(function (result) {
            if (result === 'Save successful.') {
                $state.go('Graphics.Knowledge.Detail', { knowledgeId: knowledgeId });
            } else {
                $scope.error = result;
            }
        });
    }
});