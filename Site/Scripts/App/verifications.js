(function (module) {

    module.controller('verificationRequestCreate', [
        "$scope", "$routeParams", "$location", "$api", function ($scope, $routeParams, $location, $api) {

            var patientId = $routeParams["patientId"];

            $scope.save = function () {
                $api.verificationRequests.start(patientId, $scope.request).success(function () {
                    $location.path("/patient/" + patientId);
                });
            }
            $scope.submit = function () {
                $api.verificationRequests.submitNew(patientId, $scope.request)
                    .success(function () {
                        $location.path("/patient/" + patientId);
                    });
            };
        }
    ]);

    module.controller('verificationRequestEdit', [
        "$scope", "$routeParams", "$location", "verificationRepository", "$api", function ($scope, $routeParams, $location, verifications, $api) {
            var verificationId = $routeParams["verificationId"];
            var patientId;
            verifications.getVerification(verificationId)
                .success(function (data) {
                    $scope.request = data.request;
                    patientId = data.patientId;
                });

            $scope.save = function () {
                $api.verificationRequests.update(verificationId, $scope.request)
                    .success(function () {
                        $location.path("/patient/" + patientId);
                    });
            };
            $scope.submit = function () {
                $api.verificationRequests.submit(verificationId, $scope.request)
                    .success(function () {
                        $location.path("/patient/" + patientId);
                    });
            };
        }
    ]);

    module.controller('verifyInsuranceList', [
        "$scope", "verificationRepository", function ($scope, repo) {

            $scope.verifications = repo.verifications;

            $scope.link = function (verification) {
                switch (verification.status) {
                    case "Draft":
                        return "#verification/" + verification.verificationId + "/edit";
                    case "Submitted":
                        return "#verification/" + verification.verificationId + "/verify";
                    case "Approved":
                        return "#verification/" + verification.verificationId + "/letter";
                }
            };

            $scope.refresh = function () {
                repo.refresh();
            }
        }
    ]);

    module.controller('verifyInsurance', [
        "$scope", "$routeParams", "$location", "$modal", "verificationRepository", "patientRepository", "$api", function ($scope, $routeParams, $location, $modal, verifications, patients, $api) {

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

            $scope.timeOnCall = function() {
                if ($scope.verification && $scope.verification.startTime) {
                    return Date() - startTime;
                }
                return "1:10";
            }

            $scope.save = function () {
                $api.verifications.update(verificationId, $scope.verification).success(function () {
                    console.log("saved " + verificationId);
                });
            }

            $scope.startCall = function() {
                console.log("Start Call");
            }

            $scope.actions = function () {
                $scope.items = ["a", "b"];

                var modalInstance = $modal.open({
                    animation: true,
                    templateUrl: '/Templates/Verification/actions.html',
                    controller: 'verificationActions',
                    size: 'lg',
                    resolve: {
                        items: function () {
                            return $scope.items;
                        }
                    }
                });

                modalInstance.result.then(function (selectedItem) {
                    $scope.selected = selectedItem;
                }, function () {
                    console.info('Modal dismissed at: ' + new Date());
                });
            }
        }
    ]);

    module.controller('verificationActions', [
        "$scope", "$routeParams", "$api", "verificationRepository", function ($scope, $routeParams, $api, verifications) {
            var verificationId = $routeParams["verificationId"];

            var actions = 
                 [
                    { label: "assign", template: '/Templates/Verification/assign.html' },
                    //{ label: "approve", template: '/Templates/Verification/approve.html' },
                    //{ label: "reject", template: '/Templates/Verification/reject.html' },
                    { label: "complete", template: '/Templates/Verification/complete.html' },
                    { label: "startCall", template: '/Templates/Verification/startCall.html' }
                ];

            $scope.actions = function() { return actions; };
            $scope.currentAction = $scope.actions()[0];

            verifications.getVerification(verificationId)
                .success(function (data) {
                    $scope.verification = data;
                });

            $scope.actionTemplate = function () {
                return $scope.currentAction.template;
            }

            $scope.complete = function () {
                $api.verifications.approve(verificationId, $scope.verification).success(function () {
                    $location.path("/verifications");
                });
            }

            $scope.revise = function () {
                $api.verifications.revise(verificationId, $scope.verification).success(function () {
                    $location.path("/patient/" + patientId);
                });
            }

            $scope.select = function (user) {
                $scope.selectedUser = user;
            };

            $scope.save = function () {
                $api.verifications.assign(verificationId, $scope.selectedUser);
            }
        }
    ]);

    module.controller('userChooser', [
        '$scope', '$api', function ($scope, $api) {
            var users = [];
            $scope.users = function () { return users; };

            $api.users.getAll().success(function (data) {
                users = data;
            });
        }
    ]);

    module.controller('verificationDetails', [
        "$scope", "$routeParams", "verificationRepository", function ($scope, $routeParams, verifications) {
            var verificationId = $routeParams["verificationId"];
            verifications.getVerification(verificationId)
                .success(function (data) {
                    $scope.benefits = data.benefits;
                });
        }
    ]);

    module.controller('verificationLetter', [
        "$scope", "$routeParams", "verificationRepository", function ($scope, $routeParams, verifications) {
            var verificationId = $routeParams["verificationId"];
            verifications.getVerification(verificationId)
                .success(function (data) {
                    $scope.benefits = data.benefits;
                });
        }
    ]);

}(angular.module("verificationApp")));
