(function (module) {

    module.controller('verificationRequestCreate', [
        "$scope", "$stateParams", "$state", "$api", function ($scope, $stateParams, $state, $api) {
            var patientId = $stateParams["patientId"];

            $scope.save = function () {
                $api.verifications.start(patientId, $scope.request).success(function (response) {
                    $state.go("verificationList", { verificationId: response });
                });
            }
            $scope.submit = function () {
                $api.verifications.submitNewRequest(patientId, $scope.request)
                    .success(function () {
                        $state.go("patientDetails", { patientId: patientId });
                    });
            };
        }
    ]);

    module.controller('verification', [
        "$scope", "$stateParams", "$state", "$uibModal", "verificationRepository", "$api", function ($scope, $stateParams, $state, $uibModal, verifications, $api) {
            var verificationId = $stateParams["verificationId"];
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
                        return "/Templates/Verification/displayRequest.html";
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
                endCall: { name: "End Call", handler: "endCall" },
                save: { name: "Save", handler: "save" },
                saveDraft: { name: "Save", handler: "saveDraft" },
                approve: { name: "Approve", handler: "dummy" },
                reject: { name: "Reject", handler: "dummy" }
            }

            var availableActions = {
                "Draft": [actions.saveDraft],
                "Rejected": [actions.saveDraft],
                "Submitted": [actions.assign],
                "Assigned": [actions.startCall, actions.assign],
                "In Progress": [actions.save, actions.endCall],
                "PendingApproval": [actions.approve, actions.reject],
                "Verified": []
            }

            $scope.availableActions = function () {
                return $scope.verification ?
                    availableActions[$scope.verification.status] : [];
            }

            $scope.dummy = function () {
                console.log("Dummy callback");
            }

            $scope.handleClick = function (method) {
                $scope[method]();
            }

            $scope.save = function () {
                $api.verifications.update(verificationId, $scope.verification);
            }

            $scope.saveDraft = function () {
                $api.verifications.updateRequest(verificationId, $scope.request);
            };

            $scope.submit = function () {
                $api.verifications.submitRequest(verificationId, $scope.request)
                    .success(function () {
                        $state.go("patientDetails", { patientId: patientId });
                    });
            };

            $scope.startAssignment = function () {
                popup("/Templates/Verification/assign.html");
            }

            $scope.startCall = function () {
                popup("/Templates/Verification/startCall.html");
            }

            $scope.endCall = function () {
                popup("/Templates/Verification/endCall.html");
            }

            function popup(templateUrl) {
                var modal = $uibModal.open({
                    animation: true,
                    templateUrl: '/Templates/Verification/actions.html',
                    controller: 'verification.actions',
                    size: 'lg',
                    resolve: {
                        verification: function () {
                            return $scope.verification;
                        },
                        actionTemplate: function () {
                            return templateUrl;
                        },
                        patientName: function () {
                            return $scope.verification.patient.patientName;
                        }
                    }
                });

                modal.result.then(function (result) {
                    console.dir(result);
                }, function () {
                    console.info('Modal dismissed at: ' + new Date());
                });

                return modal;
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

    module.controller('verification.actions', [
        "$scope", "$api", "$uibModalInstance", "verification", "patientName", "actionTemplate", function ($scope, $api, $uibModalInstance, verification, patientName, actionTemplate) {
            $scope.verification = verification;
            $scope.actionTemplate = actionTemplate;
            $scope.patientName = patientName;
            $scope.modal = $uibModalInstance;
        }]);

    module.controller('verification.complete', [
        '$scope', '$api', '$state', function ($scope, $api, $state) {
            $scope.complete = function () {
                $api.verifications.complete(verificationId, $scope.verification).success(function () {
                    $scope.modal.close();
                    $state.go("verificationList");
                });
            }
        }
    ]);

    module.controller('verification.reject', [
        '$scope', '$api', '$state', function ($scope, $api, $state) {
            $scope.reject = function () {
                $api.verifications.reject(verificationId, $scope.verification).success(function () {
                    $scope.modal.close();
                    $state.go("verificationList");
                });
            }
        }
    ]);

    module.controller('verification.assignUser', [
        '$scope', '$api', function ($scope, $api) {
            $scope.verification = $scope.$parent.verification;
            var verificationId = $scope.$parent.verification.verificationId;

            $scope.select = function (user) {
                $scope.selectedUser = user;
            };

            $scope.assign = function () {
                $api.verifications.delegate(verificationId, $scope.selectedUser.userId).success(function() {
                    $scope.modal.close();
                });
            }

            var users = [];
            $scope.users = function () { return users; };

            $api.users.getAll().success(function (data) {
                users = data;
            });
        }
    ]);

    module.controller('verification.startCall', [
        "$scope", "$api", function ($scope, $api) {
            $scope.verification = $scope.$parent.verification;
            var verificationId = $scope.verification.verificationId;
            $scope.startCall = function () {
                $api.verifications.startCall(verificationId, {
                    serviceCenterRepresentative: $scope.serviceCenterRepresentative
                }).success(function () {
                    $scope.modal.close();
                });
            }
        }
    ]);

    module.controller('verification.onCall', [
        "$scope", "$api", function ($scope, $api) {
            $scope.verification = $scope.$parent.verification;
            var verificationId = $scope.$parent.verification.verificationId;
            $scope.callStart = function () {
                return new Date($scope.verification.currentCallStartTime).getTime();
            }

            $scope.save = function () {
                $api.verifications.update(verificationId, $scope.verification).success(function () {
                    $scope.modal.close();
                });
            }
        }
    ]);

    module.controller('verification.endCall', [
        "$scope", "$api", function ($scope, $api) {
            $scope.verification = $scope.$parent.verification;
            var verificationId = $scope.$parent.verification.verificationId;
            $scope.serviceCenterRepresentative = $scope.verification.serviceCenterRepresentative;

            $scope.endCall = function () {
                $api.verifications.endCall(verificationId, {
                    serviceCenterRepresentative: $scope.serviceCenterRepresentative,
                    callReferenceNumber: $scope.callReferenceNumber
                }).success(function () {
                    $scope.modal.close();
                });
            }
        }
    ]);

}(angular.module("app")));
