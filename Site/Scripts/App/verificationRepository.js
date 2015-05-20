(function (app) {
    app.factory('verificationRepository', ['$http', function ($http) {

        return {
            findAll: function () {
                return $http.get("/api/insurance/verify");
            }
        }
    }]);
})(window.app);