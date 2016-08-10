// 全局Layout控制器
SleemonPortal.constant('SAVE_SETTING', true)
    .controller('MainController', ['$scope', '$rootScope', 'SAVE_SETTING', '$localStorage', 'StorageGet',
        function ($scope, $rootScope, SAVE_SETTING, $localStorage, StorageGet) {
            // 全局变量存储
            $scope.portal = $scope.portal || {};
            $scope.portal.path = {
                'assets': '../Assets' // 配置模板中的Assets相对路径
            };

            // sidebar选项
            $scope.portal.sidebar = {
                'minimized': false,
                'toggle': false,
                'reset': false
            };

            // 默认设置
            $scope.portal.settings = $scope.portal.settings || {};

            if (SAVE_SETTING) $localStorage['portal.settings'] = $localStorage['portal.settings'] || {};

            $scope.portal.settings = {
                'is_open': false,
                'open': function () {
                    $scope.portal.settings.is_open = !$scope.portal.settings.is_open;
                },

                'navbar': false,
                'sidebar': false,
                'breadcrumbs': false,
                'hover': false,
                'compact': false,
                'highlight': false,
                //'rtl': false,
                'skinColor': '#438EB9',
                'skinIndex': 0
            };

            if (SAVE_SETTING) StorageGet.load($scope, 'portal.settings');//load previously saved setting values

            // 绑定全局加载事件
            $rootScope.viewContentLoading = false;
            $rootScope.$on('$stateChangeStart', function (event) {
                $rootScope.viewContentLoading = true;

                // 手机模式下，跳转时隐藏sidebar
                $scope.portal.sidebar.toggle = false;
            });
            $rootScope.$on('$stateChangeSuccess', function (event) {
                $rootScope.viewContentLoading = false;
            });
            $rootScope.$on('$stateChangeError', function (event) {
                $rootScope.viewContentLoading = false;
            });

            // 用于切换皮肤
            $scope.bodySkin = function () {
                var skin = $scope.portal.settings.skinIndex;
                if (skin == 1 || skin == 2) return 'skin-' + skin;
                else if (skin == 3) return 'no-skin skin-3';
                return 'no-skin';
            };

            // 枚举加入全局scope,以便直接在ng模板中使用
            if (window.Enum) $scope.Enum = window.Enum;
            $scope.getEnumByValue = function (enumObj, value) {
                var targetEnumItem;
                Object.keys(enumObj).some(function (key) {
                    var current = enumObj[key];
                    if (current.Key == value) {
                        targetEnumItem = current;
                        return true;
                    }
                });
                return targetEnumItem;
            };
        }
    ]);

// 首页侧边栏
SleemonPortal.controller('SidebarController',
    ['$scope', '$state', '$rootScope', 'SidebarList', 'SAVE_SETTING', '$localStorage', 'StorageGet',
        function ($scope, $state, $rootScope, SidebarList, SAVE_SETTING, $localStorage, StorageGet) {
            $scope.portal = $scope.$parent.portal;
            $scope.portal.sidebar = $scope.portal.sidebar || {};

            if (SAVE_SETTING) {
                StorageGet.load($scope, 'portal.sidebar');//加载之前保存的侧边栏属性
                $scope.$watch('portal.sidebar.minimized', function (newValue) {
                    $localStorage['portal.sidebar']['minimized'] = newValue;
                });
            }

            var permissions = JSON.parse(window.sessionStorage.permissions);
            var routes = $.grep($state.get(), function (item) {
                return !item.nested && !angular.isUndefined(permissions[item.abstract ? item.name : item.url]);
            });

            // 获取ng的route信息，转化为数组
            $scope.sidebar = SidebarList.getList(routes);

            $rootScope.subMenuOpen = {};
            $rootScope.isSubOpen = function (name) {
                if (!(name in $rootScope.subMenuOpen)) $rootScope.subMenuOpen[name] = false;
                return $rootScope.subMenuOpen[name];
            }
            $rootScope.isActiveItem = function (name) {
                return $rootScope.activeItems ? $rootScope.activeItems[name] : false;
            }
        }
    ]
);