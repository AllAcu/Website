(function (app) {
    app.service('verificationRepository', ['$http', function ($http) {

        var verifications = {};

        function refresh() {
            return $http.get("/api/insurance/verification")
                .success(function (data) {
                    verifications = data;
                });
        }

        function refreshAuth() {
            return $http({
                method: 'GET',
                url: '/api/insurance/verification2',
                headers: {
                    "Authorization" : "Bearer " + window.sessionStorage.getItem("accessToken")
                },
                transformRequest: function (obj) {
                    var str = [];
                    for (var p in obj)
                        str.push(encodeURIComponent(p) + "=" + encodeURIComponent(obj[p]));
                    return str.join("&");
                }
            });
        }

        refresh();

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
                return refreshAuth().success(function (data) {
                    verifications = data;
                });
            }
        }
    }]);
})(window.app);