(function (module) {

    module.controller('verificationRequestCreate', [
        "$scope", "$routeParams", "$location", "verificationCommands", function ($scope, $routeParams, $location, commands) {

            var patientId = $routeParams["patientId"];

            $scope.save = function () {
                commands.request.start(patientId, $scope.request).success(function () {
                    $location.path("/patient/" + patientId);
                });
            }
            $scope.submit = function () {
                commands.request.submitNew(patientId, $scope.request)
                    .success(function () {
                        $location.path("/patient/" + patientId);
                    });
            };
        }
    ]);

    module.controller('verificationRequestEdit', [
        "$scope", "$routeParams", "$location", "verificationRepository", "verificationCommands", function ($scope, $routeParams, $location, verifications, commands) {
            var verificationId = $routeParams["verificationId"];
            var patientId;
            verifications.getRequest(verificationId)
                .success(function (data) {
                    $scope.request = data.request;
                    patientId = data.patientId;
                });

            $scope.save = function () {
                commands.request.update(verificationId, $scope.request)
                    .success(function () {
                        $location.path("/patient/" + patientId);
                    });
            };
            $scope.submit = function () {
                commands.request.submit(verificationId, $scope.request)
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

            $scope.link = function (verification) {
                switch (verification.status) {
                    case "Draft":
                        return "claims/#verification/" + verification.verificationId + "/edit";
                    case "Submitted":
                        return "claims/#verification/" + verification.verificationId + "/verify";
                    case "Approved":
                        return "claims/#verification/" + verification.verificationId + "/letter";
                }
            };

            $scope.refresh = function() {
                verifications.findAll()
                    .success(function (data) {
                        $scope.verifications = data;
                    });
            }
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
                commands.verification.update(verificationId, $scope.verification).success(function () {
                    console.log("saved " + verificationId);
                });
            }

            $scope.approve = function () {
                commands.verification.approve(verificationId, $scope.verification).success(function () {
                    $location.path("/patient/" + patientId);
                });
            }

            $scope.revise = function () {
                commands.verification.revise(verificationId, $scope.verification).success(function () {
                    $location.path("/patient/" + patientId);
                });
            }
        }
    ]);

    module.controller('verificationDetails', [
    "$scope", "$routeParams", "verificationRepository", function ($scope, $routeParams, verifications) {
        var verificationId = $routeParams["verificationId"];
        verifications.getApprovedVerification(verificationId)
            .success(function (data) {
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


}(angular.module("insuranceApp")));
