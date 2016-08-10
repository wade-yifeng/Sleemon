//nothing important, just a snippet to convert ui.router states into an array of sidebar items to be used in the partial template (sidebar.html)
//make a list of sidebar items using router states in angular/js/app.js
SleemonPortal.service('SidebarList', function () {
    //parent name for a state
    var getParentName = function (name) {
        var name = (/^(.+)\.[^.]+$/.exec(name) || [null, null])[1];
        return name;
    };
    //how many parents does this state have?
    var getParentCount = function (name) {
        return name.split('.').length;
    };

    this.getList = function (uiStateList) {

        var sidebar = { 'root': [] };//let's start with root and call it root! (see views/layouts/default/partial/sidebar.html)
        var parentList = {};//each node(item) can be a parent, so we add it to this list, and later if we find its children we know where to find the parent!

        for (var i = 0 ; i < uiStateList.length ; i++) {
            var state = uiStateList[i];
            if (!state.name || state.nested) continue;

            //copy state to 'item' (so state is not changed)
            var item = {};
            angular.copy(state, item);
            delete item['resolve']; delete item['templateUrl'];//delete these, we don't need them

            //item.name is state's name (dashboard, ui.elements, etc)
            item.url = item.name || '';

            parentList[item.name] = item;//save this item as a possible parent, and later we add possible children to it as submenu

            var parentName = getParentName(item.name);
            if (!parentName) {
                //no parent, so a root item
                sidebar.root.push(item);
                item['level-1'] = true;
            }
            else {
                //get the parent and add this item as a submenu element of parent
                var parentNode = parentList[parentName];
                if (!('submenu' in parentNode)) parentNode['submenu'] = [];
                parentNode['submenu'].push(item);
                item['level-' + getParentCount(item.name)] = true;
            }
        }

        parentList = null;

        return sidebar;
    };

});

//just load localStorage stored values, such as ace.settings, or ace.sidebar
SleemonPortal.service('StorageGet', function ($localStorage) {

    this.load = function ($scope, name) {
        $localStorage[name] = $localStorage[name] || {};

        var $ref = $scope;
        var parts = name.split('.');//for example when name is "ace.settings" or "ace.sidebar"
        for (var i = 0; i < parts.length; i++) $ref = $ref[parts[i]];
        //now $ref refers to $scope.ace.settings

        for (var prop in $localStorage[name]) if ($localStorage[name].hasOwnProperty(prop)) {
            $ref[prop] = $localStorage[name][prop];
        }
    };

});

SleemonPortal.dtLanguage = {
    "sProcessing": "处理中...",
    "sLengthMenu": "显示 _MENU_ 项结果",
    "sZeroRecords": "没有匹配结果",
    "sInfo": "显示第 _START_ 至 _END_ 项结果，共 _TOTAL_ 项",
    "sInfoEmpty": "显示第 0 至 0 项结果，共 0 项",
    "sInfoFiltered": "(由 _MAX_ 项结果过滤)",
    "sInfoPostFix": "",
    "sSearch": "搜索:",
    "sUrl": "",
    "sEmptyTable": "表中数据为空",
    "sLoadingRecords": "载入中...",
    "sInfoThousands": ",",
    "oPaginate": {
        "sFirst": "首页",
        "sPrevious": "上页",
        "sNext": "下页",
        "sLast": "末页"
    },
    "oAria": {
        "sSortAscending": ": 以升序排列此列",
        "sSortDescending": ": 以降序排列此列"
    }
}

SleemonPortal.service('DTServerSideBuilder', ['DTOptionsBuilder', 'DTDefaultOptions', function (DTOptionsBuilder, DTDefaultOptions) {
    this.initDT = function ($scope, $compile) {
        function loadServerRecords(source, data, fnCallback, settings) {
            var draw = data[0].value;
            var order = data[1].value[data[2].value[0].column].data + ' ' + data[2].value[0].dir;
            var start = data[3].value;
            var length = data[4].value;
            var query = data[5].value.value;

            $scope.loadDT(query, start, length, order, function (result) {
                var records = {
                    'draw': draw,
                    'recordsTotal': result.total,
                    'recordsFiltered': result.total,
                    'data': result.records
                };
                fnCallback(records);
            });
        }

        function createdRow(row, data, dataIndex) {
            // Recompiling so we can bind Angular directive to the DT
            $compile(angular.element(row).contents())($scope);
        }

        // angular-datatables的实例
        $scope.dtInstance = {};

        $scope.reloadDT = function () {
            $scope.dtInstance.rerender(SleemonPortal.dtLanguage);
        };

        $scope.dtOptions = DTOptionsBuilder
            .newOptions()
            .withFnServerData(loadServerRecords)
            .withDataProp('data')
            .withOption('processing', true)
            .withOption('serverSide', true)
            .withOption('paging', true)
            .withOption('createdRow', createdRow)
            .withOption('responsive', true)
            .withOption('bAutoWidth', false)
            .withDisplayLength(10).withLanguage(SleemonPortal.dtLanguage);

        DTDefaultOptions.setLoadingTemplate('<img src="/Assets/images/loading.gif" />');
    };
}]);

SleemonPortal.service('DTAngularBuilder', ['DTOptionsBuilder', 'DTDefaultOptions', function (DTOptionsBuilder, DTDefaultOptions) {
    this.initDT = function ($scope) {
        $scope.dtOptions = DTOptionsBuilder.newOptions().withDisplayLength(10).withLanguage(SleemonPortal.dtLanguage);

        DTDefaultOptions.setLoadingTemplate('<img src="/Assets/images/loading.gif" />');
    };
}]);