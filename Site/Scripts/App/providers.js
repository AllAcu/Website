(function (module) {

    module.controller('providerCreate', [
        "$scope", "$location", "careProviderRepository", function ($scope, $location, $providers) {
            $scope.provider = {};
            $scope.save = function () {
                $providers.create($scope.provider).success(function () {
                    $location.path("/patient");
                });
            }
        }]);

    module.controller('providerEdit', [
        "$scope", "$routeParams", "$location", "careProviderRepository", function ($scope, $routeParams, $location, $providers) {
            $scope.provider = {};

            $providers.edit($routeParams["id"])
            .success(function (data) {
                    $scope.provider = data;
                });

            $scope.save = function () {
                $providers.create($scope.provider).success(function () {
                    $location.path("/patient");
                });
            }
        }]);



}(angular.module("providersApp")));
