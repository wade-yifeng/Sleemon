SleemonPortal.controller('TrainingController', ['$scope', '$stateParams', 'TrainingService', 'DTOptionsBuilder', 'DTColumnDefBuilder',
    function ($scope, $stateParams, TrainingService, DTOptionsBuilder, DTColumnDefBuilder) {
        TrainingService.GetTrainingList(1).then(function (list) {
            $scope.list = list;
        })

        $scope.dtOptions = DTOptionsBuilder.newOptions()
            .withDisplayLength(10)
            .withLanguage(SleemonPortal.dtLanguage)
            .withOption('order', [1, 'asc']);

        $scope.dtColumnDefs = [
            DTColumnDefBuilder.newColumnDef(5).notSortable().withClass('sorting_disabled')
        ];
    }]);