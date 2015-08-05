(function (module) {

    module.controller('billerDetails', [
        "$scope", "$routeParams", "$location", "$api", function ($scope, $routeParams, $location, $api) {
            $scope.biller = {};
            $scope.users = function () { return $scope.biller.users; };

            $api.biller.get()
                .success(function (data) {
                    $scope.biller = data;
                });
        }]);

}(angular.module("billerApp")));