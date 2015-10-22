(function(exports, angular) {
    var app = angular.module('allAcuApp', [
        'ngRoute',
        'timer',
        'api',
        'authApp',
        'loginApp',
        'patientsApp',
        'providersApp',
        'billerApp',
        'userApp',
        'ui.bootstrap'
    ]);

    angular.module("api", []);
    angular.module("authApp", []);
    angular.module("loginApp", []);
    angular.module("patientsApp", ["verificationApp"]);
    angular.module("providersApp", ["api"]);
    angular.module("billerApp", []);
    angular.module("userApp", []);
    angular.module("claimsApp", []);
    angular.module("verificationApp", []);

    app.config([
        '$routeProvider',
        function($routeProvider) {
            $routeProvider
                .when('/login', {
                    templateUrl: '/Templates/Users/login.html',
                    controller: 'loginController',
                    anonymous: true
                })
                .when('/signup', {
                    controller: 'signupController',
                    templateUrl: '/Templates/Users/signup.html',
                    anonymous: true
                })
                .when('/register/:token', {
                    controller: 'registrationController',
                    templateUrl: '/Templates/Users/register.html',
                    anonymous: true
                })
                .when('/logout', {
                    controller: 'logoutController',
                    template: ''
                });

            $routeProvider
                .when('/patients', {
                    templateUrl: '/Templates/Patients/patientList.html',
                    controller: 'patientList',
                    canChangeProviders: true
                })
                .when('/patient/intake', {
                    templateUrl: '/Templates/Patients/intake.html',
                    controller: 'patientIntake'
                })
                .when('/patient/:id', {
                    templateUrl: '/Templates/Patients/details.html',
                    controller: 'patientDetails'
                })
                .when('/patient/:id/edit', {
                    templateUrl: '/Templates/Patients/updateVitalInfo.html',
                    controller: 'patientEdit'
                })
                .when('/patient/:id/contact/edit', {
                    templateUrl: '/Templates/Patients/updateContactInfo.html',
                    controller: 'patientEditContactInfo'
                })
                .when('/patient/:id/insurance/edit', {
                    templateUrl: '/Templates/Patients/recordInsurance.html',
                    controller: 'patientInsurance'
                })
                .when('/patient/:patientId/insurance/verification/request', {
                    templateUrl: '/Templates/Verification/verifyInsuranceRequest.html',
                    controller: 'verificationRequestCreate'
                });

            $routeProvider
                .when('/providers', {
                    templateUrl: '/Templates/Providers/list.html',
                    controller: 'providerList'
                })
                .when('/provider/create', {
                    templateUrl: '/Templates/Providers/details.html',
                    controller: 'providerCreate'
                })
                .when('/provider/:id', {
                    templateUrl: '/Templates/Providers/details.html',
                    controller: 'providerDetails'
                });

            $routeProvider
                .when('/verifications', {
                    templateUrl: '/Templates/Verification/list.html',
                    controller: 'insuranceVerificationList',
                    canChangeProviders: true
                })
                .when('/verification/:verificationId', {
                    templateUrl: '/Templates/Verification/insuranceVerification.html',
                    controller: 'verification'
                });

            $routeProvider
                .when('/biller', {
                    templateUrl: '/Templates/Biller/details.html',
                    controller: 'billerDetails'
                });

            $routeProvider
                .when('/users', {
                    templateUrl: '/Templates/Users/list.html',
                    controller: 'userListController'
                })
                .when('/user/:id', {
                    templateUrl: '/Templates/Users/details.html',
                    controller: 'userDetailsController'
                });

            $routeProvider
                .when('/claims', {
                    templateUrl: '/Templates/Claims/claimsList.html',
                    controller: 'claimsList',
                    canChangeProviders: true
                })
                .when('/claim/edit/:id', {
                    templateUrl: '/Templates/Claims/claimEdit.html',
                    controller: 'claimEdit'
                })
                .when('/claim/create', {
                    templateUrl: '/Templates/Claims/claimEdit.html',
                    controller: 'claimCreate'
                });

            $routeProvider
                .otherwise({
                    redirectTo: '/patients'
                });
        }
    ]).run([
        '$rootScope', '$location', '$http', function($rootScope, $location, $http) {
            $rootScope.$on('$routeChangeStart', function(ev, next, curr) {
                if (next.$$route) {
                    if (next.$$route.anonymous) {
                        return;
                    }

                    if (!userLoggedIn()) {
                        $location.path('/login');
                    }
                }
            });
            $rootScope.copyrightYear = new Date().getFullYear();
            $http.defaults.headers.common.Authorization = getAuthHeader;
        }
    ]);

    function userLoggedIn() {
        var authTokenService = angular.injector(['authApp']);
        return authTokenService.get('authToken').loggedIn();
    }

    function getAuthHeader() {
        var authToken = angular.injector(['authApp']).get('authToken');
        if (authToken.loggedIn()) {
            return 'Bearer ' + authToken.get();
        }
    }

    app.controller('nav', [
        '$scope', 'authToken', function($scope, authToken) {
            var _loginNavItems = [
                { label: "Login", link: "/#/login" },
                { label: "Sign up", link: "/#/signup" }
            ];
            var _navItems = [
                { label: "Verifications", link: "/#/verifications" },
                { label: "Patients", link: "/#/patients" },
                { label: "Users", link: "/#/users" },
                { label: "Claims", link: "/#/claims" },
                { label: "Providers", link: "/#/providers" },
                { label: "Biller", link: "/#/biller" }
            ];

            $scope.navItems = function() {
                if (!$scope.loggedIn()) {
                    return _loginNavItems;
                }

                return _navItems;
            }

            $scope.loggedIn = function() {
                return authToken.loggedIn();
            }
        }
    ]);

    app.controller('providerChooser', [
        '$scope', '$route', 'authToken', 'careProviderRepository', function($scope, $route, authToken, providers) {
            $scope.providers = function() { return providers.providers() || []; }
            $scope.currentProvider = function(provider) {
                return providers.current(provider);
            }

            $scope.setProvider = function() {
                providers.setCurrent($scope.currentProvider.id);
            }

            $scope.$watch(authToken.loggedIn, function() {
                providers.refresh();
            });

            $scope.canChoose = function() {
                return $route.current && $route.current.$$route && $route.current.$$route.canChangeProviders && authToken.loggedIn() && $scope.providers().length > 1;
            }

            $scope.shouldDisplay = function() {
                return authToken.loggedIn() && !!$scope.providers().length;
            }
        }
    ]);

    exports.app = app;

})(window, angular);
(function (app) {
    app.service('$api', [
        '$http', 'authToken', function ($http, authToken) {

            var baseUrl = "http://allacu.dev/api";
            function url(url) {
                return baseUrl + url;
            }

            return {
                auth: {
                    login: function (loginData) {
                        return $http({
                            method: 'POST',
                            url: '/api/Token',
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
                        return $http.get(url("/insurance/verification/{verificationId}".replace("{verificationId}"), verificationId))
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
                        return $http.get(url("/provider/{id}/be".replace("{id}", id)));
                    },
                    create: function (provider) {
                        return $http.post(url("/provider/new"), provider);
                    },
                    update: function (provider) {
                        return $http.put(url("/provider/{id}".replace("{id}", provider.id), provider));
                    },
                    join: function (userId, providerId) {
                        return $http.post(url("/provider/{id}/join".replace("{id}", providerId)), { userId: userId });
                    },
                    leave: function (userId, providerId) {
                        return $http.post(url("/provider/{id}/leave".replace("{id}", providerId)), { userId: userId });
                    },
                    grantRole: function (userId, providerId, role) {
                        return $http.post(url("/provider/{id}/grant".replace("{id}", providerId)), { userId: userId, roles: [role] });
                    },
                    revokeRole: function (userId, providerId, role) {
                        return $http.post(url("/provider/{id}/revoke".replace("{id}", providerId)), { userId: userId, roles: [role] });
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
})(angular.module("api"));

(function (app) {
    app.factory('authToken', function () {

        var authToken = {
            loggedIn: function() {
                return !!authToken.get();
            },
            get: function() {
                return window.sessionStorage.getItem("accessToken");
            },
            set: function(token) {
                window.sessionStorage.setItem("accessToken", token);
            },
            clear: function() {
                window.sessionStorage.removeItem("accessToken");
            }
        };

        return authToken;
    });
}(angular.module("authApp")));
(function (module) {

    module.controller('billerDetails', [
        "$scope", "$routeParams", "$location", "$api", function ($scope, $routeParams, $location, $api) {
            $scope.biller = {};
            $scope.users = function () { return $scope.biller.users; };

            $scope.refresh = function () {
                $api.biller.get()
                    .success(function (data) {
                        $scope.biller = data;
                    });
            }

            $scope.refresh();
        }]);

    module.controller('billerInvite', [
        '$scope', '$api', function ($scope, $api) {

            $scope.invite = function () {
                $api.biller.invite($scope.email, "employee").success(function () {
                    console.log("invited " + $scope.email);
                });
            }
        }
    ]);

    module.controller('billerPermissions', [
        "$scope", "$api", function ($scope, $api) {
            $scope.users = $scope.$parent.users;
            var refresh = $scope.$parent.refresh;

            $scope.roles = [{
                label: "Verifier",
                name: "verifier"
            }, {
                label: "Approver",
                name: "approver"
            }];

            $scope.grant = function (user, role) {
                $api.biller.grantRole(user.user.userId, role.name).success(function (data) {
                    refresh();
                });
            }
            $scope.revoke = function (user, role) {
                $api.biller.revokeRole(user.user.userId, role).success(function (data) {
                    refresh();
                });
            }
        }
    ]);
}(angular.module("billerApp")));
(function (app) {
    app.factory('careProviderRepository', ['$api', 'userSession', function ($api, session) {

        function setCurrentProvider(id) {
            var previous = session().currentProvider;
            session().currentProvider = session().providers.filter(function (p) { return p.id === id })[0];

            if (previous !== session().currentProvider) {
                $api.providers.be(session().currentProvider.id).success(function () {
                    location.reload();
                });
            }
        }

        function refresh() {
            return $api.providers.get()
                .success(function (data) {
                    session().providers = data;
                    if (session().providers && session().providers.length) {
                        $api.providers.who()
                            .success(function (current) {
                                session().currentProvider = session().providers.filter(function (p) { return p.id === current })[0];
                                if (!session().currentProvider) {
                                    setCurrentProvider(session().providers[0].id);
                                }
                            });
                    }
                })
            .error(function () {
                session().providers = [];
            });
        }

        refresh();

        return {
            providers: function () {
                if (!session().providers) {
                    session().providers = [];
                    refresh();
                }
                return session().providers;
            },
            current: function (provider) {
                if (provider) {
                    setCurrentProvider(provider.id);
                }
                return session().currentProvider;
            },
            edit: function(id) {
                return $api.providers.get(id);
            },
            setCurrent: setCurrentProvider,
            create: function (provider) {
                return $api.providers.create(provider);
            },
            refresh: function () {
                return refresh();
            }
        };
    }]);
})(window.app);
(function (module) {

    module.controller('claimsList', [
        "$scope", "claimsRepository", function ($scope, $claims) {
            $scope.drafts = [];

            $claims.findAll().success(function (data) {
                $scope.drafts = data.map(function (item) {
                    item.dateOfService = new Date(item.dateOfService);
                    return item;
                });
            });
        }
    ]);

    module.controller('claimEdit', [
        "$scope", "$routeParams", "$location", "claimsRepository", function ($scope, $routeParams, $location, $claims) {

            $scope.draft = {};
            $claims.find($routeParams["id"]).success(function (data) {
                data.dateOfService = new Date(data.dateOfService);
                $scope.draft = data;
            });

            $scope.save = function () {
                $claims.update($scope.draft).success(function () {
                    $location.path("/");
                });
            }

            $scope.submit = function () {
                $claims.submit($scope.draft).success(function () {
                    $location.path("/");
                });
            }
        }
    ]);

    module.controller('claimCreate', [
        "$scope", "$routeParams", "$location", "claimsRepository", function ($scope, $routeParams, $location, $claims) {

            $scope.draft = {};

            $scope.save = function () {
                $claims.create($scope.draft).success(function () {
                    $location.path("/");
                });
            }
        }
    ]);

}(angular.module("claimsApp")));

(function (app) {
    app.factory('claimsRepository', ['$http', function ($http) {

        var claimRepository = {
            findAll: function () {
                return $http.get("/api/claim")
                    .success(function (data) {
                        data.forEach(function (item) {
                            item.visit.dateOfService = new Date(item.visit.dateOfService).toDateString();
                        });
                        return data;
                    });
            },
            find: function (id) {
                return $http.get("/api/claim/" + id)
                    .success(function (data) {
                        data.visit.dateOfService = new Date(data.visit.dateOfService);
                        return data;
                    });
            },
            create: function (draft) {
                return $http.post("/api/claim", draft);
            },
            update: function (draft) {
                return $http.put("/api/claim/", draft);
            },
            submit: function (draft) {
                return $http.post("/api/claim/submit/" + draft.id);
            }
        }

        return claimRepository;
    }]);
})(window.app);
(function (app) {

    app.directive('displayField', function () {
        return {
            restrict: "E",
            scope: {
                ngModel: "=ngModel",
                label: "@label"
            },
            template: '<div class="row"><div class="displayLabel"><label>{{label}}</label></div><div class="displayValue">{{ngModel}}</div></div>'
        };
    });

    app.directive('editField', function () {
        return {
            restrict: "E",
            scope: {
                label: "@label",
                ngModel: "=ngModel",
                type: "@type"
            },
            template: '<div class="row"><div class="col-md-3"><label>{{label}}</label></div><div class="col-md-5"><input type="{{type}}" ng-model="ngModel" /></div></div>'
        };
    });

    app.directive('editText', function () {
        return {
            restrict: "E",
            scope: {
                label: "@label",
                ngModel: "=ngModel"
            },
            template: '<div class="row"><div class="col-md-2"><label>{{label}}</label></div><div class="col-md-4"><textarea style="width:100%;max-width:100%;" ng-model="ngModel"></textarea></div></div>'
        };
    });

    app.directive('editChoice', function () {
        return {
            restrict: "E",
            require: 'ngModel',
            scope: {
                label: "@label",
                ngModel: "=ngModel",
                options: "=options"
            },
            template: '<div class="row">' +
                '<div class="col-md-2">' +
                '<label>{{label}}</label>' +
                '</div>' +
                '<label class="col-md-3" ng-repeat="(key, value) in options">' +
                '<input type="radio" ng-model="$parent.ngModel" value="{{value.value}}">{{value.label}}' +
                '</label>' +
                '</div>'
        };
    });

    app.directive('editYesNo', function () {
        return {
            restrict: "E",
            require: 'ngModel',
            scope: {
                label: "@label",
                ngModel: "=ngModel"
            },
            template: '<div class="row">' +
                '<div class="col-md-2">' +
                '<label>{{label}}</label>' +
                '</div>' +
                '<label class="col-md-3">' +
                '<input type="radio" ng-model="ngModel" ng-value="true">Yes' +
                '</label>' +
                '<label class="col-md-3">' +
                '<input type="radio" ng-model="ngModel" ng-value="false">No' +
                '</label>' +
                '</div>'
        };
    });

    app.directive('displayAddress', function () {
        return {
            restrict: "E",
            scope: {
                address: "=ngModel"
            },
            template: '<div class="row">' +
                '<div class="col-md-3">' +
                '<label>Address</label>' +
                '</div>' +
                '<div class="col-md-4">' +
                '<div>{{address.address1}}</div>' +
                '<div>{{address.address2}}</div>' +
                '<div>{{address.city}}, {{address.state}} {{address.postalCode}}</div>' +
                '</div>' +
                '</div>'
        };
    });

    app.directive('editAddress', function () {
        return {
            restrict: "E",
            scope: {
                address: "=ngModel"
            },
            template: '<div>' +
                '<edit-field label="Address" ng-model="address.address1"></edit-field>' +
                '<edit-field ng-model="address.address2"></edit-field>' +
                '<edit-field label="City" ng-model="address.city"></edit-field>' +
                '<edit-field label="State" ng-model="address.state"></edit-field>' +
                '<edit-field label="Postal Code" ng-model="address.postalCode"></edit-field>' +
                '</div>'
        };
    });

    app.directive('question', function () {
        return {
            restrict: 'E',
            require: 'ngModel',
            scope: {
                label: "@label",
                ngModel: "=ngModel"
            },
            replace: true,
            transclude: true,
            template: '<fieldset class="panel panel-default question">' +
                '   <div class="panel-heading">' +
                '       <div class="checkbox">' +
                '           <label>' +
                '               <input type="checkbox" ng-model="ngModel"/>{{label}} ({{ngModel ? "yes" : "no"}})' +
                '           </label>' +
                '       </div>' +
                '   </div>' +
                '   <div class="panel-body" ng-show="ngModel" ng-transclude>' +
                '   </div>' +
                '</fieldset>'
        };
    });

})(window.app);

(function (module) {

    module.controller('loginController', [
        '$scope', 'userCommands', 'authToken', '$location', function ($scope, commands, authToken, $location) {

            $scope.login = function () {
                commands.login($scope.userName, $scope.password)
                    .success(function (data) {
                        authToken.set(data.access_token);
                        $location.path("/");
                    });
            }
        }
    ]);

    module.controller('logoutController', [
        '$scope', 'authToken', 'userSession', '$location', function ($scope, authToken, session, $location) {
            authToken.clear();
            session().logout();
            $location.path("/");
        }
    ]);

}(angular.module("loginApp")));
(function (app) {
    app.factory('patientCommands', ['$http', function ($http) {

        return {
            intake: function (patient) {
                return $http.post("/api/patient/", patient);
            },
            addInsurance: function (patientId, insurance) {
                return $http.post("/api/patient/" + patientId + "/insurance", {
                    medicalInsurance: insurance.classification === "medical" ? insurance.medical : null,
                    personalInjuryProtection: insurance.classification === "pip" ? insurance.pip : null
                });
            },
            update: function (patient) {
                return $http.put("/api/patient/" + patient.patientId, {
                    name: patient.name,
                    gender: patient.gender,
                    dateOfBirth: patient.dateOfBirth
                });
            },
            updateContactInfo: function (patientId, contact) {
                return $http.put("/api/patient/" + patientId + "/contact", {
                    address: {
                        line1: contact.address.address1,
                        line2: contact.address.address2,
                        city: contact.address.city,
                        state: contact.address.state,
                        postalCode: contact.address.postalCode
                    },
                    phoneNumber: contact.phoneNumber
                });
            }
        }
    }]);
})(window.app);
(function (app) {
    app.factory('patientRepository', ['$api', function ($api) {

        return {
            findAll: function () {
                return $api.patients.getAll();
            },
            details: function (id) {
                return $api.patients.get(id);
            },
            edit: function (id) {
                return $api.patients.edit(id);
            }
        }
    }]);
})(window.app);
(function (module) {

    module.controller('patientIntake', [
        "$scope", "$location", "patientCommands", function ($scope, $location, $commands) {
            $scope.patient = {};

            $scope.save = function () {
                $commands.intake($scope.patient).success(function () {
                    $location.path("/patient");
                });
            }
        }
    ]);

    module.controller('patientList', [
        "$scope", "patientRepository", function ($scope, $patients) {
            $patients.findAll().success(function (data) {
                $scope.patients = data;
            });


            $scope.dismiss = function (patient) {
                alert("deleting " + patient.name);
            }
        }
    ]);

    module.controller('patientDetails', [
        "$scope", "$routeParams", "patientRepository", function ($scope, $routeParams, $patients) {

            $scope.patient = { };
            $patients.details($routeParams["id"]).success(function (data) {
                $scope.patient = data;
            });

            $scope.insuranceLinks = {
                create: function () {
                    return $scope.patient.currentVerification && (!$scope.patient.currentVerification.status ||
                        $scope.patient.currentVerification.status === "Approved");
                },
                editRequest: function () {
                    return $scope.patient.currentVerification &&
                        $scope.patient.currentVerification.status === "Draft";
                },
                viewSubmitted: function () {
                    return $scope.patient.currentVerification &&
                        $scope.patient.currentVerification.status === 'Submitted';
                },
                viewVerification: function () {
                    return $scope.patient.currentVerification &&
                        $scope.patient.currentVerification.status === 'Approved';
                }
            }
        }
    ]);

    module.controller('patientEdit', [
        "$scope", "$routeParams", "$location", "patientRepository", "patientCommands", function ($scope, $routeParams, $location, $patients, $commands) {

            $scope.patient = {};
            $patients.edit($routeParams["id"]).success(function (data) {
                $scope.patient = data;
            });

            $scope.save = function () {
                $commands.update($scope.patient).success(function () {
                    $location.path("/patient/" + $scope.patient.patientId);
                });
            }
        }
    ]);

    module.controller('patientEditContactInfo', [
    "$scope", "$routeParams", "$location", "patientRepository", "patientCommands", function ($scope, $routeParams, $location, $patients, $commands) {

        var patientId = $routeParams["id"];
        $scope.contact = {};
        $patients.edit(patientId).success(function (data) {
            $scope.contact = {
                address: {
                    address1: data.address1,
                    address2: data.address2,
                    city: data.city,
                    state: data.state,
                    postalCode: data.postalCode
                },
                phoneNumber: data.phoneNumber
            }
        });

        $scope.save = function () {
            $commands.updateContactInfo(patientId, $scope.contact).success(function () {
                $location.path("/patient/" + patientId);
            });
        }
    }
    ]);

    module.controller('patientInsurance', [
        "$scope", "$routeParams", "$location", "patientRepository", "patientCommands", function ($scope, $routeParams, $location, $patients, $commands) {

            $scope.patientId = $routeParams["id"];
            $scope.classification = "medical";

            $patients.edit($routeParams["id"]).success(function (data) {
                $scope.medical = data.medicalInsurance || {};
                $scope.pip = data.personalInjuryProtection || {};
            });

            $scope.save = function () {
                $commands.addInsurance($scope.patientId, $scope).success(function () {
                    $location.path("/patient/" + $scope.patientId);
                });
            }

            $scope.insuranceTemplate = function () {
                if ($scope.classification === "medical")
                    return '/Templates/Patients/inputMedicalInsurance.html';
                else if ($scope.classification === "pip")
                    return '/Templates/Patients/inputPersonalInjuryProtection.html';
                return "";
            }
        }
    ]);

}(angular.module("patientsApp")));

(function (module) {

    module.controller('providerList', [
        '$scope', 'careProviderRepository', function ($scope, $providers) {
            $scope.providers = function () { return $providers.providers(); };

            $scope.link = function (provider) {
                return "#provider/" + provider.id;
            };
        }
    ]);

    module.controller('providerCreate', [
        "$scope", "$location", "$api", function ($scope, $location, $api) {
            $scope.provider = {};
            $scope.save = function () {
                $api.providers.create($scope.provider).success(function () {
                    $location.path("/providers");
                });
            }
        }]);

    module.controller('providerDetails', [
        "$scope", "$routeParams", "$location", "$api", "careProviderRepository", function ($scope, $routeParams, $location, $api, $providers) {
            $scope.provider = {};
            $scope.users = function () { return $scope.provider.users; };

            function refresh() {
                $providers.edit($routeParams["id"])
                    .success(function (data) {
                        $scope.provider = data;
                    });
            }

            refresh();

            $scope.save = function () {
                $api.providers.update($scope.provider).success(function () {
                    $providers.refresh();
                    $location.path("/providers");
                });
            }
            $scope.refresh = function () {
                $providers.refresh();
                refresh();
            }
        }]);

    module.controller('providerPermissions', [
        "$scope", "$api", function ($scope, $api) {
            var provider = function () { return $scope.$parent.provider; }
            $scope.users = $scope.$parent.users;
            var refresh = $scope.refresh = $scope.$parent.refresh;
            $scope.roles = [
            {
                label: "Owner",
                name: "owner"
            }, {
                label: "Practitioner",
                name: "practitioner"
            }];

            $scope.grant = function (user, role) {
                $api.providers.grantRole(user.user.userId, provider().id, role.name).success(function (data) {
                    refresh();
                });
            }
            $scope.revoke = function (user, role) {
                $api.providers.revokeRole(user.user.userId, provider().id, role).success(function (data) {
                    refresh();
                });
            }
        }
    ]);
}(angular.module("providersApp")));

(function (app) {
    app.factory('userCommands', ['$api', function ($api) {

        return {
            login: function (userName, password) {
                var loginData = {
                    grant_type: 'password',
                    username: userName,
                    password: password
                };

                return $api.auth.login(loginData);
            },
            signup: function(email) {
                return $api.users.signup(email);
            },
            invite: function (provider, email) {
                return $api.users.invite(provider, email);
            },
            register: function (token, name, password) {
                return $api.users.register(token, name, password);
            }
        }
    }]);
})(angular.module("loginApp"));
(function (app) {
    app.factory('userSession',
        function () {
            var session = reset();

            function reset() {
                return {
                    logout: function() {
                        session = reset();
                    }
                };
            }

            return function() {
                return session;
            }
        }
    );
})(window.app);

(function (module) {

    module.controller('registrationController', [
        '$scope', '$routeParams', '$location', 'userCommands', function ($scope, $routeParams, $location, commands) {

            var token = $routeParams["token"];
            $scope.registration = {};

            $scope.save = function () {
                if ($scope.password !== $scope.confirmPassword) {
                    return;
                }

                commands.register(token, $scope.registration.name, $scope.registration.password)
                    .success(function () {
                        console.log("registered " + $scope.name);
                        $location.path("/");
                    });
            }
        }
    ]);

    module.controller('userListController', [
        '$scope', '$api', function ($scope, $api) {
            var users = [];
            $scope.users = function () { return users; };

            $scope.link = function (user) {
                return "#user/" + user.userId;
            };

            $api.users.getAll().success(function (data) {
                users = data;
            });
        }
    ]);

    module.controller('userDetailsController', [
        '$scope', '$routeParams', '$api', "careProviderRepository", function ($scope, $routeParams, $api, providerRepo) {
            var userId = $routeParams["id"];
            $scope.user = null;
            $scope.providers = function () { return $scope.user ? $scope.user.providerRoles.map(function (r) { return r.provider; }) : []; };
            $scope.billers = function () { return $scope.user ? $scope.user.billerRoles.map(function (r) { return r.biller; }) : []; };
            $scope.providerInvitations = function () {
                return $scope.user ? $scope.user.providerInvitations : [];
            };
            $scope.billerInvitations = function () {
                return $scope.user ? $scope.user.billerInvitations : [];
            };
            $scope.hasProvider = function (provider) {
                return $scope.user && $scope.providers().some(function (p) { return p.id === provider.id; });
            }

            function refreshUser() {
                $api.users.get(userId)
                    .success(function (data) {
                        $scope.user = data;
                    });
                providerRepo.refresh();
            }

            refreshUser();

            $scope.accept = function (invite) {
                $api.users.accept(userId, invite.organization.id)
                    .success(function () {
                        refreshUser();
                    });
            }

            $scope.leave = function (provider) {
                $api.providers.leave(userId, provider.id)
                    .success(function () {
                        refreshUser();
                    });
            }
        }
    ]);

    module.controller('inviteController', [
        '$scope', '$routeParams', '$location', 'userCommands', function ($scope, $routeParams, $location, userCommands) {

            var providerId = $routeParams["id"];

            $scope.invite = function () {
                userCommands.invite(providerId, $scope.email).success(function () {
                    console.log("invited " + $scope.email);
                    $location.path("/providers");
                });
            }
        }
    ]);

    module.controller('signupController', [
        '$scope', '$routeParams', '$location', 'userCommands', function ($scope, $routeParams, $location, userCommands) {
            $scope.signup = function () {
                userCommands.signup($scope.email).success(function () {
                    console.log("signed up " + $scope.email);
                    $location.path("/");
                });
            }
        }
    ]);

    module.controller('userChooser', [
        '$scope', '$api', function ($scope, $api) {
            var users = [];
            $scope.users = function () { return users; };

            $api.users.getAll().success(function (data) {
                users = data;
            });
        }
    ]);

    module.controller('roleChooser', [
        "$scope", function ($scope) {
            $scope.roles = function (user) {
                return $scope.$parent.roles.filter(function (role) {
                    return !user.roles.some(function (r) { return r === role.name; });
                });
            }

            $scope.grant = $scope.$parent.grant;
        }
    ]);


    module.controller('oustandingInvites', [
        '$scope', '$routeParams', '$location', '$api', function ($scope, $routeParams, $location, $api) {

            $scope.confirmations = [];
            $api.users.getConfirmations().success(function (data) {
                $scope.confirmations = data;
            });

            $scope.link = function (confirmation) {
                return "#register/" + confirmation.token;
            }
        }
    ]);

}(angular.module("userApp")));
(function (app) {
    app.service('verificationRepository', ['$api', 'userSession', function ($api, session) {

        function refresh() {
            if (!session().verifications) {
                session().verifications = {};
            }
            return $api.verifications.getAll()
                .success(function (data) {
                    session().verifications = data;
                });
        }

        refresh();

        return {
            getVerification: function (id) {
                if (session().verifications && session().verifications[id] && session().verifications[id].request) {
                    return {
                        success: function (callback) {
                            callback(session().verifications[id]);
                        }
                    }
                };

                return $api.verifications.get(id)
                            .success(function (verification) {
                                session().verifications[verification.verificationId] = verification;
                            });
            },
            verifications: function () {
                if (!session().verifications) {
                    session().verifications = {};
                    refresh();
                }
                return session().verifications;
            }
        }
    }]);
})(window.app);
(function (module) {

    module.controller('verificationRequestCreate', [
        "$scope", "$routeParams", "$location", "$api", function ($scope, $routeParams, $location, $api) {
            var patientId = $routeParams["patientId"];

            $scope.save = function () {
                $api.verifications.start(patientId, $scope.request).success(function (response) {
                    $location.path("/verification/" + response);
                });
            }
            $scope.submit = function () {
                $api.verifications.submitNewRequest(patientId, $scope.request)
                    .success(function () {
                        $location.path("/patient/" + patientId);
                    });
            };
        }
    ]);

    module.controller('verification', [
        "$scope", "$routeParams", "$location", "$modal", "verificationRepository", "$api", function ($scope, $routeParams, $location, $modal, verifications, $api) {
            var verificationId = $routeParams["verificationId"];
            var patientId;

            verifications.getVerification(verificationId)
                .success(function (data) {
                    $scope.verification = data;
                    patientId = data.patientId;
                });

            $scope.verificationTemplate = function () {

                switch ($scope.verification && $scope.verification.status) {
                    case "Draft":
                    case "Rejected":
                        return "/Templates/Verification/verifyInsuranceRequest.html";
                    case "Submitted":
                        return "/Templates/Verification/verifyInsurance.html";
                    case "Assigned":
                        return "/Templates/Verification/displayRequest.html";
                    case "In Progress":
                        return "/Templates/Verification/verifyInsurance.html";
                    case "PendingApproval":
                        return "/Templates/Verification/verifyInsurance.html";
                    case "Verified":
                        return "/Templates/Verification/insuranceVerificationLetter.html";
                }

                return "";
            }

            var actions = {
                assign: { name: "Assign", handler: "startAssignment" },
                startCall: { name: "Start Call", handler: "startCall" },
                endCall: { name: "End Call", handler: "endCall" },
                save: { name: "Save", handler: "save" },
                saveDraft: { name: "Save", handler: "saveDraft" },
                approve: { name: "Approve", handler: "dummy" },
                reject: { name: "Reject", handler: "dummy" }
            }

            $scope.availableActions = function () {
                switch ($scope.verification && $scope.verification.status) {
                    case "Draft":
                    case "Rejected":
                        return [actions.saveDraft];
                    case "Submitted":
                        return [actions.assign];
                    case "Assigned":
                        return [actions.startCall, actions.assign];
                    case "In Progress":
                        return [actions.save, actions.endCall];
                    case "PendingApproval":
                        return [actions.approve, actions.reject];
                    case "Verified":
                        return [];
                }
                return [];
            }

            $scope.dummy = function () {
                console.log("Dummy callback");
            }

            $scope.handleClick = function (method) {
                $scope[method]();
            }

            $scope.saveDraft = function () {
                $api.verifications.updateRequest(verificationId, $scope.request);
            };

            $scope.submit = function () {
                $api.verifications.submitRequest(verificationId, $scope.request)
                    .success(function () {
                        $location.path("/patient/" + patientId);
                    });
            };

            $scope.startAssignment = function () {
                popup("/Templates/Verification/assign.html");
            }

            $scope.startCall = function () {
                popup("/Templates/Verification/startCall.html");
            }

            $scope.endCall = function () {
                popup("/Templates/Verification/endCall.html");
            }

            function popup(templateUrl) {
                var modalInstance = $modal.open({
                    animation: true,
                    templateUrl: '/Templates/Verification/actions.html',
                    controller: 'verificationActions',
                    size: 'lg',
                    resolve: {
                        verification: function () {
                            return $scope.verification;
                        },
                        template: function () {
                            return templateUrl;
                        },
                        patientName: function () {
                            return $scope.verification.patient.patientName;
                        }
                    }
                });

                modalInstance.result.then(function (result) {
                    console.dir(result);
                }, function () {
                    console.info('Modal dismissed at: ' + new Date());
                });
            }
        }
    ]);

    module.controller('insuranceVerificationList', [
        "$scope", "verificationRepository", function ($scope, repo) {

            $scope.verifications = repo.verifications;

            $scope.link = function (verification) {
                return "#verification/" + verification.verificationId;
            };

            $scope.refresh = function () {
                repo.refresh();
            }
        }
    ]);

    module.controller('verificationActions', [
        "$scope", "$api", "verification", "patientName", "template", function ($scope, $api, verification, patientName, template) {
            var verificationId = verification.id;
            $scope.verification = verification;
            $scope.actionTemplate = template;
            $scope.patientName = patientName;

            $scope.complete = function () {
                $api.verifications.complete(verificationId, $scope.verification).success(function () {
                    $location.path("/verifications");
                });
            }

            $scope.select = function (user) {
                $scope.selectedUser = user;
            };

            $scope.reject = function () {
                $api.verifications.reject(verificationId, $scope.verification).success(function () {
                    $location.path("/patient/" + patientId);
                });
            }

            $scope.assign = function () {
                $api.verifications.delegate(verificationId, $scope.selectedUser.userId);
            }
        }
    ]);

    module.controller('verification.startCall', [
        "$scope", "$location", "$api", function ($scope, $location, $api) {
            var verificationId = $scope.$parent.verification.verificationId;
            $scope.startCall = function () {

                console.log("starting call: " + verificationId);

                $api.verifications.startCall(verificationId, {
                    serviceCenterRepresentative: $scope.serviceCenterRepresentative
                }).success(function () {
                    $location.path("/verification/" + verificationId);
                });
            }
        }
    ]);

    module.controller('verification.onCall', [
        "$scope", "$api", function ($scope, $api) {
            $scope.verification = $scope.$parent.verification;
            var verificationId = $scope.$parent.verification.verificationId;
            $scope.callStart = function () {
                return new Date($scope.verification.currentCallStartTime).getTime();
            }

            $scope.save = function () {
                $api.verifications.update(verificationId, $scope.verification).success(function () {
                    console.log("saved " + verificationId);
                });
            }
        }
    ]);

    module.controller('verification.endCall', [
        "$scope", "$api", function ($scope, $api) {
            $scope.verification = $scope.$parent.verification;
            var verificationId = $scope.$parent.verification.verificationId;
            $scope.serviceCenterRepresentative = $scope.verification.serviceCenterRepresentative;

            $scope.endCall = function () {
                $api.verifications.endCall(verificationId, {
                    serviceCenterRepresentative: $scope.serviceCenterRepresentative,
                    callReferenceNumber: $scope.callReferenceNumber
                });
            }

        }]);

    module.controller('userChooser', [
        '$scope', '$api', function ($scope, $api) {
            var users = [];
            $scope.users = function () { return users; };

            $api.users.getAll().success(function (data) {
                users = data;
            });
        }
    ]);

}(angular.module("verificationApp")));
