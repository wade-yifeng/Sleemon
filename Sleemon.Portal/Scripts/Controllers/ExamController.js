SleemonPortal.controller('ExamController', ['$scope', '$http', 'ExamService', 'DTOptionsBuilder', 'DTColumnDefBuilder',
    function ($scope, $http, ExamService, DTOptionsBuilder, DTColumnDefBuilder) {
        ExamService.GetExamList()
                .then(function (result) {
                    $scope.exams = result;
                });

        $scope.dtOptions = DTOptionsBuilder.newOptions()
            .withDisplayLength(10)
            .withLanguage(SleemonPortal.dtLanguage)
            .withOption('order', [1, 'asc']);

        $scope.dtColumnDefs = [
            DTColumnDefBuilder.newColumnDef(6).notSortable().withClass('sorting_disabled')
        ];
    }]);