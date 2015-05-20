(function (module) {

    module.controller('verifyInsurance', [
        "$scope", function ($scope) {

        $scope.patient = {
            carrier: "Premera",
            phone: "206-555-3939"
        };

        $scope.verification = {
            isCovered: "true"
        };

        $scope.save = function () {
            console.log(JSON.stringify($scope.verification));
        }
    }
    ]);

    module.controller('requestVerification', [
        "$scope", function ($scope) {

        $scope.save = function () {
            console.log(JSON.stringify($scope.verification));
        }
    }
    ]);

    module.controller('verifyInsuranceList', [
        "$scope", "verificationRepository", function ($scope, verifications) {

            verifications.findAll()
                .success(function(data) {
                    $scope.verifications = data;
                });

        }
    ]);




}(angular.module("insuranceApp")));
