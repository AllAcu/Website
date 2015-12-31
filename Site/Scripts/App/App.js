(function (exports, angular) {
    var app = angular.module('app', [
        'ui.router',
        'timer',
        'ui.bootstrap',
        'ui.bootstrap.tpls'
    ]);

    app.config([
        '$stateProvider', '$urlRouterProvider',
        function ($stateProvider, $urlRouterProvider) {
            $urlRouterProvider.otherwise("");
            $stateProvider
                .state({
                    name: 'login',
                    url: '/login',
                    templateUrl: '/Templates/Users/login.html',
                    controller: 'loginController',
                    anonymous: true
                })
                .state({
                    name: 'signup',
                    url: '/signup',
                    controller: 'signupController',
                    templateUrl: '/Templates/Users/signup.html',
                    anonymous: true
                })
                .state({
                    name: 'register',
                    url: '/register/:token',
                    controller: 'registrationController',
                    templateUrl: '/Templates/Users/register.html',
                    anonymous: true
                })
                .state({
                    name: 'logout',
                    url: '/logout',
                    controller: 'logoutController',
                    template: ''
                });

            $stateProvider
                .state({
                    name: 'patients',
                    url: '/patients',
                    templateUrl: '/Templates/Patients/patientList.html',
                    controller: 'patientList',
                    canChangeProviders: true
                })
                .state({
                    name: 'intake',
                    url: '/patient/intake',
                    templateUrl: '/Templates/Patients/intake.html',
                    controller: 'patientIntake'
                })
                .state({
                    name: 'patientDetails',
                    url: '/patient/:id',
                    templateUrl: '/Templates/Patients/details.html',
                    controller: 'patientDetails'
                })
                .state({
                    name: 'patientInfoEdit',
                    url: '/patient/:id/edit',
                    templateUrl: '/Templates/Patients/updateVitalInfo.html',
                    controller: 'patientEdit'
                })
                .state({
                    name: 'patientContactEdit',
                    url: '/patient/:id/contact/edit',
                    templateUrl: '/Templates/Patients/updateContactInfo.html',
                    controller: 'patientEditContactInfo'
                })
                .state({
                    name: 'patientInsuranceEdit',
                    url: '/patient/:id/insurance/edit',
                    templateUrl: '/Templates/Patients/recordInsurance.html',
                    controller: 'patientInsurance'
                })
                .state({
                    name: 'verificationRequest',
                    url: '/patient/:patientId/insurance/verification/request',
                    templateUrl: '/Templates/Verification/verifyInsuranceRequest.html',
                    controller: 'verificationRequestCreate'
                });

            $stateProvider
                .state({
                    name: 'providerList',
                    url: '/providers',
                    templateUrl: '/Templates/Providers/list.html',
                    controller: 'providerList'
                })
                .state({
                    name: 'providerCreate',
                    url: '/provider/create',
                    templateUrl: '/Templates/Providers/details.html',
                    controller: 'providerCreate'
                })
                .state({
                    name: 'providerDetails',
                    url: '/provider/:id',
                    templateUrl: '/Templates/Providers/details.html',
                    controller: 'providerDetails'
                });

            $stateProvider
                .state({
                    name: 'verificationList',
                    url: '/verifications',
                    templateUrl: '/Templates/Verification/list.html',
                    controller: 'insuranceVerificationList',
                    canChangeProviders: true
                })
                .state({
                    name: 'verificationEdit',
                    url: '/verification/:verificationId',
                    templateUrl: '/Templates/Verification/insuranceVerification.html',
                    controller: 'verification'
                });

            $stateProvider
                .state({
                    name: 'billerDetails',
                    url: '/biller',
                    templateUrl: '/Templates/Biller/details.html',
                    controller: 'billerDetails'
                });

            $stateProvider
                .state({
                    name: 'userList',
                    url: '/users',
                    templateUrl: '/Templates/Users/list.html',
                    controller: 'userListController'
                })
                .state({
                    name: 'userDetails',
                    url: '/user/:id',
                    templateUrl: '/Templates/Users/details.html',
                    controller: 'userDetailsController'
                });

            $stateProvider
                .state({
                    name: 'claimList',
                    url: '/claims',
                    templateUrl: '/Templates/Claims/claimsList.html',
                    controller: 'claimsList',
                    canChangeProviders: true
                })
                .state({
                    name: 'claimEdit',
                    url: '/claim/edit/:id',
                    templateUrl: '/Templates/Claims/claimEdit.html',
                    controller: 'claimEdit'
                })
                .state({
                    name: 'claimCreate',
                    url: '/claim/create',
                    templateUrl: '/Templates/Claims/claimEdit.html',
                    controller: 'claimCreate'
                });
        }
    ]).run(['$rootScope', '$state', '$http', 'authToken', function ($rootScope, $state, $http, authToken) {
        $rootScope.$on('$stateChangeStart', function (event, toState, toParams, fromState, fromParams) {
            if (!toState.anonymous && !authToken.loggedIn()) {
                event.preventDefault();
                return $state.go('login');
            }
        });
        $rootScope.copyrightYear = new Date().getFullYear();
        $http.defaults.headers.common.Authorization = function () {
            if (authToken.loggedIn()) {
                return 'Bearer ' + authToken.get();
            }
        };
    }]);

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
                { label: "Biller", link: "/AllAcu/#/biller" }
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

    app.controller('providerChooser', ['$scope', '$state', 'authToken', 'careProviderRepository', function ($scope, $state, authToken, providers) {
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

        $scope.canChoose = function () {
            return $state.current && $state.current.$$route && $state.current.$$route.canChangeProviders && authToken.loggedIn() && $scope.providers().length > 1;
        }

        $scope.shouldDisplay = function () {
            return authToken.loggedIn() && !!$scope.providers().length;
        }
    }]);

    exports.app = app;

})(window, angular)