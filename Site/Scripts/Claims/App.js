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
                    $scope.drafts = data;
                });
            }
    ]);

    controllers.controller('claimEdit', [
            "$scope", "$routeParams", "claimsRepository", function ($scope, $routeParams, $claims) {

                $scope.draft = {};
                $claims.find($routeParams["id"]).success(function (data) {
                    $scope.draft = data;
                });
            }
    ]);

    exports.app = app;
    exports.controllers = controllers;
})(window)