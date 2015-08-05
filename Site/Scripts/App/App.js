(function (exports, angular) {
    var app = angular.module('allAcuApp', [
        'ngRoute',
        'authApp',
        'loginApp',
        'patientsApp',
        'providersApp',
        'billerApp',
        'claimsApp',
        'userApp',
        'verificationApp'
    ]);

    angular.module("authApp", []);
    angular.module("loginApp", []);
    angular.module("patientsApp", ["verificationApp"]);
    angular.module("providersApp", []);
    angular.module("billerApp", []);
    angular.module("userApp", []);
    angular.module("claimsApp", []);
    angular.module("verificationApp", []);

    app.config([
        '$routeProvider',
        function ($routeProvider) {
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
                    controller: 'patientList'
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
                .when('/patient/:patientId/insurance/verification/start', {
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
                    controller: 'verifyInsuranceList'
                })
                .when('/verification/:verificationId', {
                    templateUrl: '/Templates/Verification/insuranceVerificationDetails.html',
                    controller: 'verificationDetails'
                })
                .when('/verification/:verificationId/letter', {
                    templateUrl: '/Templates/Verification/insuranceVerificationLetter.html',
                    controller: 'verificationLetter'
                })
                .when('/verification/:verificationId/edit', {
                    templateUrl: '/Templates/Verification/verifyInsuranceRequest.html',
                    controller: 'verificationRequestEdit'
                })
                .when('/verification/:verificationId/verify', {
                    templateUrl: '/Templates/Verification/verifyInsurance.html',
                    controller: 'verifyInsurance'
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
                    controller: 'claimsList'
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
    ]).run(['$rootScope', '$location', '$http', function ($rootScope, $location, $http) {
        $rootScope.$on('$routeChangeStart', function (ev, next, curr) {
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
    }]);

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

    app.controller('nav', ['$scope', 'authToken', function ($scope, authToken) {
        var _loginNavItems = [
            { label: "Login", link: "/AllAcu/#/login" },
            { label: "Sign up", link: "/Allacu/#/signup" }
        ];
        var _navItems = [
                { label: "Verifications", link: "/AllAcu/#/verifications" },
                { label: "Patients", link: "/AllAcu/#/patients" },
                { label: "Users", link: "/AllAcu/#/users" },
                { label: "Claims", link: "/AllAcu/#/claims" },
                { label: "Providers", link: "/AllAcu/#/providers" },
                { label: "Logout", link: "/AllAcu/#/logout" }
        ];

        $scope.navItems = function () {
            if (!$scope.loggedIn()) {
                return _loginNavItems;
            }

            return _navItems;
        }

        $scope.loggedIn = function () {
            return authToken.loggedIn();
        }
    }]);

    app.controller('header', ['$scope', 'authToken', 'careProviderRepository', function ($scope, authToken, providers) {
        $scope.providers = function () { return providers.providers() || []; }
        $scope.currentProvider = function (provider) {
            return providers.current(provider);
        }

        $scope.setProvider = function () {
            providers.setCurrent($scope.currentProvider.id);
        }

        $scope.$watch(authToken.loggedIn, function () {
            providers.refresh();
        });

        $scope.available = function () {
            return authToken.loggedIn();
        }
    }]);

    exports.app = app;

})(window, angular)