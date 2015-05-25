(function (app) {
    app.factory('verificationRepository', ['$http', function ($http) {

        return {
            findAll: function () {
                return $http.get("/api/insurance/verify");
            },
            getRequest: function (id) {
                return $http.get("/api/insurance/verifyRequest/{VerificationId}"
                                    .replace("{VerificationId}", id));
            },
            getVerification: function (id) {
                return $http.get("/api/insurance/verification/{VerificationId}"
                                    .replace("{VerificationId}", id))
                            .success(function (data) {
                                var benefits = data.benefits;
                                benefits.calendarYearPlanEnd = benefits.calendarYearPlanEnd ? new Date(benefits.calendarYearPlanEnd) : null;
                                benefits.calendarYearPlanBegin = benefits.calendarYearPlanBegin ? new Date(benefits.calendarYearPlanBegin) : null;
                            });
            },
            updateVerification: function (id, verification) {
                return $http.put("/api/insurance/verification/{VerificationId}"
                                    .replace("{VerificationId}", id), {
                                        verificationId: id,
                                        benefits: verification
                                    });
            }

        }
    }]);
})(window.app);