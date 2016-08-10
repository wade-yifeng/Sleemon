SleemonPortal.controller('QnAController', ['$scope', 'QnAService', 'DTOptionsBuilder', 'DTColumnDefBuilder',
    function ($scope, QnAService, DTOptionsBuilder, DTColumnDefBuilder) {
        
        QnAService.GetQnAList().then(function (result) {
            $scope.userQues = result;
        });

        $scope.dtOptions = DTOptionsBuilder.newOptions()
            .withDisplayLength(10)
            .withLanguage(SleemonPortal.dtLanguage)
            .withOption('order', [5, 'asc']);

        $scope.dtColumnDefs = [
            DTColumnDefBuilder.newColumnDef(6).notSortable().withClass('sorting_disabled')
        ];
    }]);