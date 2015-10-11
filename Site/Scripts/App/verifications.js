(function (module) {

    module.controller('verificationRequestCreate', [
        "$scope", "$routeParams", "$location", "$api", function ($scope, $routeParams, $location, $api) {
            var patientId = $routeParams["patientId"];

            $scope.save = function () {
                $api.verifications.start(patientId, $scope.request).success(function (response) {
                    $location.path("/verification/" + response);
                });
            }
            $scope.submit = function () {
                $api.verification.submitNewRequest(patientId, $scope.request)
                    .success(function () {
                        $location.path("/patient/" + patientId);
                    });
            };
        }
    ]);

    module.controller('verification', [
        "$scope", "$routeParams", "$location", "$modal", "verificationRepository", "$api", function ($scope, $routeParams, $location, $modal, verifications, $api) {
            var verificationId = $routeParams["verificationId"];
            var patientId;

            verifications.getVerification(verificationId)
                .success(function (data) {
                    $scope.verification = data;
                    patientId = data.patientId;
                });

            $scope.verificationTemplate = function () {

                switch ($scope.verification && $scope.verification.status) {
                    case "Draft":
                    case "Rejected":
                        return "/Templates/Verification/verifyInsuranceRequest.html";
                    case "Submitted":
                        return "/Templates/Verification/verifyInsurance.html";
                    case "Assigned":
                        return "/Templates/Verification/displayRequest.html";
                    case "In Progress":
                        return "/Templates/Verification/verifyInsurance.html";
                    case "PendingApproval":
                        return "/Templates/Verification/verifyInsurance.html";
                    case "Verified":
                        return "/Templates/Verification/insuranceVerificationLetter.html";
                }

                return "";
            }

            var actions = {
                assign: { name: "Assign", handler: "startAssignment" },
                startCall: { name: "Start Call", handler: "startCall" },
                endCall: { name: "End Call", handler: "dummy" },
                save: { name: "Save", handler: "save" },
                saveDraft: { name: "Save", handler: "saveDraft" },
                approve: { name: "Approve", handler: "dummy" },
                reject: { name: "Reject", handler: "dummy" }
            }

            $scope.availableActions = function () {
                switch ($scope.verification && $scope.verification.status) {
                    case "Draft":
                    case "Rejected":
                        return [actions.saveDraft];
                    case "Submitted":
                        return [actions.assign];
                    case "Assigned":
                        return [actions.startCall, actions.assign];
                    case "In Progress":
                        return [actions.save, actions.endCall];
                    case "PendingApproval":
                        return [actions.approve, actions.reject];
                    case "Verified":
                        return [];
                }
                return [];
            }

            $scope.dummy = function () {
                console.log("Dummy callback");
            }

            $scope.handleClick = function (method) {
                $scope[method]();
            }

            $scope.saveDraft = function () {
                $api.verifications.updateRequest(verificationId, $scope.request);
            };

            $scope.submit = function () {
                $api.verifications.submitRequest(verificationId, $scope.request)
                    .success(function () {
                        $location.path("/patient/" + patientId);
                    });
            };

            $scope.startAssignment = function () {
                popup("/Templates/Verification/assign.html");
            }

            $scope.startCall = function () {
                popup("/Templates/Verification/startCall.html");
            }

            $scope.save = function () {
                $api.verifications.update(verificationId, $scope.verification).success(function () {
                    console.log("saved " + verificationId);
                });
            }

            function popup(templateUrl) {
                var modalInstance = $modal.open({
                    animation: true,
                    templateUrl: '/Templates/Verification/actions.html',
                    controller: 'verificationActions',
                    size: 'lg',
                    resolve: {
                        verification: function() {
                            return $scope.verification;
                        },
                        template: function() {
                            return templateUrl;
                        },
                        patientName: function () {
                            return $scope.verification.patient.patientName;
                        }
                    }
                });

                modalInstance.result.then(function (result) {
                    console.dir(result);
                }, function () {
                    console.info('Modal dismissed at: ' + new Date());
                });
            }
        }
    ]);

    module.controller('insuranceVerificationList', [
        "$scope", "verificationRepository", function ($scope, repo) {

            $scope.verifications = repo.verifications;

            $scope.link = function (verification) {
                return "#verification/" + verification.verificationId;
            };

            $scope.refresh = function () {
                repo.refresh();
            }
        }
    ]);

    module.controller('verificationActions', [
        "$scope", "$api", "verification", "patientName", "template", function ($scope, $api, verification, patientName, template) {
            var verificationId = verification.id;
            $scope.verification = verification;
            $scope.actionTemplate = template;
            $scope.patientName = patientName;

            $scope.complete = function () {
                $api.verifications.complete(verificationId, $scope.verification).success(function () {
                    $location.path("/verifications");
                });
            }

            $scope.select = function (user) {
                $scope.selectedUser = user;
            };

            $scope.reject = function () {
                $api.verifications.reject(verificationId, $scope.verification).success(function () {
                    $location.path("/patient/" + patientId);
                });
            }

            $scope.assign = function () {
                $api.verifications.delegate(verificationId, $scope.selectedUser.userId);
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

}(angular.module("verificationApp")));
