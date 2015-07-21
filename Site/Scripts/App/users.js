(function (module) {

    module.controller('userRegistrationController', [
        '$scope', 'userCommands', function ($scope, commands) {

            $scope.registration = {};

            $scope.save = function () {
                commands.register($scope.registration)
                    .success(function (data) {
                        console.log(data);
                    });
            }
        }
    ]);

    module.controller('userListController', [
        '$scope', '$api', function ($scope, $api) {
            var users = [];
            $scope.users = function () { return users; };

            $scope.link = function (user) {
                return "#user/" + user.userId;
            };

            $api.users.getAll().success(function (data) {
                users = data;
            });
        }
    ]);

    module.controller('userDetailsController', [
        '$scope', '$routeParams', '$api', "careProviderRepository", function ($scope, $routeParams, $api, providerRepo) {
            var userId = $routeParams["id"];
            var providers = [];
            $scope.user = null;
            $scope.providers = function() { return providers; };

            $scope.hasProvider = function(provider) {
                return $scope.user && $scope.user.providers.some(function(p) { return p === provider.id; });
            }

            $api.providers.getAll()
                .success(function (data) {
                    providers = data;
                });

            function refreshUser() {
                $api.users.get(userId)
                    .success(function(data) {
                        $scope.user = data;
                    });
                providerRepo.refresh();
            }

            refreshUser();

            $scope.join = function (provider) {
                $api.providers.join(userId, provider.id)
                    .success(function() {
                        refreshUser();
                    });
            }

            $scope.leave = function(provider) {
                $api.providers.leave(userId, provider.id)
                    .success(function () {
                        refreshUser();
                    });
            }
        }
    ]);

}(angular.module("userApp")));