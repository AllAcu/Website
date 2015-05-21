(function (app) {
    app.factory('verificationCommands', ['$http', function ($http) {

        return {
            start: function (patientId, verification) {
                return $http.post("api/{PatientId}/insurance/verify"
                    .replace("{PatientId}", patientId), {
                        requestDraft: verification
                    });
            },
            update: function (verificationId, verification) {
                return $http.put("api/insurance/verify", {
                    verificationId: verificationId,
                    requestDraft: verification
                });
            }
        }
    }]);
})(angular.module("loginApp"));