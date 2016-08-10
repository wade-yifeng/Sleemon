SleemonPortal.service('TrainingService', ['$http', 'Promise', 'HttpGet', 'HttpPost', function ($http, Promise, HttpGet, HttpPost) {
    return {
        GetTrainingList: function (pageIndex) {
            return Promise(function (defer) {
                HttpGet('/Training/GetTrainingList', {
                    pageIndex: pageIndex,
                    pageSize: config.PAGE_SIZE
                }, defer);
            });
        },
        GetTraining: function (trainingId) {
            return Promise(function (defer) {
                HttpGet('/Training/GetTrainingDetail/' + trainingId, null, defer);
            });
        }
        //CreateTraining: function (training) {
        //    return Promise(function (defer) {
        //        HttpPost('/Training/CreateTraining', {
        //            training: training
        //        }, defer);
        //    });
        //},
        //DeleteTraining: function (trainingId) {
        //    return Promise(function (defer) {
        //        HttpPost('/Training/DeleteTraining', {
        //            id: trainingId
        //        }, defer);
        //    });
        //}
    };
}]);