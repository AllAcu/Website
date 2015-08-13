(function (module) {

    module.controller('billerDetails', [
        "$scope", "$routeParams", "$location", "$api", function ($scope, $routeParams, $location, $api) {
            $scope.biller = {};
            $scope.users = function () { return $scope.biller.users; };

            $scope.refresh = function () {
                $api.biller.get()
                    .success(function (data) {
                        $scope.biller = data;
                    });
            }

            $scope.refresh();
        }]);

    module.controller('billerInvite', [
        '$scope', '$api', function ($scope, $api) {

            $scope.invite = function () {
                $api.biller.invite($scope.email, "employee").success(function () {
                    console.log("invited " + $scope.email);
                });
            }
        }
    ]);

    module.controller('billerPermissions', [
        "$scope", "$api", function ($scope, $api) {
            $scope.users = $scope.$parent.users;
            var refresh = $scope.$parent.refresh;

            $scope.grant = function (user) {
                $api.biller.grantRole(user.user.userId, "approver").success(function (data) {
                    refresh();
                });
            }
            $scope.revoke = function (user, role) {
                $api.biller.revokeRole(user.user.userId, role).success(function (data) {
                    refresh();
                });
            }
        }
    ]);
}(angular.module("billerApp")));