(function (module) {

    module.controller('loginController', [
        '$scope', 'userCommands', 'authToken', '$location', function ($scope, commands, authToken, $location) {

            $scope.login = function () {
                commands.login($scope.userName, $scope.password)
                    .success(function (data) {
                        authToken.set(data.access_token);
                        $location.path("/");
                    });
            }
        }
    ]);

    module.controller('logoutController', [
        '$scope', 'authToken', '$location', function ($scope, authToken, $location) {
            authToken.clear();
            $location.path("/");
        }
    ]);

}(angular.module("loginApp")));