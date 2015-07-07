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
                auth: {
                    login: function(loginData) {
                        return $http({
                            method: 'POST',
                            url: '/Token',
                            headers: {
                                'Content-Type': 'application/x-www-form-urlencoded'
                            },
                            transformRequest: function(obj) {
                                var str = [];
                                for (var p in obj)
                                    str.push(encodeURIComponent(p) + "=" + encodeURIComponent(obj[p]));
                                return str.join("&");
                            },
                            data: loginData
                        });
                    },
                    changePassword: function(oldPassword, newPassword, confirmPassword) {
                        return $httpAuth({
                            method: "POST",
                            url: "api/Account/ChangePassword",
                            data: {
                                oldPassword: oldPassword,
                                newPassword: newPassword,
                                confirmPassword: confirmPassword
                            }
                        });
                    }
                },
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
                    getAuth: function () {
                        return $httpAuth({
                            method: 'GET',
                            url: '/api/insurance/verification2'
                        });
                    }
                },
                providers: {
                    getAll: function () {
                        return $httpAuth({
                            method: "get",
                            url: "/api/provider"
                        });
                    },
                    who: function () {
                        return $http.get("/api/provider/who");
                    },
                    be: function (id) {
                        return $http.get("/api/provider/be/" + id);
                    },
                    create: function (provider) {
                        return $http.post("/api/provider/new", provider);
                    }
                },
                users: {
                    get: function(id) {
                        return $http.get("/api/user/" + id);
                    },
                    getAll: function() {
                        return $http.get("/api/user");
                    },
                    register: function(data) {
                        return $http.post('/api/Account/Register', data);
                    },
                    join: function(userId, providerId) {
                        return $http.post('/api/user/' + userId + "/join", { providerId: providerId });
                    },
                    leave: function (userId, providerId) {
                        return $http.post('/api/user/' + userId + "/leave", { providerId: providerId });
                    }
                }
            };
        }
    ]);
})(window.app);