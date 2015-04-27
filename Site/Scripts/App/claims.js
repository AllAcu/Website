(function (module) {

    module.controller('claimsList', [
        "$scope", "claimsRepository", function ($scope, $claims) {
            $scope.drafts = [];

            $claims.findAll().success(function (data) {
                $scope.drafts = data.map(function (item) {
                    item.dateOfService = new Date(item.dateOfService);
                    return item;
                });
            });
        }
    ]);

    module.controller('claimEdit', [
        "$scope", "$routeParams", "$location", "claimsRepository", function ($scope, $routeParams, $location, $claims) {

            $scope.draft = {};
            $claims.find($routeParams["id"]).success(function (data) {
                data.dateOfService = new Date(data.dateOfService);
                $scope.draft = data;
            });

            $scope.save = function () {
                $claims.update($scope.draft).success(function () {
                    $location.path("/");
                });
            }

            $scope.submit = function () {
                $claims.submit($scope.draft).success(function () {
                    $location.path("/");
                });
            }
        }
    ]);

    module.controller('claimCreate', [
        "$scope", "$routeParams", "$location", "claimsRepository", function ($scope, $routeParams, $location, $claims) {

            $scope.draft = {};

            $scope.save = function () {
                $claims.create($scope.draft).success(function () {
                    $location.path("/");
                });
            }
        }
    ]);

}(angular.module("claimsApp", [])));
