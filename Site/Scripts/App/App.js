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
                .when('/patient/:id/insurance/verification', {
                    templateUrl: '/Templates/Verification/verificationList.html',
                    controller: 'patientVerificationList'
                })
                .when('/patient/:id/insurance/verification/start', {
                    templateUrl: '/Templates/Patients/requestInsuranceVerification.html',
                    controller: 'verificationRequestCreate'
                })
                .when('/patient/:id/insurance/verification/:verificationId', {
                    templateUrl: '/Templates/Patients/requestInsuranceVerification.html',
                    controller: 'verificationRequestEdit'
                })
                .when('/verification', {
                    templateUrl: '/Templates/Verification/verificationList.html',
                    controller: 'verifyInsuranceList'
                })
                .when('/verification/:verificationId/verify', {
                    templateUrl: '/Templates/Patients/verifyInsurance.html',
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