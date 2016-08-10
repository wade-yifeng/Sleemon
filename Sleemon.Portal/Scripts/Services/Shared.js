SleemonPortal.service('Promise', function ($q) {
    return function (logic) {
        var defer = $q.defer();
        logic(defer);
        return defer.promise;
    };
});

SleemonPortal.service('HttpGet', function ($http) {
    return function (url, data, defer) {
        $http({
            url: url,
            method: 'GET',
            params: data
        }).success(function (result) {
            defer.resolve(result);
        }).error(function (result, status) {
            defer.reject(status);
        });
    };
});

SleemonPortal.service('HttpPost', function ($http) {
    return function (url, data, defer) {
        $http({
            url: url,
            method: 'POST',
            data: data,
            headers: {
                '__RequestVerificationToken': data.antiForgeryToken
            }
        }).success(function (result) {
            defer.resolve(result);
        }).error(function (result, status) {
            defer.reject(status);
        });
    };
});