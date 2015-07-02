(function (module) {

    module.controller('loginController', [
        '$scope', 'userCommands', 'authToken', function($scope, commands, authToken) {

            $scope.login = function() {
                commands.login($scope.userName, $scope.password)
                    .success(function (data) {
                        authToken.set(data.access_token);
                    });
            }
        }
    ]);

    module.controller('userRegistrationController', [
        '$scope', 'userCommands', function($scope, commands) {

            $scope.registration = {};

            $scope.save = function() {
                commands.register($scope.registration)
                    .success(function(data) {
                        console.log(data);
                    });
            }
        }
    ]);

    module.controller('userList', [
        '$scope', '$api', function ($scope, $api) {
            var users = [];
            $scope.users = function() { return users; };

            $api.users.getAll().success(function(data) {
                users = data;
            });

            $scope.create = function() {
                console.log("create a user");
            }
        }
    ]);

}(angular.module("loginApp")));