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
        "$scope", "$location", "$api", function ($scope, $location, $api) {
            $scope.provider = {};
            $scope.save = function () {
                $api.providers.create($scope.provider).success(function () {
                    $location.path("/providers");
                });
            }
        }]);

    module.controller('providerDetails', [
        "$scope", "$routeParams", "$location", "$api", "careProviderRepository", function ($scope, $routeParams, $location, $api, $providers) {
            $scope.provider = {};
            $scope.users = function () { return $scope.provider.users; };

            $providers.edit($routeParams["id"])
                .success(function (data) {
                    $scope.provider = data;
                });

            $scope.save = function () {
                $api.providers.update($scope.provider).success(function () {
                    $providers.refresh();
                    $location.path("/providers");
                });
            }
            $scope.refresh = function() {
                $providers.refresh();
            }
        }]);

    module.controller('providerPermissions', [
        "$scope", "$api", function ($scope, $api) {
            var provider = function () { return $scope.$parent.provider; }
            $scope.users = $scope.$parent.users;
            var refresh = $scope.$parent.refresh;

            $scope.grant = function (user) {
                $api.providers.grantRole(user.user.userId, provider().id, "ui").success(function (data) {
                    refresh();
                });
            }
            $scope.revoke = function (user, role) {
                $api.providers.revokeRole(user.user.userId, provider().id, role).success(function (data) {
                    refresh();
                });
            }
        }
    ]);

}(angular.module("providersApp")));
