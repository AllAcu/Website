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
        "$scope", "$stateParams", "$location", "$api", "careProviderRepository", function ($scope, $stateParams, $location, $api, $providers) {
            $scope.provider = {};
            $scope.users = function () { return $scope.provider.users; };

            function refresh() {
                $providers.edit($stateParams["id"])
                    .success(function (data) {
                        $scope.provider = data;
                    });
            }

            refresh();

            $scope.save = function () {
                $api.providers.update($scope.provider).success(function () {
                    $providers.refresh();
                    $location.path("/providers");
                });
            }
            $scope.refresh = function () {
                $providers.refresh();
                refresh();
            }
        }]);

    module.controller('providerPermissions', [
        "$scope", "$api", function ($scope, $api) {
            var provider = function () { return $scope.$parent.provider; }
            $scope.users = $scope.$parent.users;
            var refresh = $scope.refresh = $scope.$parent.refresh;
            $scope.roles = [
            {
                label: "Owner",
                name: "owner"
            }, {
                label: "Practitioner",
                name: "practitioner"
            }];

            $scope.grant = function (user, role) {
                $api.providers.grantRole(user.user.userId, provider().id, role.name).success(function (data) {
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
}(angular.module("app")));
