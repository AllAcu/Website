(function (module) {

    module.controller('verificationRequestCreate', [
        "$scope", "$routeParams", "$location", "verificationCommands", function ($scope, $routeParams, $location, commands) {

            $scope.save = function () {
                commands.start($routeParams["id"], $scope.verification).success(function () {
                    $location.path("/patient/" + $routeParams["patientId"]);
                });
            }
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
            verifications.getRequest(verificationId)
                .success(function (data) {
                    $scope.verification = data.request;
                    patientId = data.patientId;
                });

            $scope.save = function () {
                commands.update(verificationId, $scope.verification)
                    .success(function () {
                        $location.path("/patient/" + patientId);
                    });
            };
            $scope.submit = function () {
                commands.submit(verificationId, $scope.verification)
                    .success(function () {
                        $location.path("/patient/" + patientId);
                    });
            };
        }
    ]);

    module.controller('verifyInsuranceList', [
        "$scope", "verificationRepository", function ($scope, verifications) {

            verifications.findAll()
                .success(function (data) {
                    $scope.verifications = data;
                });

        }
    ]);

    module.controller('verifyInsurance', [
        "$scope", "$routeParams", "$location", "verificationRepository", "patientRepository", "verificationCommands", function ($scope, $routeParams, $location, verifications, patients, commands) {

            var verificationId = $routeParams["verificationId"];
            var patientId;

            $scope.verification = {
                isCovered: "true"
            };

            verifications.getVerification(verificationId)
                .success(function (data) {
                    $scope.verification = data;
                    patientId = data.patientId;
                });

            $scope.save = function () {
                commands.update(verificationId, $scope.verification).success(function () {
                    console.log("saved " + verificationId);
                });
            }

            $scope.submit = function () {
                commands.approve(verificationId, $scope.verification).success(function () {
                    $location.path("/patient/" + patientId);
                });
            }
        }
    ]);

}(angular.module("insuranceApp")));
