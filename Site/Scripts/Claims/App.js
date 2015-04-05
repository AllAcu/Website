(function (exports) {
    var app = angular.module('claimsApp', [
        'ngRoute',
        'claimsAppControllers'
    ]);

    app.config([
        '$routeProvider',
        function ($routeProvider) {
            $routeProvider.
                when('/claims', {
                    templateUrl: 'Templates/claimsList.html',
                    controller: 'claimsList'
                }).
                when('/claims/edit', {
                    templateUrl: 'Templates/claimEdit.html',
                    controller: 'claimEdit'
                }).
                otherwise({
                    redirectTo: '/claims'
                });
        }
    ]);

    var controllers = angular.module("claimsAppControllers", []);

    controllers.controller('claimsList', [
            "$scope", "claimsRepository", function ($scope, $claims) {
                $scope.drafts = [
                    {
                        'patient': 'John Q. Public',
                        'diagnosis': 'Sad'
                    }
                ];

                $scope.grab = function () {
                    $claims.findAll().success(function (data) {
                        $scope.drafts = data;
                    });
                }

                $scope.edit = function (item) {
                    console.log("editing: " + JSON.stringify(item.draft));
                }
            }
    ]);

    controllers.controller('claimEdit', function ($scope) {
            $scope.testValue = "value number 1";
        }
    );

    exports.app = app;
    exports.controllers = controllers;
})(window)