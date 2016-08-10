SleemonPortal.controller('ILegalCommentController', ['$scope', '$state', '$stateParams', 'SupervisionService', 'DTOptionsBuilder', 'DTColumnDefBuilder',
    function ($scope, $state, $stateParams, SupervisionService, DTOptionsBuilder, DTColumnDefBuilder) {
        SupervisionService.GetIlegalCommentList()
            .then(function (result) {
                $scope.comments = result;
            });

        $scope.dtOptions = DTOptionsBuilder.newOptions()
            .withDisplayLength(10)
            .withLanguage(SleemonPortal.dtLanguage)
            .withOption('order', [3, 'asc']);

        $scope.dtColumnDefs = [
            DTColumnDefBuilder.newColumnDef(4).notSortable().withClass('sorting_disabled')
        ];
        
        $scope.updateLegalStatus= function (commentId, legalStatus) {
            SupervisionService.UpdateCommentLegalStatus(commentId, legalStatus).then(function (result) {
                $state.transitionTo($state.current, $stateParams, {
                    reload: true,
                    inherit: false,
                    notify: true
                });
            });
        }
    }]);