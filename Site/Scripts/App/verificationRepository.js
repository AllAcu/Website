(function (app) {
    app.factory('verificationRepository', ['$http', function ($http) {

        return {
            findAll: function () {
                return $http.get("/api/insurance/verify");
            },
            getRequest: function(id) {
                return $http.get("/api/insurance/verifyRequest/{VerificationId}"
                                    .replace("{VerificationId}", id));
            },
            getVerification: function(id) {
                return $http.get("/api/insurance/verification/{VerificationId}"
                                    .replace("{VerificationId}", id));
            }
        }
    }]);
})(window.app);