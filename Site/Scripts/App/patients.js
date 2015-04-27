(function(module) {

    module.controller('patientList', [
        "$scope", "patientRepository", function($scope, $patients) {
            $patients.findAll().success(function(data) {
                $scope.patients = data;
            });

            $scope.intakeName = "Jimmy";

            $scope.intake = function() {
                $patients.intake($scope.intakeName);
            }

            $scope.dismiss = function(patient) {
                alert("deleting " + patient.name);
            }
        }
    ]);

    module.controller('patientEdit', [
    "$scope", "$routeParams", "$location", "patientRepository", function ($scope, $routeParams, $location, $patients) {

        $scope.patient = {};
        $patients.find($routeParams["id"]).success(function (data) {
            $scope.patient = data.personalInformation;
            $scope.patient.id = data.patientId;
        });

        $scope.save = function () {
            $patients.update($scope.patient).success(function () {
                $location.path("/patient");
            });
        }
    }
    ]);

}(angular.module("patientsApp", [])));
