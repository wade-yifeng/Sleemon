SleemonPortal.controller('QnADetailController', ['$scope', '$state', '$stateParams', 'QnAService',
    function ($scope, $state, $stateParams, QnAService) {

        var questionId = $stateParams.questionId;
        if (questionId) {
            QnAService.GetUserQuestionById(questionId).then(function (result) {
                $scope.question = result;
            });
        }

        $scope.addToKnowledge = function () {
            $state.go('Graphics.Knowledge.Create', { question: $scope.question });
            // $state.transitionTo('Graphics.Knowledge.Create', { question: $scope.question });
        };
    }]);