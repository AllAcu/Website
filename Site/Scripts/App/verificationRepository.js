(function (app) {
    app.factory('verificationRepository', ['$http', function ($http) {

        return {
            findAll: function () {
                return $http.get("/api/insurance/verify");
            },
            get: function(id) {
                return $http.get("/api/insurance/verify/{VerificationId}"
                                    .replace("{VerificationId}", id));
            }
        }
    }]);
})(window.app);