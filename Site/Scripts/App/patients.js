(function (module) {

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
                    $location.path("/patient");
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
        "$scope", "$routeParams", "patientCommands", function ($scope, $routeParams, $commands) {

            $scope.patientId = $routeParams["id"];
            $scope.insurance = {};

            $scope.save = function () {
                $commands.addInsurance($scope.patientId, $scope.insurance).success(function () {
                    $location.path("/patient");
                });
            }
        }
    ]);

}(angular.module("patientsApp", [])));
