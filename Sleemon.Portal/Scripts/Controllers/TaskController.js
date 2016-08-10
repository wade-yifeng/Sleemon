SleemonPortal.controller('TaskController', ['$scope', 'TaskService', 'DTOptionsBuilder', 'DTColumnDefBuilder',
    function ($scope, TaskService, DTOptionsBuilder, DTColumnDefBuilder) {
        TaskService.GetTaskList().then(function (result) {
                $scope.tasks = result;
            });

        $scope.dtOptions = DTOptionsBuilder.newOptions()
            .withDisplayLength(10)
            .withLanguage(SleemonPortal.dtLanguage)
            .withOption('order', [1, 'asc']);
    }]);