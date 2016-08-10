SleemonPortal.controller('TrainingDetailController', ['$scope', '$stateParams', 'TrainingService', '$state',
    function ($scope, $stateParams, TrainingService, $state) {
        TrainingService.GetTraining($stateParams.trainingId).then(function (result) {
            $scope.training = result;
        });
    }]);
