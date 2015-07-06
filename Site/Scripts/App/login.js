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

}(angular.module("loginApp")));