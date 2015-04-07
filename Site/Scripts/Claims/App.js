(function (exports) {
    var app = angular.module('claimsApp', [
        'ngRoute',
        'claimsAppControllers'
    ]);

    app.config([
        '$routeProvider',
        function ($routeProvider) {
            $routeProvider.
                when('/', {
                    templateUrl: '/Templates/Claims/claimsList.html',
                    controller: 'claimsList'
                }).
                when('/edit/:id', {
                    templateUrl: '/Templates/Claims/claimEdit.html',
                    controller: 'claimEdit'
                }).
                when('/create', {
                    templateUrl: '/Templates/Claims/claimEdit.html',
                    controller: 'claimCreate'
                }).
                otherwise({
                    redirectTo: '/'
                });
        }
    ]);

    var controllers = angular.module("claimsAppControllers", []);

    controllers.controller('claimsList', [
            "$scope", "claimsRepository", function ($scope, $claims) {
                $scope.drafts = [];

                $claims.findAll().success(function (data) {
                    $scope.drafts = data.map(function(item) {
                        item.dateOfService = new Date(item.dateOfService);
                        return item;
                    });
                });
            }
    ]);

    controllers.controller('claimEdit', [
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
            }
    ]);

    controllers.controller('claimCreate', [
            "$scope", "$routeParams", "$location", "claimsRepository", function ($scope, $routeParams, $location, $claims) {

                $scope.draft = {};

                $scope.save = function () {
                    $claims.create($scope.draft).success(function() {
                        $location.path("/");
                    });
                }
            }
    ]);

    exports.app = app;
    exports.controllers = controllers;
})(window)