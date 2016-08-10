SleemonPortal.controller('QuestionnaireDetailController', ['$scope', '$stateParams', 'QuestionnaireService',
    function ($scope, $stateParams, QuestionnaireService) {
        $scope.isDetailPage = true;

        QuestionnaireService.GetQuestionnaire($stateParams.questionnaireId).then(function (result) {
            $scope.questionnaire = result;
            initWaterfall($scope.questionnaire.Questions.length > 0);
        });

        $scope.convertToChoiceChar = function (choice) {
            return String.fromCharCode(64 + choice);
        }
    }]);