var SleemonPortal = angular.module('SleemonPortal', [
    'ngSanitize',
    'oc.lazyLoad',
    'ui.bootstrap',
    'ui.router',
    'ace.directives',
    'ngStorage'
]);

// $http验证结果处理
SleemonPortal.factory('AuthInterceptor', AuthInterceptor);

var configFunction = function ($stateProvider, $httpProvider) {
    $stateProvider
        .state('Setting',
        {
            'abstract': true,
            //url: 'Setting/Index/',
            title: '基础设置',
            template: '<ui-view/>',
            icon: 'fa fa-pencil-square-o'
        })
        .state('Setting.Connect',
        {
            url: '/Setting-Connect',
            title: '连接企业号',
            templateUrl: '/Setting/Connect/',
            controller: 'ConnectController',
            resolve: {
                lazyLoad: [
                    '$ocLazyLoad', function ($ocLazyLoad) {
                        return $ocLazyLoad.load([
                            {
                                name: 'treeControl',
                                files: ['/Assets/components/angular-tree-control/angular-tree-control.min.js']
                            },
                            {
                                serie: true,
                                name: 'dataTables',
                                files: [
                                    '/Assets/components/datatables/media/js/jquery.dataTables.min.js',
                                    '/Assets/components/datatables/jquery.dataTables.bootstrap.min.js',
                                    '/Assets/components/angular-datatables/angular-datatables.min.js'
                                ]
                            },
                            {
                                name: 'bootbox',
                                files: [
                                    '/Assets/components/bootbox.js/bootbox.min.js'
                                ]
                            }
                        ]);
                    }
                ]
            }
        })
        .state('Setting.Admin',
        {
            url: '/Setting-Admin',
            title: '管理人员',
            templateUrl: '/Setting/Admin/',
            controller: 'AdminController',
            resolve: {
                lazyLoad: [
                    '$ocLazyLoad', function ($ocLazyLoad) {
                        return $ocLazyLoad.load([
                            {
                                serie: true,
                                name: 'dataTables',
                                files: [
                                    '/Assets/components/datatables/media/js/jquery.dataTables.min.js',
                                    '/Assets/components/datatables/jquery.dataTables.bootstrap.min.js',
                                    '/Assets/components/angular-datatables/angular-datatables.min.js'
                                ]
                            },
                            {
                                name: 'ngAside',
                                files: [
                                    '/Assets/components/angular-aside/angular-aside.min.js',
                                    '/Assets/components/angular-aside/angular-aside.min.css'
                                ]
                            },
                            {
                                name: 'multiselect',
                                files: [
                                    '/Assets/components/bootstrap-multiselect/bootstrap-multiselect.min.js',
                                    '/Assets/components/bootstrap-multiselect/bootstrap-multiselect.min.css'
                                ]
                            },
                            {
                                name: 'duallistbox',
                                files: [
                                    '/Assets/components/bootstrap-duallistbox/jquery.bootstrap-duallistbox.min.js',
                                    '/Assets/components/bootstrap-duallistbox/bootstrap-duallistbox.min.css'
                                ]
                            },
                            {
                                name: 'bootbox',
                                files: [
                                    '/Assets/components/bootbox.js/bootbox.min.js'
                                ]
                            },
                            {
                                name: 'validate',
                                files: [
                                    '/Assets/components/jquery-validation/jquery.validate.min.js'
                                ]
                            }
                        ]);
                    }
                ]
            }
        })
        .state('Setting.Roles',
        {
            url: '/Setting-Roles',
            title: '角色/权限',
            templateUrl: '/Setting/Roles/'
        })
        .state('Setting.Global',
        {
            url: '/Setting-Global',
            title: '全局设置',
            templateUrl: '/Setting/Global/'
        })
        .state('Training',
        {
            'abstract': true,
            title: '培训管理',
            template: '<ui-view/>',
            icon: 'fa fa-calendar'
        })
        .state('Training.Training',
        {
            url: '/Training-Training',
            title: '培训管理',
            templateUrl: '/Training/Index/',
            controller: 'TrainingController',
            resolve: {
                lazyLoad: ['$ocLazyLoad', function ($ocLazyLoad) {
                    return $ocLazyLoad.load([{
                        serie: true,
                        files: [
                            '/Assets/components/datatables/media/js/jquery.dataTables.min.js',
                            '/Assets/components/datatables/jquery.dataTables.bootstrap.min.js',
                            '/Assets/components/angular-datatables/angular-datatables.min.js'
                        ]
                    }]);
                }]
            }
        })
        .state('Training.Training.Detail',
        {
            url: '/Detail/:trainingId',
            title: '培训详情',
            views: {
                '@': {
                    templateUrl: "/Training/Detail",
                    controller: 'TrainingDetailController'
                }
            }
        })
        .state('Training.TrainingCreate',
        {
            nested: true,
            url: '/Training-TrainingCreate',
            title: '新增/编辑 培训',
            templateUrl: '/Training/Create/',
            controller: 'TrainingCreateController',
            resolve: {
                lazyLoad: ['$ocLazyLoad', function ($ocLazyLoad) {
                    return $ocLazyLoad.load([{
                        files: [
                            '/Assets/components/jquery-inputlimiter/jquery.inputlimiter.js',
                        ]
                    }]);
                }]
            }
        })
        .state('Training.Exam',
        {
            url: '/Training-Exam',
            title: '试卷管理',
            templateUrl: "/Exam/Index/",
            controller: 'ExamController',
            resolve: {
                lazyLoad: [
                    '$ocLazyLoad', function ($ocLazyLoad) {
                        return $ocLazyLoad.load([
                            {
                                serie: true,
                                name: 'dataTables',
                                files: [
                                    '/Assets/components/datatables/media/js/jquery.dataTables.min.js',
                                    '/Assets/components/datatables/jquery.dataTables.bootstrap.min.js',
                                    '/Assets/components/angular-datatables/angular-datatables.min.js'
                                ]
                            }]);
                    }]
            }
        })
        .state('Training.Exam.Detail',
        {
            url: '/Detail/:examId',
            title: '试卷详情',
            views: {
                '@': {
                    templateUrl: "/Exam/Create",
                    controller: 'ExamDetailController'
                }
            },
            resolve: {
                lazyLoad: ['$ocLazyLoad', function ($ocLazyLoad) {
                    return $ocLazyLoad.load([{
                        files: [
                            '/Assets/components/jquery-waterfall/waterfall.min.js'
                        ]
                    }]);
                }]
            }
        })
        .state('Training.Exam.Create',
        {
            url: '/Create/:examId',
            title: '新增/编辑 试卷',
            views: {
                '@': {
                    templateUrl: "/Exam/Create",
                    controller: 'ExamCreateController'
                }
            },
            resolve: {
                lazyLoad: ['$ocLazyLoad', function ($ocLazyLoad) {
                    return $ocLazyLoad.load([
                        {
                            serie: true,
                            name: 'text_area',
                            files: [
                                '/Assets/components/jquery-inputlimiter/jquery.inputlimiter.js',
                                '/Assets/components/angular-elastic/elastic.js',
                                '/Assets/components/jquery-waterfall/waterfall.min.js',
                                '/Assets/components/bootstrap/modal.js'
                            ]
                        },
                        {
                            name: 'spinner',
                            files: ['/Assets/components/fuelux/js/spinbox.js']
                        },
                        {
                            name: 'mask',
                            files: ['/Assets/components/angular-ui-mask/dist/mask.js']
                        }]);
                }]
            }
        })
        .state('Training.Questionnaire',
        {
            url: '/Training-Questionnaire',
            title: '问卷管理',
            templateUrl: '/Questionnaire/Index/',
            controller: 'QuestionnaireController',
            resolve: {
                lazyLoad: ['$ocLazyLoad', function ($ocLazyLoad) {
                    return $ocLazyLoad.load([{
                        serie: true,
                        files: [
                            '/Assets/components/datatables/media/js/jquery.dataTables.min.js',
                            '/Assets/components/datatables/jquery.dataTables.bootstrap.min.js',
                            '/Assets/components/angular-datatables/angular-datatables.min.js'
                        ]
                    }]);
                }]
            }
        })
        .state('Training.Questionnaire.Detail',
        {
            url: '/Detail/:questionnaireId',
            title: '问卷详情',
            views: {
                '@': {
                    templateUrl: "/Questionnaire/Create",
                    controller: 'QuestionnaireDetailController'
                }
            },
            resolve: {
                lazyLoad: ['$ocLazyLoad', function ($ocLazyLoad) {
                    return $ocLazyLoad.load([{
                        files: [
                            '/Assets/components/jquery-waterfall/waterfall.min.js'
                        ]
                    }]);
                }]
            }
        })
        .state('Training.Questionnaire.Create',
        {
            url: '/Create/:questionnaireId',
            title: '新增/编辑 问卷',
            views: {
                '@': {
                    templateUrl: "/Questionnaire/Create",
                    controller: 'QuestionnaireCreateController'
                }
            },
            resolve: {
                lazyLoad: ['$ocLazyLoad', function ($ocLazyLoad) {
                    return $ocLazyLoad.load([{
                        files: [
                            '/Assets/components/jquery-inputlimiter/jquery.inputlimiter.js',
                            '/Assets/components/jquery-waterfall/waterfall.min.js',
                            '/Assets/components/bootstrap/modal.js',
                            '/Assets/components/angular-messages/angular-messages.js'
                        ]
                    }]);
                }]
            }
        })
        .state('Graphics',
        {
            'abstract': true,
            title: '图文管理',
            template: '<ui-view/>',
            icon: 'fa fa-picture-o'
        })
        .state('Graphics.Course',
        {
            url: '/Graphics-Course',
            title: '在线课程',
            templateUrl: '/Course/Index/',
            controller: 'CourseController',
            resolve: {
                lazyLoad: [
                    '$ocLazyLoad', function ($ocLazyLoad) {
                        return $ocLazyLoad.load([{
                            serie: true,
                            name: 'dataTables',
                            files: [
                                '/Assets/components/datatables/media/js/jquery.dataTables.min.js',
                                '/Assets/components/datatables/jquery.dataTables.bootstrap.min.js',
                                '/Assets/components/angular-datatables/angular-datatables.min.js'
                            ]
                        },
                        {
                            name: 'modal',
                            files: ['/Assets/components/bootstrap/modal.js']
                        },
                        {
                            name: 'text_area',
                            files: ['/Assets/components/jquery-inputlimiter/jquery.inputlimiter.js']
                        },
                        {
                            name: 'nestable',
                            files: ['/Assets/components/jquery.nestable/jquery.nestable.js']
                        },
                        {
                            name: 'ueditor',
                            files: ["/Assets/components/ueditor/ueditor.config.js",
                                "/Assets/components/ueditor/ueditor.all.js",
                                "/Assets/components/ueditor/lang/zh-cn/zh-cn.js"]
                        }]);
                    }]
            }
        })
        .state('Graphics.Course.Detail',
        {
            url: '/Detail/:courseId',
            title: '课程详情',
            views: {
                '@': {
                    templateUrl: "/Course/Detail",
                    controller: 'TaskDetailController'
                }
            }
        })
        .state('Graphics.Course.Create',
        {
            url: '/Create/:courseId',
            title: '新增/编辑 课程',
            views: {
                '@': {
                    templateUrl: "/Course/Create",
                    controller: 'CourseCreateController'
                }
            },
            resolve: {
                lazyLoad: ['$ocLazyLoad', function ($ocLazyLoad) {
                    return $ocLazyLoad.load([{
                            name: 'ngMessages',
                            files: ['/Assets/components/angular-messages/angular-messages.js']
                        },
                        {
                            name: 'wizard',
                            files: ['/Assets/components/angular-wizard/dist/angular-wizard.js']
                        },
                        {
                            name: 'text_area',
                            files: ['/Assets/components/jquery-inputlimiter/jquery.inputlimiter.js']
                        },
                        {
                            name: 'spinner',
                            files: ['/Assets/components/fuelux/js/spinbox.js']
                        },
                        {
                            name: 'datepicker',
                            files: ['/Assets/components/bootstrap-datepicker/dist/js/bootstrap-datepicker.js']
                        },
                        {
                            name: 'dataTables',
                            files: [
                                '/Assets/components/datatables/media/js/jquery.dataTables.min.js',
                                '/Assets/components/datatables/jquery.dataTables.bootstrap.min.js',
                                '/Assets/components/angular-datatables/angular-datatables.min.js'
                            ]
                        }]);
                }]
            }
        })
        .state('Graphics.QnA',
        {
            url: '/Graphics-QnA',
            title: '问答中心',
            templateUrl: '/QnA/Index/',
            controller: 'QnAController',
            resolve: {
                lazyLoad: [
                    '$ocLazyLoad', function ($ocLazyLoad) {
                        return $ocLazyLoad.load([{
                            serie: true,
                            name: 'dataTables',
                            files: [
                                '/Assets/components/datatables/media/js/jquery.dataTables.min.js',
                                '/Assets/components/datatables/jquery.dataTables.bootstrap.min.js',
                                '/Assets/components/angular-datatables/angular-datatables.min.js'
                            ]
                        },
                            {
                                name: 'text_area',
                                files: ['/Assets/components/jquery-inputlimiter/jquery.inputlimiter.js']
                            }]);
                    }]
            }
        })
        .state('Graphics.QnA.Detail',
        {
            url: '/Detail/:questionId',
            title: '问题详情',
            views: {
                '@': {
                    templateUrl: "/QnA/Detail",
                    controller: 'QnADetailController'
                }
            },
            resolve: {
                lazyLoad: [
                    '$ocLazyLoad', function ($ocLazyLoad) {
                        return $ocLazyLoad.load([
                            {
                                name: 'text_area',
                                files: ['/Assets/components/jquery-inputlimiter/jquery.inputlimiter.js']
                            }]);
                    }]
            }
        })
        .state('Graphics.QnA.Reply',
        {
            url: '/Reply/:questionId',
            title: '回复问题',
            views: {
                '@': {
                    templateUrl: "/QnA/Reply",
                    controller: 'QnAReplyController'
                }
            },
            resolve: {
                lazyLoad: [
                    '$ocLazyLoad', function ($ocLazyLoad) {
                        return $ocLazyLoad.load([
                            {
                                name: 'text_area',
                                files: ['/Assets/components/jquery-inputlimiter/jquery.inputlimiter.js']
                            }]);
                    }]
            }
        })
        .state('Graphics.Knowledge',
        {
            url: '/Graphics-Knowledge',
            title: '知识库',
            templateUrl: '/Knowledge/Index/',
            controller: 'KnowledgeController',
            resolve: {
                lazyLoad: [
                    '$ocLazyLoad', function ($ocLazyLoad) {
                        return $ocLazyLoad.load([{
                            serie: true,
                            name: 'dataTables',
                            files: [
                                '/Assets/components/datatables/media/js/jquery.dataTables.min.js',
                                '/Assets/components/datatables/jquery.dataTables.bootstrap.min.js',
                                '/Assets/components/angular-datatables/angular-datatables.min.js'
                            ]
                        }]);
                    }]
            }
        })
        .state('Graphics.Knowledge.Detail',
        {
            url: '/Detail/:knowledgeId',
            title: '知识详情',
            views: {
                '@': {
                    templateUrl: "/Knowledge/Detail",
                    controller: 'KnowledgeDetailController'
                }
            },
            resolve: {
                lazyLoad: [
                    '$ocLazyLoad', function ($ocLazyLoad) {
                        return $ocLazyLoad.load([
                            {
                                name: 'text_area',
                                files: ['/Assets/components/jquery-inputlimiter/jquery.inputlimiter.js']
                            }]);
                    }]
            }
        })
        .state('Graphics.Knowledge.Create',
        {
            url: '/Create/:knowledgeId',
            title: '新增/编辑 知识',
            params: { question: null },
            views: {
                '@': {
                    templateUrl: "/Knowledge/Create",
                    controller: 'KnowledgeCreateController'
                }
            },
            resolve: {
                lazyLoad: [
                    '$ocLazyLoad', function ($ocLazyLoad) {
                        return $ocLazyLoad.load([
                            {
                                name: 'text_area',
                                files: ['/Assets/components/jquery-inputlimiter/jquery.inputlimiter.js']
                            },
                            {
                                name: 'chosen',
                                files: ['/Assets/components/chosen/chosen.jquery.js',
                                    '/Assets/components/angular-chosen-localytics/chosen.js',
                                    '/Assets/components/chosen/chosen.css'
                                ]
                            },
                            {
                                name: 'multiselect',
                                files: ['/Assets/components/angular-bootstrap-multiselect/angular-bootstrap-multiselect.js']
                            },
                            {
                                name: 'select2',
                                files: ['/Assets/components/ui-select/dist/select.js',
                                        '/Assets/components/select2.v3/select2.css']
                            }
                        ]);
                    }]
            }
        })
        .state('Graphics.Notice',
        {
            url: '/Graphics-Notice',
            title: '资讯管理',
            templateUrl: '/EnterpriseNotice/Index/',
            controller: 'NoticeController',
            resolve: {
                lazyLoad: ['$ocLazyLoad', function ($ocLazyLoad) {
                    return $ocLazyLoad.load([{
                        serie: true,
                        files: [
                            '/Assets/components/datatables/media/js/jquery.dataTables.min.js',
                            '/Assets/components/datatables/jquery.dataTables.bootstrap.min.js',
                            '/Assets/components/angular-datatables/angular-datatables.min.js'
                        ]
                    }]);
                }]
            }
        })
        .state('Graphics.Notice.Create',
        {
            url: '/Create/:noticeId',
            title: '新增/编辑 资讯',
            views: {
                '@': {
                    templateUrl: "/EnterpriseNotice/Create",
                    controller: 'NoticeCreateController'
                }
            },
            resolve: {
                lazyLoad: ['$ocLazyLoad', function ($ocLazyLoad) {
                    return $ocLazyLoad.load([{
                        files: [
                            '/Assets/components/jquery-inputlimiter/jquery.inputlimiter.js',
                            '/Assets/components/jquery-waterfall/waterfall.min.js',
                            '/Assets/components/angular-messages/angular-messages.js'
                        ]
                    }]);
                }]
            }
        })
        .state('Graphics.ILegalComment',
        {
            url: '/Graphics-Comment',
            title: '违规评论',
            templateUrl: '/Supervision/Index/',
            controller: 'ILegalCommentController',
            resolve: {
                lazyLoad: [
                    '$ocLazyLoad', function ($ocLazyLoad) {
                        return $ocLazyLoad.load([{
                            serie: true,
                            name: 'dataTables',
                            files: [
                                '/Assets/components/datatables/media/js/jquery.dataTables.min.js',
                                '/Assets/components/datatables/jquery.dataTables.bootstrap.min.js',
                                '/Assets/components/angular-datatables/angular-datatables.min.js'
                            ]
                        },
                            {
                                name: 'text_area',
                                files: ['/Assets/components/jquery-inputlimiter/jquery.inputlimiter.js']
                            }]);
                    }]
            }
        })
        .state('Task',
        {
            'abstract': true,
            title: '任务管理',
            template: '<ui-view/>',
            icon: 'fa fa-list-alt'
        })
        .state('Task.Single',
        {
            url: '/Task-Single',
            title: '单项任务',
            templateUrl: '/Task/Index/',
            controller: 'TaskController',
            resolve: {
                lazyLoad: [
                    '$ocLazyLoad', function ($ocLazyLoad) {
                        return $ocLazyLoad.load([
                            {
                                serie: true,
                                name: 'dataTables',
                                files: [
                                    '/Assets/components/datatables/media/js/jquery.dataTables.min.js',
                                    '/Assets/components/datatables/jquery.dataTables.bootstrap.min.js',
                                    '/Assets/components/angular-datatables/angular-datatables.min.js'
                                ]
                            }]);
                    }]
            }
        })
        .state('Task.Single.Detail',
        {
            url: '/Detail/:taskId',
            title: '任务详情',
            views: {
                '@': {
                    templateUrl: "/Task/Detail/",
                    controller: 'TaskDetailController'
                }
            }
        })
        .state('Task.Single.Create',
        {
            url: '/Create/:taskId',
            title: '新增/编辑 任务',
            views: {
                '@': {
                    templateUrl: "/Task/Create/",
                    controller: 'TaskCreateController'
                }
            },
            resolve: {
                lazyLoad: ['$ocLazyLoad', function ($ocLazyLoad) {
                    return $ocLazyLoad.load([{
                        name: 'ngMessages',
                        files: ['/Assets/components/angular-messages/angular-messages.js']
                    },

                        {
                            name: 'wizard',
                            files: ['/Assets/components/angular-wizard/dist/angular-wizard.js']
                        },
                        {
                            name: 'text_area',
                            files: ['/Assets/components/jquery-inputlimiter/jquery.inputlimiter.js']
                        },
                        {
                            name: 'spinner',
                            files: ['/Assets/components/fuelux/js/spinbox.js']
                        },
                        {
                            name: 'datepicker',
                            files: ['/Assets/components/bootstrap-datepicker/dist/js/bootstrap-datepicker.js']
                        },
                        {
                            name: 'dataTables',
                            files: [
                                '/Assets/components/datatables/media/js/jquery.dataTables.min.js',
                                '/Assets/components/datatables/jquery.dataTables.bootstrap.min.js',
                                '/Assets/components/angular-datatables/angular-datatables.min.js'
                            ]
                        }]);
                }]
            }
        });

    $httpProvider.interceptors.push('AuthInterceptor');
}
configFunction.$inject = ['$stateProvider', '$httpProvider'];

SleemonPortal.config(configFunction);