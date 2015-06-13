(function (app) {
    app.service('verificationRepository', ['$http', function ($http) {

        var verifications = {};

        $http.get("/api/insurance/verification")
            .success(function (data) {
                verifications = data;
            });

        return {
            getVerification: function (id) {
                if (verifications[id] && verifications[id].request) {
                    return {
                        success: function (callback) {
                            callback(verifications[id]);
                        }
                    }
                };

                return $http.get("/api/insurance/verification/{VerificationId}".replace("{VerificationId}", id))
                            .success(function (verification) {
                                var benefits = verification.benefits;
                                benefits.calendarYearPlanEnd = benefits.calendarYearPlanEnd ? new Date(benefits.calendarYearPlanEnd) : null;
                                benefits.calendarYearPlanBegin = benefits.calendarYearPlanBegin ? new Date(benefits.calendarYearPlanBegin) : null;
                                verification.patientName = verification.patient.Name;
                                verifications[verification.verificationId] = verification;
                            });
            },
            verifications: function () {
                return verifications;
            },
            refresh: function () {
                $http.get("/api/insurance/verification")
                    .success(function (data) {
                        verifications = data;
                    });
            }
        }
    }]);
})(window.app);