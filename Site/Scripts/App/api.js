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
                    start: function (patientId, request) {
                        return $http.post("/api/{PatientId}/insurance/verification/request"
                            .replace("{PatientId}", patientId), {
                                requestDraft: request
                            });
                    },
                    updateRequest: function (verificationId, request) {
                        return $http.put("/api/insurance/verification/{VerificationId}/request"
                            .replace("{VerificationId}", verificationId), {
                                requestDraft: request
                            });
                    },
                    submitNewRequest: function (patientId, request) {
                        return $http.post("/api/{PatientId}/insurance/verification/submitRequest"
                            .replace("{PatientId}", patientId), {
                                request: request
                            });
                    },
                    submitRequest: function (verificationId, request) {
                        return $http.post("/api/insurance/verification/{VerificationId}/submitRequest"
                            .replace("{VerificationId}", verificationId), {
                                request: request
                            });
                    },
                    reject: function (verificationId, reason) {
                        return $http.post("/api/insurance/verification/{verificationId}/rejectRequest"
                            .replace("{verificationId}", verificationId), {
                                reason: reason
                            });
                    },
                    delegate: function (verificationId, assignTo) {
                        return $http.post("/api/insurance/verification/{verificationId}/delegate"
                            .replace("{verificationId}", verificationId), {
                                assignToUserId: assignTo,
                                comments: "from the chooser"
                            });
                    },
                    startCall: function (verificationId) {
                        return $http.post("/api/insurance/verification/{VerificationId}/startCall"
                            .replace("{verificationId}", verificationId));
                    },
                    update: function (verificationId, verification) {
                        return $http.put("/api/insurance/verification/{verificationId}"
                            .replace("{verificationId}", verificationId), {
                                benefits: verification.benefits
                            });
                    },
                    endCall: function (verificationId) {
                        return $http.post("insurance/verification/{VerificationId}/endCall")
                            .replace("{verificationId}", verificationId);
                    },
                    submitForApproval: function (verificationId) {
                        return $http.post("insurance/verification/{VerificationId}/submitForApproval")
                            .replace("{verificationId}", verificationId);
                    },
                    complete: function (verificationId, verification) {
                        return $http.post("/api/insurance/verification/{verificationId}/complete"
                            .replace("{verificationId}", verificationId), {
                                benefits: verification.benefits
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
                    },
                    grantRole: function (userId, providerId, role) {
                        return $http.post('/api/provider/' + providerId + "/grant", { userId: userId, roles: [role] });
                    },
                    revokeRole: function (userId, providerId, role) {
                        return $http.post('/api/provider/' + providerId + "/revoke", { userId: userId, roles: [role] });
                    }
                },
                biller: {
                    get: function () {
                        return $http.get("/api/biller");
                    },
                    invite: function (email, role) {
                        return $http.post("/api/user/inviteToBiller", {
                            role: role,
                            email: email
                        });
                    },
                    grantRole: function (userId, role) {
                        return $http.post('/api/biller/grant', { userId: userId, roles: [role] });
                    },
                    revokeRole: function (userId, role) {
                        return $http.post('/api/biller/revoke', { userId: userId, roles: [role] });
                    }
                },
                patients: {
                    getAll: function () {
                        return $http.get("/api/patient");
                    },
                    get: function (id) {
                        return $http.get("/api/patient/" + id);
                    },
                    edit: function (id) {
                        return $http.get("/api/patient/edit/" + id).success(function (data) {
                            data.dateOfBirth = new Date(data.dateOfBirth);
                            return data;
                        });
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
                            email: email,
                            organizationId: providerId
                        });
                    },
                    getInvites: function (userId) {
                        return $http.get("/api/user/{id}/invites".replace("{id}", userId));
                    },
                    accept: function (userId, providerId) {
                        return $http.post("/api/user/{id}/accept".replace("{id}", userId),
                        {
                            organizationId: providerId
                        });
                    },
                    register: function (token, name, password) {
                        return $http.post("/api/user/register", {
                            name: name,
                            password: password,
                            token: token
                        });
                    },
                    getConfirmations: function () {
                        return $http.get("/api/user/confirmations");
                    }
                }
            };
        }
    ]);
})(angular.module("api"));
