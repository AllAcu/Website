﻿(function (module) {

    module.controller('registrationController', [
        '$scope', '$routeParams', '$location', 'userCommands', function ($scope, $routeParams, $location, commands) {

            var token = $routeParams["token"];
            $scope.registration = {};

            $scope.save = function () {
                if ($scope.password !== $scope.confirmPassword) {
                    return;
                }

                commands.register(token, $scope.registration.name, $scope.registration.password)
                    .success(function () {
                        console.log("registered " + $scope.name);
                        $location.path("/");
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
            $scope.providers = function () { return providers; };

            $scope.hasProvider = function (provider) {
                return $scope.user && $scope.user.providers.some(function (p) { return p.id === provider.id; });
            }

            $api.providers.getAll()
                .success(function (data) {
                    providers = data;
                });

            function refreshUser() {
                $api.users.get(userId)
                    .success(function (data) {
                        $scope.user = data;
                    });
                providerRepo.refresh();
            }

            refreshUser();

            $scope.join = function (provider) {
                $api.providers.join(userId, provider.id)
                    .success(function () {
                        refreshUser();
                    });
            }

            $scope.leave = function (provider) {
                $api.providers.leave(userId, provider.id)
                    .success(function () {
                        refreshUser();
                    });
            }
        }
    ]);

    module.controller('inviteController', [
        '$scope', '$routeParams', function ($scope, $routeParams) {
            $scope.invite = function () {
                console.log($scope.email);
            }
        }
    ]);

    module.controller('signupController', [
        '$scope', '$routeParams', 'userCommands', function ($scope, $routeParams, userCommands) {
            $scope.signup = function () {
                userCommands.signup($scope.email).success(function () {
                    console.log("signed up!");
                });
            }
        }
    ]);

}(angular.module("userApp")));