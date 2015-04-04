(function(exports) {
    var app = window.app = angular.module('claimsApp', []);

    app.controller('claimsList', [ "$scope", "claimsRepository", function($scope, $claims) {
            $scope.drafts = [
                {
                    'patient': 'John Q. Public',
                    'diagnosis': 'Sad'
                }
            ];

            $scope.grab = function() {
                $claims.findAll().success(function(data) {
                    $scope.drafts = data;
                });
            }
        }
    ]);

    exports.app = app;
})(window)