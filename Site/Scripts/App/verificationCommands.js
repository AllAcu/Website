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
                return $http.put("/api/insurance/verification/{verificationId}"
                        .replace("{verificationId}", verificationId), {
                            verificationId: verificationId,
                            benefits: verification.benefits
                        });
            },
            submit: function (verificationId, verification) {
                return $http.post("/api/insurance/verify/submit", {
                    verificationId: verificationId,
                    verificationRequest: verification
                });
            },
            approve: function (verificationId, verification) {
                return $http.post("/api/insurance/verification/{verificationId}/approve"
                        .replace("{verificationId}", verificationId), {
                            verificationId: verificationId,
                            benefits: verification.benefits
                        });
            },
            revise: function (verificationId, reason) {
                return $http.post("/api/insurance/verification/{verificationId}/revise"
                        .replace("{verificationId}", verificationId), {
                            verificationId: verificationId,
                            reason: reason
                        });
            }
        }
    }]);
})(angular.module("loginApp"));