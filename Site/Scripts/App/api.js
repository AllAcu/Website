(function (app) {
    app.service('$api', [
        '$http', 'authToken', function ($http, authToken) {

            return {
                auth: {
                    login: function (loginData) {
                        return $http({
                            method: 'POST',
                            url: '/Token',
                            headers: {
                                'Content-Type': 'application/x-www-form-urlencoded'
                            },
                            transformRequest: function (obj) {
                                var str = [];
                                for (var p in obj)
                                    str.push(encodeURIComponent(p) + "=" + encodeURIComponent(obj[p]));
                                return str.join("&");
                            },
                            data: loginData
                        });
                    },
                    changePassword: function (oldPassword, newPassword, confirmPassword) {
                        return $http.post("api/Account/ChangePassword", {
                            oldPassword: oldPassword,
                            newPassword: newPassword,
                            confirmPassword: confirmPassword
                        }
                        );
                    },
                    loggedIn: function () {
                        return authToken.loggedIn();
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
                    update: function (verificationId, verification) {
                        return $http.put("/api/insurance/verification/{verificationId}".replace("{verificationId}", verificationId), {
                            verificationId: verificationId,
                            benefits: verification.benefits
                        }
                        );
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
                },
                verificationRequests: {
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
                providers: {
                    get: function (id) {
                        if (id) {
                            return $http.get("/api/provider/{id}".replace("{id}", id));
                        }
                        return $http.get("/api/provider");
                    },
                    getAll: function () {
                        return $http.get("/api/provider/all");
                    },
                    who: function () {
                        return $http.get("/api/provider/who");
                    },
                    be: function (id) {
                        return $http.get("/api/provider/" + id + "/be");
                    },
                    create: function (provider) {
                        return $http.post("/api/provider/new", provider);
                    },
                    update: function (provider) {
                        return $http.put("/api/provider/{id}".replace("{id}", provider.id), provider);
                    },
                    join: function (userId, providerId) {
                        return $http.post('/api/provider/' + providerId + "/join", { userId: userId });
                    },
                    leave: function (userId, providerId) {
                        return $http.post('/api/provider/' + providerId + "/leave", { userId: userId });
                    }
                },
                users: {
                    get: function (id) {
                        return $http.get("/api/user/" + id);
                    },
                    getAll: function () {
                        return $http.get("/api/user");
                    },
                    signup: function (email) {
                        return $http.post("/api/user/signup", {
                            email: email
                        });
                    },
                    invite: function (providerId, email) {
                        return $http.post("/api/user/invite", {
                            providerId: providerId,
                            email: email
                        });
                    },
                    register: function (token, name, password) {
                        return $http.post("/api/user/register", {
                            name: name,
                            password: password,
                            token: token
                        });
                    }
                }
            };
        }
    ]);
})(window.app);