(function (module) {

    module.controller('verifyInsurance', [
        "$scope", function ($scope) {

        $scope.patient = {
            carrier: "Premera",
            phone: "206-555-3939"
        };

        $scope.verification = {
            isCovered: "true"
        };

        $scope.save = function () {
            console.log(JSON.stringify($scope.verification));
        }
    }
    ]);

    module.controller('verificationRequestCreate', [
        "$scope", "$routeParams", "verificationCommands", function ($scope, $routeParams, commands) {

            $scope.save = function () {
                commands.start($routeParams["id"], $scope.verification);
        }
    }
    ]);

    module.controller('verifyInsuranceList', [
        "$scope", "verificationRepository", function ($scope, verifications) {

            verifications.findAll()
                .success(function(data) {
                    $scope.verifications = data;
                });

        }
    ]);

    module.controller('verificationDetails', [
        "$scope", "verificationRepository", function ($scope, verifications) {

        }
    ]);

    module.controller('verificationRequestEdit', [
        "$scope", "verificationRepository", function ($scope, verifications) {

        }
    ]);




}(angular.module("insuranceApp")));
