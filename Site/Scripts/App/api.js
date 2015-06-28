(function (app) {
    app.service('$api', [
        '$http', 'authToken', function ($http, authToken) {

            function $httpAuth(options) {
                return $http({
                    method: options.method,
                    url: options.url,
                    data: options.data,
                    headers: {
                        "Authorization": "Bearer " + authToken.get()
                    },
                    transformRequest: function (obj) {
                        var str = [];
                        for (var p in obj)
                            str.push(encodeURIComponent(p) + "=" + encodeURIComponent(obj[p]));
                        return str.join("&");
                    }
                });
            }

            return {
                verifications: {
                    getAll: function () {
                        return $http.get("/api/insurance/verification");
                    },
                    get: function (id) {
                        return $http.get("/api/insurance/verification/{VerificationId}".replace("{VerificationId}", id))
                            .success(function (verification) {
                                var benefits = verification.benefits;
                                benefits.calendarYearPlanEnd = benefits.calendarYearPlanEnd ? new Date(benefits.calendarYearPlanEnd) : null;
                                benefits.calendarYearPlanBegin = benefits.calendarYearPlanBegin ? new Date(benefits.calendarYearPlanBegin) : null;
                                verification.patientName = verification.patient.Name;
                            });
                    },
                    getAuth: function() {
                        return $httpAuth({
                            method: 'GET',
                            url: '/api/insurance/verification2'
                        });
                    }
                },
                providers: {
                    getAll: function() {
                        return $http.get("/api/provider");
                    },
                    who: function() {
                        return $http.get("/api/provider/who");
                    },
                    be: function(id) {
                        return $http.get("/api/provider/be/" + id);
                    },
                    create: function(provider) {
                        return $http.post("/api/provider/new", provider);
                    }
                }

            };
        }
    ]);
})(window.app);