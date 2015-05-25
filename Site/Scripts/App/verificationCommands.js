(function (app) {
    app.factory('verificationCommands', ['$http', function ($http) {

        return {
            start: function (patientId, verification) {
                return $http.post("/api/{PatientId}/insurance/verify"
                    .replace("{PatientId}", patientId), {
                        requestDraft: verification
                    });
            },
            update: function (verificationId, verification) {
                return $http.put("/api/insurance/verify", {
                    verificationId: verificationId,
                    requestDraft: verification
                });
            },
            submit: function (verificationId, verification) {
                return $http.post("/api/insurance/verify/submit", {
                    verificationId: verificationId,
                    verificationRequest: verification
                });
            },
            approve: function (verificationId, verification) {
                return $http.post("/api/insurance/verify/approve", {
                    verificationId: verificationId,
                    verification: verification
                });
            }
        }
    }]);
})(angular.module("loginApp"));