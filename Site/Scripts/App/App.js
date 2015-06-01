(function (exports, angular) {
    var app = angular.module('allAcuApp', [
        'ngRoute',
        'loginApp',
        'patientsApp',
        'providersApp',
        'claimsApp'
    ]);

    angular.module("loginApp", []);
    angular.module("patientsApp", ["insuranceApp"]);
    angular.module("providersApp", []);
    angular.module("claimsApp", []);
    angular.module("insuranceApp", []);

    app.config([
        '$routeProvider',
        function ($routeProvider) {
            $routeProvider
                .when('/', {
                    templateUrl: '/Templates/Claims/claimsList.html',
                    controller: 'claimsList'
                })
                .when('/login', {
                    templateUrl: '/Templates/login.html',
                    controller: 'loginController'
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
                    templateUrl: '/Templates/Verification/verificationList.html',
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