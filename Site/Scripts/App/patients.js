(function (module) {

    module.controller('patientIntake', [
        "$scope", "$location", "patientRepository", function ($scope, $location, $patients) {
            $scope.patient = {};

            $scope.save = function () {
                $patients.intake($scope.patient).success(function () {
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
        "$scope", "$routeParams", "$location", "patientRepository", function ($scope, $routeParams, $location, $patients) {

            $scope.patient = {};
            $patients.edit($routeParams["id"]).success(function (data) {
                $scope.patient = data;
            });

            $scope.save = function () {
                $patients.update($scope.patient).success(function () {
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

}(angular.module("patientsApp", [])));
