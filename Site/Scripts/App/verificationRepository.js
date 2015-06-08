(function (app) {
    app.factory('verificationRepository', ['$http', function ($http) {

        var verifications = [];

        $http.get("/api/insurance/verification")
            .success(function (data) {
                verifications = data;
            });

        return {
            getVerification: function (id) {
                return $http.get("/api/insurance/verification/{VerificationId}"
                                    .replace("{VerificationId}", id))
                            .success(function (data) {
                                var benefits = data.benefits;
                                benefits.calendarYearPlanEnd = benefits.calendarYearPlanEnd ? new Date(benefits.calendarYearPlanEnd) : null;
                                benefits.calendarYearPlanBegin = benefits.calendarYearPlanBegin ? new Date(benefits.calendarYearPlanBegin) : null;
                            });
            },
            verifications: function() {
                return verifications;
            },
            touch: function() {
                $http.get("/api/insurance/verification")
                    .success(function (data) {
                        verifications = data;
                    });
            }
        }
    }]);
})(window.app);