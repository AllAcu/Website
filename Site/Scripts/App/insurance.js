(function (module) {

    module.controller('verificationRequestCreate', [
        "$scope", "$routeParams", "$location", "verificationCommands", function ($scope, $routeParams, $location, commands) {

            var patientId = $routeParams["patientId"];

            $scope.save = function () {
                commands.start(patientId, $scope.verification).success(function () {
                    $location.path("/patient/" + patientId);
                });
            }
            $scope.submit = function () {
                commands.submit(null, $scope.verification)
                    .success(function () {
                        $location.path("/patient/" + patientId);
                    });
            };
        }
    ]);

    module.controller('verificationDetails', [
        "$scope", "$routeParams", "verificationRepository", function ($scope, $routeParams, verifications) {
            var verificationId = $routeParams["verificationId"];
            verifications.getApprovedVerification(verificationId)
                .successs(function (data) {
                    $scope.benefits = data.benefits;
                });
        }
    ]);

    module.controller('verificationLetter', [
        "$scope", "$routeParams", "verificationRepository", function ($scope, $routeParams, verifications) {
            var verificationId = $routeParams["verificationId"];
            verifications.getApprovedVerification(verificationId)
                .success(function (data) {
                    $scope.benefits = data.benefits;
                });
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

            $scope.link = function(verification) {
                switch (verification.status) {
                case "Draft":
                    return "claims/#verification/" + verification.verificationId + "/edit";
                case "Submitted":
                    return "claims/#verification/" + verification.verificationId + "/verify";
                case "Approved":
                    return "claims/#verification/" + verification.verificationId + "/letter";
                }
            };
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

            $scope.approve = function () {
                commands.approve(verificationId, $scope.verification).success(function () {
                    $location.path("/patient/" + patientId);
                });
            }

            $scope.revise = function () {
                commands.revise(verificationId, $scope.verification).success(function () {
                    $location.path("/patient/" + patientId);
                });
            }
        }
    ]);

}(angular.module("insuranceApp")));
