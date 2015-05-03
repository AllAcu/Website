﻿(function (module) {

    module.controller('patientIntake', [
        "$scope", "$location", "patientCommands", function ($scope, $location, $commands) {
            $scope.patient = {};

            $scope.save = function () {
                $commands.intake($scope.patient).success(function () {
                    $location.path("/patient");
                });
            }
        }
    ]);

    module.controller('patientList', [
        "$scope", "patientRepository", function ($scope, $patients) {
            $patients.findAll().success(function (data) {
                $scope.patients = data;
            });


            $scope.dismiss = function (patient) {
                alert("deleting " + patient.name);
            }
        }
    ]);

    module.controller('patientEdit', [
        "$scope", "$routeParams", "$location", "patientRepository", "patientCommands", function ($scope, $routeParams, $location, $patients, $commands) {

            $scope.patient = {};
            $patients.edit($routeParams["id"]).success(function (data) {
                $scope.patient = data;
            });

            $scope.save = function () {
                $commands.update($scope.patient).success(function () {
                    $location.path("/patient/" + $scope.patient.patientId);
                });
            }
        }
    ]);

    module.controller('patientDetails', [
        "$scope", "$routeParams", "patientRepository", function ($scope, $routeParams, $patients) {

            $scope.patient = {};
            $patients.details($routeParams["id"]).success(function (data) {
                $scope.patient = data;
            });
        }
    ]);

    module.controller('patientInsurance', [
        "$scope", "$routeParams", "$location", "patientRepository", "patientCommands", function ($scope, $routeParams, $location, $patients, $commands) {

            $scope.patientId = $routeParams["id"];
            $scope.classification = "medical";
            $scope.medical = {};
            $scope.pip = {};

            $patients.edit($routeParams["id"]).success(function (data) {
                $scope.medical = data.medicalInsurance;
                $scope.pip = data.personalInjuryProtection;
            });

            $scope.save = function () {
                $commands.addInsurance($scope.patientId, $scope).success(function () {
                    $location.path("/patient/" + $scope.patientId);
                });
            }

            $scope.insuranceTemplate = function () {
                if ($scope.classification === "medical")
                    return 'Templates/Patients/inputMedicalInsurance.html';
                else if ($scope.classification === "pip")
                    return 'Templates/Patients/inputPersonalInjuryProtection.html';
                return "";
            }
        }
    ]);
}(angular.module("patientsApp", [])));
