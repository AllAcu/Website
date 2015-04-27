(function (exports, angular) {
    var app = angular.module('claimsApp', [
        'ngRoute',
        'claimsAppControllers',
        'patientsApp',
        'providersApp'
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

    var controllers = angular.module("claimsAppControllers", []);

    controllers.controller('claimsList', [
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

    controllers.controller('claimEdit', [
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

    controllers.controller('claimCreate', [
            "$scope", "$routeParams", "$location", "claimsRepository", function ($scope, $routeParams, $location, $claims) {

                $scope.draft = {};

                $scope.save = function () {
                    $claims.create($scope.draft).success(function () {
                        $location.path("/");
                    });
                }
            }
    ]);

    controllers.controller('header', ['$scope', 'careProviderRepository', function ($scope, $providers) {
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