(function (app) {
    app.factory('verificationCommands', ['$http', function ($http) {

        return {
            start: function(patientId, verification) {
                $http.post("api/{PatientId}/insurance/verify"
                    .replace("{PatientId}", patientId), {
                        requestDraft: verification
                    });
            }
        }
    }]);
})(angular.module("loginApp"));