(function (module) {

    module.controller('loginController', [
        '$scope', 'userCommands', function($scope, commands) {

            $scope.login = function() {
                commands.login($scope.userName, $scope.password)
                    .success(function (data) {
                        window.sessionStorage.setItem("accessToken", data.access_token);
                        console.log(window.sessionStorage.getItem("accessToken"));
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