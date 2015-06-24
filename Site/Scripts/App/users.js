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

    module.controller('userCreateController', [
        '$scope', 'userCommands', function($scope, commands) {

            $scope.save = function() {
                commands.createUser($scope.email, $scope.password, $scope.confirmPassword)
                    .success(function(data) {
                        console.log(data);
                    });
            }
        }
    ]);

}(angular.module("loginApp")));