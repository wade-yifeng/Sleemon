SleemonPortal.controller('NoticeCreateController', ['$scope', '$stateParams', 'NoticeService', '$http', '$state',
    function ($scope, $stateParams, NoticeService, $http, $state) {
        initUEditor();

        /* fields */
        var fields = $scope.fields = {
            notceId: $stateParams.noticeId,
            isNew: true,
            choiceForFileInput: null,
            text_limiter: {
                remText: '%n character%s remaining...',
                limitText: 'max allowed : %n.'
            },
            fileInputOptions: {
                style: 'well',
                btn_choose: '点击选择图片或者直接将图片拖进这里',
                btn_change: null,
                no_icon: 'ace-icon fa fa-cloud-upload',
                droppable: true,
                thumbnail: 'large'
            },
            fileInputProps: {}
        };
        var notice = $scope.notice = {

        };

        /* init */
        if (fields.notceId) {
            NoticeService.GetNotice(fields.notceId).then(function (result) {
                notice = $scope.notice = result;
                notice.AvatarPath = result.avatar;
                setUEditor(result.Context);
            });
        }

        $scope.submitNotice = function () {
            NoticeService.SubmitNotice(notice).then(function (result) {
                $state.go('Graphics.Notice');
            });
        };

        $scope.noticeImageChanged = function () {
            if (!notice.fileInput) return;

            fields.fileInputProps.loading = true;

            var formData = new FormData();
            formData.append('file', notice.fileInput[0]);

            $http.post('/EnterpriseNotice/UpLoadImageFile', formData,
                {
                    headers: {
                        'Content-Type': undefined,
                        'HTTP_X_REQUESTED_WITH': 'XMLHttpRequest'
                    }
                }).success(function (data) {
                    notice.AvatarPath = data.filePath;
                    fields.fileInputProps.loading = false;
                })
                .error(function (data, status) {
                    fields.fileInputProps.loading = false;
                });
        };
    }]);