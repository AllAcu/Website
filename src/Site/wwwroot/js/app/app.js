(function(exports, angular) {
    var app = angular.module('app', [
        'ngRoute',
        'timer',
        'ui.bootstrap'
    ]);

    app.config([
        '$routeProvider',
        function($routeProvider) {
            $routeProvider
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
        '$rootScope', '$window', '$http', function($rootScope, $window, $http) {
            $rootScope.$on('$routeChangeStart', function(ev, next, curr) {
                if (next.$$route) {
                    if (next.$$route.anonymous) {
                        return;
                    }

                    if (!userLoggedIn()) {
                        $window.location.href("/account/login");
                    }
                }
            });
            $rootScope.copyrightYear = new Date().getFullYear();
            $http.defaults.headers.common.Authorization = getAuthHeader;
        }
    ]);

    function userLoggedIn() {
        return true;
        //var authTokenService = angular.injector(['app']);
        //return authTokenService.get('authToken').loggedIn();
    }

    function getAuthHeader() {
        var authToken = angular.injector(['app']).get('authToken');
        if (authToken.loggedIn()) {
            return 'Bearer ' + authToken.get();
        }
    }

    app.controller('nav', [
        '$scope', 'authToken', function($scope, authToken) {
            var _loginNavItems = [
                { label: "Login", link: "/account/login" }
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