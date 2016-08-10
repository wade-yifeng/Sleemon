SleemonPortal.controller('KnowledgeController', ['$scope', 'KnowledgeService', 'DTOptionsBuilder', 'DTColumnDefBuilder',
    function ($scope, KnowledgeService, DTOptionsBuilder, DTColumnDefBuilder) {
        
        KnowledgeService.GetKnowledgeList().then(function (result) {
            $scope.knowledges = result;
        });

        $scope.dtOptions = DTOptionsBuilder.newOptions()
            .withDisplayLength(10)
            .withLanguage(SleemonPortal.dtLanguage)
            .withOption('order', [3, 'asc']);

        $scope.dtColumnDefs = [
            DTColumnDefBuilder.newColumnDef(4).notSortable().withClass('sorting_disabled')
        ];
    }]);