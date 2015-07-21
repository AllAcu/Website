(function (module) {

    module.controller('providerCreate', [
        "$scope", "$location", "careProviderRepository", "careProviderCommands", function ($scope, $location, $providers, $commands) {
            $scope.provider = {};
            $scope.save = function () {
                $commands.create($scope.provider).success(function () {
                    $location.path("/patient");
                });
            }
        }]);

    module.controller('providerEdit', [
        "$scope", "$routeParams", "$location", "careProviderRepository", "careProviderCommands", function ($scope, $routeParams, $location, $providers, $commands) {
            $scope.provider = {};

            $providers.edit($routeParams["id"])
            .success(function (data) {
                    $scope.provider = data;
                });

            $scope.save = function () {
                $commands.update($scope.provider).success(function () {
                    $location.path("/patient");
                });
            }
        }]);



}(angular.module("providersApp")));
