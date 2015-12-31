(function (app) {
    app.service('$api', [
        '$http', 'authToken', function ($http, authToken) {

            var baseUrl = "http://localhost:64064/api";
            function url(url) {
                return baseUrl + url;
            }

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
                        return $http.post(url("/Account/ChangePassword"), {
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
                        return $http.get(url("/insurance/verification"));
                    },
                    get: function (verificationId) {
                        return $http.get(url("/insurance/verification/{verificationId}"
                            .replace("{verificationId}", verificationId)))
                            .success(function (verification) {
                                var benefits = verification.benefits;
                                benefits.calendarYearPlanEnd = benefits.calendarYearPlanEnd ? new Date(benefits.calendarYearPlanEnd) : null;
                                benefits.calendarYearPlanBegin = benefits.calendarYearPlanBegin ? new Date(benefits.calendarYearPlanBegin) : null;
                                verification.patientName = verification.patient.Name;
                            });
                    },
                    start: function (patientId, request) {
                        return $http.post(url("/{PatientId}/insurance/verification/request"
                            .replace("{PatientId}", patientId)), {
                                requestDraft: request
                            });
                    },
                    updateRequest: function (verificationId, request) {
                        return $http.put(url("/insurance/verification/{verificationId}/request"
                            .replace("{verificationId}", verificationId)), {
                                requestDraft: request
                            });
                    },
                    submitNewRequest: function (patientId, request) {
                        return $http.post(url("/{PatientId}/insurance/verification/submitRequest"
                            .replace("{PatientId}", patientId)), {
                                request: request
                            });
                    },
                    submitRequest: function (verificationId, request) {
                        return $http.post(url("/insurance/verification/{verificationId}/submitRequest"
                            .replace("{verificationId}", verificationId)), {
                                request: request
                            });
                    },
                    reject: function (verificationId, reason) {
                        return $http.post(url("/insurance/verification/{verificationId}/rejectRequest"
                            .replace("{verificationId}", verificationId)), {
                                reason: reason
                            });
                    },
                    delegate: function (verificationId, assignTo) {
                        return $http.post(url("/insurance/verification/{verificationId}/delegate"
                            .replace("{verificationId}", verificationId)), {
                                assignToUserId: assignTo,
                                comments: "from the chooser"
                            });
                    },
                    startCall: function (verificationId, callData) {
                        return $http.post(url("/insurance/verification/{verificationId}/startCall"
                            .replace("{verificationId}", verificationId)), callData);
                    },
                    update: function (verificationId, verification) {
                        return $http.put(url("/insurance/verification/{verificationId}"
                            .replace("{verificationId}", verificationId)), {
                                benefits: verification.benefits
                            });
                    },
                    endCall: function (verificationId, callData) {
                        return $http.post(url("/insurance/verification/{verificationId}/endCall"
                            .replace("{verificationId}", verificationId)), callData);
                    },
                    submitForApproval: function (verificationId) {
                        return $http.post(url("insurance/verification/{verificationId}/submitForApproval"
                            .replace("{verificationId}", verificationId)));
                    },
                    complete: function (verificationId, verification) {
                        return $http.post(url("/insurance/verification/{verificationId}/complete"
                            .replace("{verificationId}", verificationId)), {
                                benefits: verification.benefits
                            });
                    }
                },
                providers: {
                    get: function (id) {
                        if (id) {
                            return $http.get(url("/provider/{id}".replace("{id}", id)));
                        }
                        return $http.get(url("/provider"));
                    },
                    getAll: function () {
                        return $http.get(url("/provider/all"));
                    },
                    who: function () {
                        return $http.get(url("/provider/who"));
                    },
                    be: function (id) {
                        return $http.get(url("/provider/" + id + "/be"));
                    },
                    create: function (provider) {
                        return $http.post(url("/provider/new", provider));
                    },
                    update: function (provider) {
                        return $http.put(url("/provider/{id}".replace("{id}", provider.id), provider));
                    },
                    join: function (userId, providerId) {
                        return $http.post(url("/provider/" + providerId + "/join"), { userId: userId });
                    },
                    leave: function (userId, providerId) {
                        return $http.post(url("/provider/" + providerId + "/leave"), { userId: userId });
                    },
                    grantRole: function (userId, providerId, role) {
                        return $http.post(url("/provider/" + providerId + "/grant"), { userId: userId, roles: [role] });
                    },
                    revokeRole: function (userId, providerId, role) {
                        return $http.post(url("/provider/" + providerId + "/revoke"), { userId: userId, roles: [role] });
                    }
                },
                biller: {
                    get: function () {
                        return $http.get(url("/biller"));
                    },
                    invite: function (email, role) {
                        return $http.post(url("/user/inviteToBiller"), {
                            role: role,
                            email: email
                        });
                    },
                    grantRole: function (userId, role) {
                        return $http.post(url("/biller/grant"), { userId: userId, roles: [role] });
                    },
                    revokeRole: function (userId, role) {
                        return $http.post(url("/biller/revoke"), { userId: userId, roles: [role] });
                    }
                },
                patients: {
                    getAll: function () {
                        return $http.get(url("/patient"));
                    },
                    get: function (id) {
                        return $http.get(url("/patient/" + id));
                    },
                    edit: function (id) {
                        return $http.get(url("/patient/edit/" + id)).success(function (data) {
                            data.dateOfBirth = new Date(data.dateOfBirth);
                            return data;
                        });
                    }
                },
                users: {
                    get: function (id) {
                        return $http.get(url("/user/" + id));
                    },
                    getAll: function () {
                        return $http.get(url("/user"));
                    },
                    signup: function (email) {
                        return $http.post(url("/user/signup"), {
                            email: email
                        });
                    },
                    invite: function (providerId, email) {
                        return $http.post(url("/user/invite"), {
                            email: email,
                            organizationId: providerId
                        });
                    },
                    getInvites: function (userId) {
                        return $http.get(url("/user/{id}/invites".replace("{id}", userId)));
                    },
                    accept: function (userId, providerId) {
                        return $http.post(url("/user/{id}/accept".replace("{id}", userId)),
                        {
                            organizationId: providerId
                        });
                    },
                    register: function (token, name, password) {
                        return $http.post(url("/user/register"), {
                            name: name,
                            password: password,
                            token: token
                        });
                    },
                    getConfirmations: function () {
                        return $http.get(url("/user/confirmations"));
                    }
                }
            };
        }
    ]);
})(angular.module("app"));
