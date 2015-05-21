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
        "$scope", "$routeParams", "$location", "verificationCommands", function ($scope, $routeParams, location, commands) {

            $scope.save = function () {
                commands.start($routeParams["id"], $scope.verification).success(function() {
                    $location.path("/patient/" + $routeParams["patientId"]);
                });
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
        "$scope", "$routeParams", "$location", "verificationRepository", "verificationCommands", function ($scope, $routeParams, $location, verifications, commands) {
            var verificationId = $routeParams["verificationId"];
            var patientId;
            verifications.get(verificationId)
                .success(function(data) {
                    $scope.verification = data.request;
                    patientId = data.patientId;
                });

            $scope.save = function() {
                commands.update(verificationId, $scope.verification)
                    .success(function () {
                        $location.path("/patient/" + patientId);
                    });
            };
            $scope.submit = function() {
                commands.submit(verificationId, $scope.verification)
                    .success(function () {
                        $location.path("/patient/" + patientId);
                    });
            };
        }
    ]);




}(angular.module("insuranceApp")));
