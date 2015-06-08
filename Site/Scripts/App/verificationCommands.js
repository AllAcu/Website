(function (app) {
    app.factory('verificationCommands', ['$http', function ($http) {

        return {
            request: {
                start: function (patientId, request) {
                    return $http.post("/api/{PatientId}/insurance/verification/request"
                        .replace("{PatientId}", patientId), {
                            requestDraft: request
                        });
                },
                submitNew: function (patientId, request) {
                    return $http.post("/api/{PatientId}/insurance/verification/submit"
                        .replace("{PatientId}", patientId), {
                            request: request
                        });
                },
                update: function (verificationId, request) {
                    return $http.put("/api/insurance/verification/{VerificationId}/request"
                        .replace("{VerificationId}", verificationId), {
                            requestDraft: request
                        });
                },
                submit: function (verificationId, request) {
                    return $http.post("/api/insurance/verification/{VerificationId}/submit"
                        .replace("{VerificationId}", verificationId), {
                            request: request
                        });
                }
            },
            verification: {
                update: function (verificationId, verification) {
                    return $http.put("/api/insurance/verification/{verificationId}"
                        .replace("{verificationId}", verificationId), {
                            verificationId: verificationId,
                            benefits: verification.benefits
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
        }
    }]);
})(angular.module("loginApp"));