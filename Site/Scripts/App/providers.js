(function(module) {

    module.controller('providerCreate', [
    "$scope", "$location", "careProviderRepository", function ($scope, $location, $providers) {
        $scope.provider = {};
        $scope.save = function () {
            $providers.create($scope.provider).success(function () {
                $location.path("/patient");
            });
        }
    }]);

}(angular.module("providersApp", [])));
