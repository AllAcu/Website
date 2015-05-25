﻿(function (app) {
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
            approve: function (verificationId, verification) {
                return $http.post("/api/insurance/verification/{verificationId}/approve"
                        .replace("{verificationId}", verificationId), {
                            verificationId: verificationId,
                            benefits: verification.benefits
                        });
            }
        }
    }]);
})(angular.module("loginApp"));