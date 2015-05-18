(function (module) {

    module.controller('loginController', [ '$scope', 'authCommands', function ($scope, commands) {

        $scope.login = function () {
            commands.login($scope.userName, $scope.password);
        }
    }]);

}(angular.module("loginApp")));