SleemonPortal.controller('CourseController',
    ['$scope', '$window', 'CourseService', 'DTOptionsBuilder', 'DTColumnDefBuilder', '$compile', '$timeout',
        function ($scope, $window, CourseService, DTOptionsBuilder, DTColumnDefBuilder, $compile, $timeout) {
            $scope.file = {};
            CourseService.GetCourseList().then(function (result) {
                $scope.courses = result;
            });

            // angular-datatables的实例
            $scope.dtInstance = {};

            $scope.dtOptions = DTOptionsBuilder.newOptions()
                .withDisplayLength(10)
                .withLanguage(SleemonPortal.dtLanguage)
                .withOption('order', [1, 'asc']);

            $scope.dtColumnDefs = [
                DTColumnDefBuilder.newColumnDef(0).notSortable().withClass('sorting_disabled')
            ];

            $scope.ShowDetail = function (course, event) {
                $scope.chapters = course.Chapters;

                var link = angular.element(event.currentTarget),
                    icon = link.find('i.ace-icon'),
                    tr = link.parent().parent(),
                    table = $scope.dtInstance.DataTable,
                    row = table.row(tr);

                if (row.child.isShown()) {
                    icon.removeClass('fa-angle-double-up').addClass('fa-angle-double-down');
                    row.child.hide();
                    tr.removeClass('shown');
                }
                else {
                    row.child($compile('<div tmpl templatUrl="/Templates/course.html" class="table-detail"></div>')($scope)).show();
                    $timeout(function () {
                        icon.removeClass('fa-angle-double-down').addClass('fa-angle-double-up');
                        $('.dd').nestable();
                        $('.dd-handle a').on('mousedown', function (e) {
                            e.stopPropagation();
                        });
                        tr.addClass('shown');
                    });
                }
            }

            $scope.editChapter = function (chapter) {
                $scope.fileDetail = null;
                $scope.chapterDetail = chapter;
            }
            $scope.editFile = function (file) {
                $scope.chapterDetail = null;
                $scope.fileDetail = file;
            }

            $scope.showFileEditor = function (file) {
                $scope.file = file || {};
                var editor = $('#fileEditor');
                editor.modal();
            };

            $scope.setFileType = function (fileType) {

            };

            // var contentInput;        
            // $window.UEDITOR_HOME_URL = 'Assets/components/ueditor/';
            // var ue = $window.UE.getEditor('container', {
            //     autoHeightEnabled: false
            // });

            // ue.ready(function () {
            //     ue.addListener('afterInsertVideo', function (t, arg) {
            //         if (contentInput) {
            //             contentInput = arg[0].url;
            //         }
            //     });
            // });

            // $scope.$on('$destory', function () {
            //     ue.destory();
            // });

            // $scope.uploadFile = function (file) {
            //     contentInput = file.Content;
            //     var videoDialog = ue.getDialog('insertvideo');
            //     videoDialog.open();
            // }

            $scope.submitLearningFile = function () {

            };
        }
    ]
);