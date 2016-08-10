SleemonPortal.controller('ExamCreateController', ['$scope', '$stateParams', '$http', '$state', 'ExamService',
    function ($scope, $stateParams, $http, $state, ExamService) {

        //text limiter options
        $scope.text_limiter = {
            remText: '%n 字数%s 尚可输入...',
            limitText: '最大允许输入字数 : %n.'
        };

        //spinner options
        $scope.$spinner = [{ min: 0, max: 100, step: 10, on_sides: true, icon_up: 'ace-icon fa fa-plus bigger-110', icon_down: 'ace-icon fa fa-minus bigger-110', btn_up_class: 'btn-success', btn_down_class: 'btn-danger' }
            , { min: 0, max: 100, step: 5, on_sides: true, icon_up: 'ace-icon fa fa-plus bigger-110', icon_down: 'ace-icon fa fa-minus bigger-110', btn_up_class: 'btn-success', btn_down_class: 'btn-danger' }];
        //model for spinner input
        $scope.spinVal = 0;

        /* fields */
        var fields = $scope.fields = {
            examId: $stateParams.examId,
            isNew: true,
            choiceForFileInput: null,
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

        var exam = $scope.exam = {
            Status: Enum.ActionCategory.Save.Key,
            Questions: []
        }

        var question = $scope.question = {
            Choices: []
        };

        if (fields.examId != 0) {
            ExamService.GetExamDetail(fields.examId)
                .then(function (result) {
                    exam = $scope.exam = result;
                    initWaterfall(exam.Questions.length > 0);
                });
        } else {
            initWaterfall();
        }

        $scope.showEditor = function (ques) {
            fields.isNew = ques ? false : true;
            if (ques) {
                question = $scope.question = ques;
            } else {
                if (exam.Questions.length == config.MAX_QUESTION_COUNT) {
                    return showInfo('最多只能添加' + config.MAX_QUESTION_COUNT + '个问题');
                }
                question = $scope.question = { Choices: [] };
            }
            showEditor(fields.isNew);
        };

        $scope.setCurrentChoice = function (choice) {
            fields.choiceForFileInput = choice;
        };

        $scope.quesImageChanged = function () {
            if (!question.fileInput) return;
            uploadImage(question);
        }

        $scope.choiceImageChanged = function () {
            if (!fields.choiceForFileInput.fileInput) return;
            uploadImage(fields.choiceForFileInput);
        };

        function uploadImage(entity) {
            fields.fileInputProps.loading = true;

            var formData = new FormData();
            formData.append('file', entity.fileInput[0]);

            $http.post('/Exam/UpLoadImageFile', formData, {
                headers: {
                    'Content-Type': undefined,
                    'HTTP_X_REQUESTED_WITH': 'XMLHttpRequest'
                }
            }).success(function (data) {
                entity.Image = data.filePath;
                fields.fileInputProps.loading = false;
            })
            .error(function (data, status) {
                fields.fileInputProps.loading = false;
            });
        }

        $scope.addChoice = function () {
            if (question.Choices.length == config.MAX_CHOICE_COUNT) {
                return showInfo('最多只能添加' + config.MAX_CHOICE_COUNT + '个选项');
            }
            question.Choices.push({ Choice: question.Choices.length + 1 });
        };

        $scope.removeChoice = function (choice) {
            var index = question.Choices.indexOf(choice);
            question.Choices.forEach(function (item) {
                if (item.Choice > index) {
                    item.Choice -= 1;
                }
            });
            if (question.Choices.length > 0) {
                question.Choices.splice(index, 1);
            }
            refreshWaterfall();
        };

        $scope.removeQuestion = function (ques) {
            var index = exam.Questions.indexOf(ques);
            if (index > -1) {
                exam.Questions.forEach(function (item) {
                    if (item.No > index) {
                        item.No -= 1;
                    }
                });
                if (exam.Questions.length > 0) {
                    exam.Questions.splice(index, 1);
                }
                refreshWaterfall();
            }
        };

        $scope.submitQuestion = function () {
            if (fields.isNew) {
                question.No = exam.Questions.length + 1;
                exam.Questions.push(question);
            }
            hideEditor();
            refreshWaterfall();
        };

        $scope.submitExam = function (state) {
            exam.Status = state;
            console.log(exam);
            ExamService.ExamCreate(exam).then(function (result) {
                $state.go('Training.Exam');
            });
        };

        $scope.convertToChoiceChar = function (choice) {
            return String.fromCharCode(64 + choice);
        }
    }]);