SleemonPortal.controller('ExamDetailController', ['$scope', '$stateParams', 'ExamService',
    function ($scope, $stateParams, ExamService) {
        $scope.isDetailPage = true;

        ExamService.GetExamDetail($stateParams.examId).then(function (result) {
            exam = $scope.exam = result;
            initWaterfall(exam.Questions.length > 0);
        });

        $scope.convertToChoiceChar = function (choice) {
            return String.fromCharCode(64 + choice);
        }
    }]);