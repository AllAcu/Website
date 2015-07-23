(function (module) {

    module.controller('providerList', [
        '$scope', 'careProviderRepository', function ($scope, $providers) {
            $scope.providers = function () { return $providers.providers(); };

            $scope.link = function (provider) {
                return "#provider/" + provider.id;
            };
        }
    ]);

    module.controller('providerCreate', [
        "$scope", "$location", "careProviderRepository", "careProviderCommands", function ($scope, $location, $providers, $commands) {
            $scope.provider = {};
            $scope.save = function () {
                $commands.create($scope.provider).success(function () {
                    $location.path("/providers");
                });
            }
        }]);

    module.controller('providerDetails', [
        "$scope", "$routeParams", "$location", "careProviderRepository", "careProviderCommands", function ($scope, $routeParams, $location, $providers, $commands) {
            $scope.provider = {};

            $providers.edit($routeParams["id"])
                .success(function (data) {
                    $scope.provider = data;
                });

            $scope.save = function () {
                $commands.update($scope.provider).success(function () {
                    $providers.refresh();
                    $location.path("/providers");
                });
            }
        }]);

}(angular.module("providersApp")));
