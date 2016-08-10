SleemonPortal.controller('QuestionnaireCreateController', ['$scope', '$stateParams', 'QuestionnaireService', '$http', '$state',
    function ($scope, $stateParams, QuestionnaireService, $http, $state) {
        /* fields */
        var fields = $scope.fields = {
            questionnaireId: $stateParams.questionnaireId,
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
        var questionnaire = $scope.questionnaire = {
            Status: Enum.ActionCategory.Save.Key,
            Questions: []
        };
        var question = $scope.question = { Choices: [] };

        /* init */
        if (fields.questionnaireId) {
            QuestionnaireService.GetQuestionnaire(fields.questionnaireId).then(function (result) {
                questionnaire = $scope.questionnaire = result;
                initWaterfall(questionnaire.Questions.length > 0);
            });
        } else {
            initWaterfall();
        }

        /* events */
        $scope.showEditor = function (ques) {
            fields.isNew = ques ? false : true;
            if (ques) {
                question = $scope.question = ques;
            } else {
                if (questionnaire.Questions.length == config.MAX_QUESTION_COUNT) {
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
        };

        $scope.choiceImageChanged = function () {
            if (!fields.choiceForFileInput.fileInput) return;
            uploadImage(fields.choiceForFileInput);
        };

        function uploadImage(entity) {
            fields.fileInputProps.loading = true;

            var formData = new FormData();
            formData.append('file', entity.fileInput[0]);

            $http.post('/Questionnaire/UpLoadImageFile', formData,
                {
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
                if (item.Choice > index) item.Choice -= 1;
            });
            if (question.Choices.length > 0) {
                question.Choices.splice(index, 1);
            }
            refreshWaterfall();
        };

        $scope.removeQuestion = function (ques) {
            var index = questionnaire.Questions.indexOf(ques);
            questionnaire.Questions.forEach(function (item) {
                if (item.No > index) item.No -= 1;
            });
            if (questionnaire.Questions.length > 0) {
                questionnaire.Questions.splice(index, 1);
            }
            refreshWaterfall();
        };

        $scope.submitQuestion = function () {
            if (fields.isNew) {
                question.No = questionnaire.Questions.length + 1;
                questionnaire.Questions.push(question);
            }
            hideEditor();
            refreshWaterfall();
        };

        $scope.submitQuestionnaire = function () {
            QuestionnaireService.CreateQuestionnaire(questionnaire).then(function (result) {
                $state.go('Training.Questionnaire');
            });
        };

        /* helpers */
        $scope.convertToChoiceChar = function (choice) {
            return String.fromCharCode(64 + choice);
        }
    }]);