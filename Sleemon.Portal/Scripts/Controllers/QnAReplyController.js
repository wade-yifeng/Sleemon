SleemonPortal.controller('QnAReplyController', ['$scope', '$state', '$stateParams', 'QnAService',
    function ($scope, $state, $stateParams, QnAService) {
        $scope.error = '';
        
        var questionId = $stateParams.questionId;
        if (questionId) {
            QnAService.GetUserQuestionById(questionId).then(function (result) {
                $scope.question = result;
            });
        }
        
        $scope.replyQuestion = function () {
            $scope.error = '';
            if ($scope.question.Answer !== '') {
                QnAService.SaveReplyAnswer($scope.question).then(function (result) {
                    if (result === 'Save successful.') {
                        $state.go('Graphics.QnA.Detail', {questionId: questionId});
                    } else {
                        $scope.error = result;
                    }
                });
            } else {
                $scope.error = '请输入回复内容后 再点击<回复>按钮';
            }
        };

    }]);