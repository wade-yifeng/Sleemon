SleemonPortal.controller('TaskCreateController', ['$scope', '$state', '$stateParams', 'TaskService', 'WizardHandler', 'DTOptionsBuilder', 'DTColumnDefBuilder',
    function ($scope, $state, $stateParams, TaskService, WizardHandler, DTOptionsBuilder, DTColumnDefBuilder) {
        $scope.taskItemId;
        var task = $scope.task = {};

        if ($stateParams.taskId && $stateParams.taskId > 0) {
            TaskService.GetTaskDetail($stateParams.taskId).then(function (result) {
                task = $scope.task = result;
                if(task){
                    if(task.TaskCategory == Enum.TaskCategory.Learning.Key) {
                        $scope.taskItemId = task.LearningFiles[0].Id;
                    } else if (task.TaskCategory == Enum.TaskCategory.Exam.Key) {
                        $scope.taskItemId = task.Exams[0].Id;
                    } else if (task.TaskCategory == Enum.TaskCategory.Questionnaire.Key) {
                        $scope.taskItemId = task.Questionnaires[0].Id;
                    }
                }
            });
        }

        //spinner options
        $scope.$spinner = [
            { min: 0, max: 1000, step: 5, on_sides: true, icon_up: 'ace-icon fa fa-plus bigger-110', icon_down: 'ace-icon fa fa-minus bigger-110', btn_up_class: 'btn-success', btn_down_class: 'btn-danger' }
            , { min: 0, max: 1000, step: 5, btn_up_class: 'btn-info', btn_down_class: 'btn-info' }
        ];

        //datepicker value and options
        $scope.datePicker = {
            value: null,
            range: null,
            opts: {
                format: 'yyyy-mm-dd',
                autoclose: true,
                todayHighlight: true
            }
        };

        // data table options
        $scope.dtOptions = DTOptionsBuilder.newOptions().withDisplayLength(10).withLanguage({
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
        }).withOption('order', [1, 'asc']);

        //we keep track of current step so that we know when to hide/show 'finish' button and 'next' button
        $scope.currentStepName = null;
        $scope.currentStepNumber = 1;
        $scope.isFinalStep = false;

        $scope.$watch('currentStepName', function (newValue) {
            $scope.currentStepNumber = WizardHandler.wizard('myWizard').currentStepNumber();
            $scope.totalStepCount = WizardHandler.wizard('myWizard').totalStepCount();

            $scope.isFinalStep = $scope.currentStepNumber == $scope.totalStepCount;
        });

        $scope.formSubmitted = false;//indicates whether user has tried submitting for or not
        $scope.updateFormScope = function () {
            $scope.formScope = this;//get a reference to form object
        };

        //check if a specific field has error
        $scope.hasError = function (field) {
            var myForm = $scope.formScope.myForm;
            return $scope.formSubmitted && myForm[field].$invalid;
        };

        //check if form has an invalid field
        $scope.isValidForm = function () {
            $scope.formSubmitted = true;
            return !$scope.formScope.myForm.$invalid;
        };

        $scope.loadTaskItems = function () {
            if (task.TaskCategory && task.TaskCategory > 0) {
                TaskService.GetTaskItemList(task.TaskCategory).then(function (result) {
                    $scope.taskItems = result;
                });
            }
            return true;
        };

        $scope.setTaskItem = function (x) {
            var item = $scope.taskItems[x];
            $scope.taskItemId = item.Id;
        };

        $scope.isTaskItemSelected = function () {
            return angular.isDefined($scope.taskItemId);
        };

        $scope.canEnterStep = function () {
            var currentStep = WizardHandler.wizard('myWizard').currentStep();
            //enter this step only if previous step has been completed, or we have already been in this step
            var ret = (currentStep.wzData.number == this.wzData.number - 1 && currentStep.completed) || (this.completed == true);
            return ret;
        };

        //last step
        $scope.finishedWizard = function () {
            task.Status = Enum.ActionCategory.Publish.Key;

            if (task.TaskCategory === 2) {
                task.LearningFiles = [];
                task.LearningFiles.push({ Id: $scope.taskItemId });
            } else if (task.TaskCategory === 3) {
                task.Exams = [];
                task.Exams.push({ Id: $scope.taskItemId });
            } else if (task.TaskCategory === 5) {
                task.Questionnaires = [];
                task.Questionnaires.push({ Id: $scope.taskItemId });
            }

            TaskService.TaskCreate(task).then(function () {
                $state.go('Task.Single');
            }, function () {
                alert("任务发布失败!");
            });
        };
    }]);