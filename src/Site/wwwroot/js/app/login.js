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
        '$scope', 'authToken', 'userSession', '$location', function ($scope, authToken, session, $location) {
            authToken.clear();
            session().logout();
            $location.path("/");
        }
    ]);

}(angular.module("loginApp")));