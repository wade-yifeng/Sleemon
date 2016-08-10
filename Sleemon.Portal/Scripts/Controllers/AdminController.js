//
SleemonPortal.controller('AdminController', ['$scope', 'Admin', 'DTOptionsBuilder', 'DTColumnDefBuilder', '$aside',
    function ($scope, Admin, DTOptionsBuilder, DTColumnDefBuilder, $aside) {
        var updateAdminRoles = function () {
            Admin.GetAdminListWithRoles().then(function (result) {
                $scope.adminRoles = result;
            });
            $scope.selectedCount = 0;
        };

        var getSelectedCount = function () {
            return ($scope.adminRoles != undefined ? $.grep($scope.adminRoles, function (admin) { return admin.selected; }) : []).length;
        };

        var getSelectedAdmin = function () {
            return $scope.adminRoles != undefined ? $.grep($scope.adminRoles, function (admin) { return admin.selected; }) : [];
        };

        updateAdminRoles();

        // angular-datatables的实例
        $scope.dtInstance = {};

        $scope.dtOptions = DTOptionsBuilder.newOptions()
            .withDisplayLength(10)
            .withLanguage(SleemonPortal.dtLanguage)
            .withOption('order', [1, 'asc']);

        $scope.dtColumnDefs = [
            DTColumnDefBuilder.newColumnDef(0).notSortable().withClass('sorting_disabled')
        ];

        $scope.selectFlag = false;
        $scope.selectedCount = 0;

        $scope.selectAll = function (selectFlag) {
            angular.forEach($scope.adminRoles, function (admin) {
                admin.selected = selectFlag;
            });
            $scope.selectedCount = getSelectedCount();
        };

        $scope.selectAdmin = function () {
            $scope.selectedCount = getSelectedCount();
        };

        $scope.updateRoles = function () {
            var admins = getSelectedAdmin();
            var editModel = admins.length == 1;
            if (editModel) {
                $aside.open({
                    templateUrl: 'updateRoles.html',
                    placement: 'bottom',
                    backdrop: false,
                    controller: function ($scope, $uibModalInstance, $timeout, Admin) {
                        $scope.admin = admins[0];
                        Admin.GetAllRoleList().then(function (result) {
                            angular.forEach(result, function (item) {
                                item.selected = $scope.admin.RoleIds.indexOf(item.value) >= 0;
                            });
                            $scope.roles = result;
                            $timeout(function () {
                                var roleList = $('select[name="roles"]')
                                    .bootstrapDualListbox({
                                        filterPlaceHolder: '搜索',
                                        moveOnSelect: false,
                                        moveSelectedLabel: '移动选中项',
                                        moveAllLabel: '全部选中',
                                        removeSelectedLabel: '清除选中项',
                                        removeAllLabel: '全部清除',
                                        infoTextFiltered: '<span class="label label-purple label-lg">搜索</span>'
                                    });
                                var container = roleList.bootstrapDualListbox('getContainer');
                                container.find('span.info-container').remove();
                                container.find('.btn').addClass('btn-white btn-info btn-bold');
                            });
                        });

                        $scope.cancel = function (e) {
                            $uibModalInstance.dismiss();
                            e.stopPropagation();
                        };

                        $scope.updateAdminRoles = function () {
                            var $submitBtn = $(document.activeElement);
                            $submitBtn.attr("disabled", "disabled");
                            Admin.UpdateAdminRoles(
                                updateRoleForm['__RequestVerificationToken'].value,
                                $.parseJSON(updateRoleForm['admin'].value),
                                $('select[name="roles"]').val()
                            ).then(function (result) {
                                if (result && result.success) {
                                    updateAdminRoles();
                                    $uibModalInstance.close();
                                    bootbox.dialog({
                                        size: 'small',
                                        message: "角色已经更新",
                                        title: "操作成功",
                                        buttons: {
                                            main: {
                                                label: "确认",
                                                className: "btn-primary",
                                                callback: function () { }
                                            }
                                        }
                                    });
                                } else {
                                    bootbox.dialog({
                                        size: 'small',
                                        message: "提交发生错误，请联系管理员！",
                                        title: "发生异常",
                                        buttons: {
                                            main: {
                                                label: "确认",
                                                className: "btn-primary",
                                                callback: function () { }
                                            }
                                        }
                                    });
                                }

                                $submitBtn.removeAttr("disabled");
                            });
                        }
                    }
                });
            } else {
                $aside.open({
                    templateUrl: 'addAdminRoles.html',
                    placement: 'bottom',
                    backdrop: false,
                    controller: function ($scope, $uibModalInstance, $timeout, Admin) {
                        $scope.selectedAdmins = angular.toJson(admins, true);

                        $scope.cancel = function (e) {
                            $uibModalInstance.dismiss();
                            e.stopPropagation();
                        };

                        Admin.GetAllRoleList().then(function (result) {
                            $scope.roles = result;
                        });

                        $timeout(function () {
                            $('#adminRoleForm').validate({
                                errorElement: 'div',
                                errorClass: 'help-block',
                                ignore: "",
                                rules: {
                                    adminRoles: {
                                        required: true,
                                        minlength: 3
                                    }
                                },

                                messages: {
                                    adminRoles: {
                                        required: "请至少选择一个权限",
                                        minlength: "请至少选择一个权限"
                                    }
                                },


                                highlight: function (e) {
                                    $(e).closest('.form-group').removeClass('has-info').addClass('has-error');
                                },

                                success: function (e) {
                                    $(e).closest('.form-group').removeClass('has-error');//.addClass('has-info');
                                    $(e).remove();
                                },

                                errorPlacement: function (error, element) {
                                    if (element.is('.multiselect')) {
                                    }
                                    else error.insertAfter(element);
                                },

                                submitHandler: function (form) {
                                    var $submitBtn = $(document.activeElement);
                                    $submitBtn.attr("disabled", "disabled");
                                    Admin.SubmitAdminRoles(
                                        adminRoleForm['__RequestVerificationToken'].value,
                                        $.parseJSON(adminRoleForm['admins'].value),
                                        $.parseJSON(adminRoleForm['adminRoles'].value)
                                    ).then(function (result) {
                                        if (result && result.success) {
                                            updateAdminRoles();
                                            $uibModalInstance.close();
                                            bootbox.dialog({
                                                size: 'small',
                                                message: "角色已经更新",
                                                title: "操作成功",
                                                buttons: {
                                                    main: {
                                                        label: "确认",
                                                        className: "btn-primary",
                                                        callback: function () { }
                                                    }
                                                }
                                            });
                                        } else {
                                            bootbox.dialog({
                                                size: 'small',
                                                message: "提交发生错误，请联系管理员！",
                                                title: "发生异常",
                                                buttons: {
                                                    main: {
                                                        label: "确认",
                                                        className: "btn-primary",
                                                        callback: function () { }
                                                    }
                                                }
                                            });
                                        }

                                        $submitBtn.removeAttr("disabled");
                                    });
                                },
                                invalidHandler: function (form) { }
                            });

                            $('select.multiselect').on('change', function () {
                                $(this).closest('form').validate().element($('input[name="adminRoles"]'));
                            });
                        });
                    }
                });
            }
        };
    }]
);