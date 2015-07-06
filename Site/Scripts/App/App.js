﻿(function (exports, angular) {
    var app = angular.module('allAcuApp', [
        'ngRoute',
        'loginApp',
        'patientsApp',
        'providersApp',
        'claimsApp',
        'userApp',
        'verificationApp'
    ]);

    angular.module("loginApp", []);
    angular.module("patientsApp", ["verificationApp"]);
    angular.module("providersApp", []);
    angular.module("userApp", []);
    angular.module("claimsApp", []);
    angular.module("verificationApp", []);

    app.config([
        '$routeProvider',
        function ($routeProvider) {
            $routeProvider
                .when('/', {
                    templateUrl: '/Templates/Claims/claimsList.html',
                    controller: 'claimsList'
                })
                .when('/login', {
                    templateUrl: '/Templates/Users/login.html',
                    controller: 'loginController'
                })
                .when('/users/register', {
                    templateUrl: '/Templates/Users/register.html',
                    controller: 'userRegistrationController'
                })
                .when('/patient', {
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
                .when('/provider/create', {
                    templateUrl: '/Templates/Providers/providerEdit.html',
                    controller: 'providerCreate'
                })
                .when('/patient/:id/insurance/edit', {
                    templateUrl: '/Templates/Patients/recordInsurance.html',
                    controller: 'patientInsurance'
                })
                .when('/patient/:patientId/insurance/verification/start', {
                    templateUrl: '/Templates/Verification/verifyInsuranceRequest.html',
                    controller: 'verificationRequestCreate'
                })
                .when('/verification', {
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
                })
                .when('/users', {
                    templateUrl: '/Templates/Users/list.html',
                    controller: 'userListController'
                })
                .when('/user/:id', {
                    templateUrl: '/Templates/Users/details.html',
                    controller: 'userDetailsController'
                })
                .when('/edit/:id', {
                    templateUrl: '/Templates/Claims/claimEdit.html',
                    controller: 'claimEdit'
                })
                .when('/create', {
                    templateUrl: '/Templates/Claims/claimEdit.html',
                    controller: 'claimCreate'
                })
                .otherwise({
                    redirectTo: '/'
                });
        }
    ]);

    app.controller('header', ['$scope', 'careProviderRepository', function ($scope, $providers) {
        $scope.providers = function () { return $providers.providers(); }
        $scope.currentProvider = function (provider) {
            return $providers.current(provider);
        }

        $scope.setProvider = function () {
            $providers.setCurrent($scope.currentProvider.id);
        }
    }]);

    exports.app = app;

})(window, angular)