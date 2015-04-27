(function (exports, angular) {
    var app = angular.module('allAcuApp', [
        'ngRoute',
        'patientsApp',
        'providersApp',
        'claimsApp'
    ]);

    app.config([
        '$routeProvider',
        function ($routeProvider) {
            $routeProvider.
                when('/', {
                    templateUrl: '/Templates/Claims/claimsList.html',
                    controller: 'claimsList'
                }).
                when('/edit/:id', {
                    templateUrl: '/Templates/Claims/claimEdit.html',
                    controller: 'claimEdit'
                }).
                when('/create', {
                    templateUrl: '/Templates/Claims/claimEdit.html',
                    controller: 'claimCreate'
                }).
                when('/patient', {
                    templateUrl: '/Templates/Patients/patientList.html',
                    controller: 'patientList'
                }).
                when('/patient/edit/:id', {
                    templateUrl: '/Templates/Patients/patientEdit.html',
                    controller: 'patientEdit'
                }).
                when('/provider/create', {
                    templateUrl: '/Templates/Providers/providerEdit.html',
                    controller: 'providerCreate'
                }).
                otherwise({
                    redirectTo: '/'
                });
        }
    ]);

    app.controller('header', ['$scope', 'careProviderRepository', function ($scope, $providers) {
        $scope.providers = [];

        $providers.findAll().success(function (data) {
            $scope.providers = data;
            $providers.getCurrent().success(function (current) {
                $scope.currentProvider = $scope.providers.filter(function (p) { return p.id == current })[0];
            });
        });

        $scope.setProvider = function () {
            $providers.setCurrent($scope.currentProvider.id);
        }
    }]);

    exports.app = app;

})(window, angular)