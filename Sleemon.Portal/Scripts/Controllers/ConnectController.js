//
SleemonPortal.controller('ConnectController', ['$scope', '$compile', 'Contacts', 'DTColumnBuilder', 'DTServerSideBuilder',
    function ($scope, $compile, Contacts, DTColumnBuilder, DTServerSideBuilder) {
        var records = [];

        function createSubTree(endFlag, parentId) {
            if (!endFlag) {
                var items = $.grep(records, function (item) {
                    return item.ParentId == parentId;
                });

                if (items.length === 0) {
                    endFlag = true;
                    return [];
                }

                var res = [];
                angular.forEach(items, function (item, i) {
                    res.push({ "label": item.Name, "id": item.Id, "i": i, "children": createSubTree(endFlag, item.Id) });
                })

                return res;
            }
            else
                return [];
        };

        $scope.treeOptions = {
            injectClasses: {
                ul: 'tree tree-branch-children',
                li: 'tree-branch',
                liSelected: 'tree-selected',
                iExpanded: 'icon-caret ace-icon tree-minus',
                iCollapsed: 'icon-caret ace-icon tree-plus',
                iLeaf: 'icon-item ace-icon fa fa-circle bigger-110',
                label: 'tree-branch-header'
            }
        };

        Contacts.GetDepartment().then(function (result) {
            records = result.records;

            var treeData = [];
            angular.forEach(result.rootIds, function (item) {
                treeData = $.merge(treeData, createSubTree(false, item));
            });

            $scope.treeData = treeData;

            $scope.selected = treeData[0];
            $scope.showSelected(treeData[0]);
        });

        $scope.setAdmin = function () {
            var $submitBtn = $(document.activeElement);
            $submitBtn.attr("disabled", "disabled");
            var userUniqueId = $submitBtn.closest('tr').find('td:first').text();
            Contacts.SetAdmin($('input[name="__RequestVerificationToken"]').val(), userUniqueId).then(function (result) {
                if (result && result.success) {
                    bootbox.dialog({
                        size: 'small',
                        message: "设置成功",
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
        };

        $scope.dtColumns = [
            DTColumnBuilder.newColumn('UserUniqueId').withTitle('工号'),
            DTColumnBuilder.newColumn('Name').withTitle('姓名').renderWith(function (data, type, full) {
                return '<a href="#" >' + data + '</a>';
            }),
            DTColumnBuilder.newColumn('Mobile').withTitle('手机号'),
            DTColumnBuilder.newColumn('Position').withTitle('职位'),
            DTColumnBuilder.newColumn('Status').withTitle('状态').renderWith(function (data, type, full) {
                switch (data) {
                    case 1:
                        return '<span class="label label-sm label-success">已关注</span>';
                    case 2:
                        return '<span class="label label-sm label-danger">已禁用</span>';
                    case 4:
                        return '<span class="label label-sm label-warning">未关注</span>';
                }
            }),
            DTColumnBuilder.newColumn('IsAdmin').withTitle('管理员').renderWith(function (data, type, full) {
                switch (data) {
                    case true:
                        return '<span uib-tooltip="已是管理员" tooltip-placement="right" class="badge badge-transparent tooltip-success"><button class="btn btn-xs btn-success"><i class="ace-icon fa fa-check bigger-110"></i></button></span>';
                    case false:
                        return '<span uib-tooltip="设为管理员" tooltip-placement="right" class="badge badge-transparent tooltip-warning"><button class="btn btn-xs btn-warning" ng-click="setAdmin()"><i class="ace-icon fa fa-flag bigger-110"></i></button></span>';
                }
            })
        ];

        $scope.showSelected = function (node) {
            $scope.Department = node.label;
            $scope.deptId = node.id;
            $scope.reloadDT();
        };

        $scope.loadDT = function (query, start, length, order, callback) {
            if ($scope.deptId !== undefined) {
                Contacts.GetUserListByDepartment($scope.deptId, query, start, length, order).then(callback);
            }
        };

        DTServerSideBuilder.initDT($scope, $compile);
    }
]);