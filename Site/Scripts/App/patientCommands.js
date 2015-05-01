(function (app) {
    app.factory('patientCommands', ['$http', function ($http) {

        return {
            intake: function (patient) {
                return $http.post("/api/patient/", patient);
            },
            addInsurance: function (patientId, insurance) {
                return $http.post("/api/patient/" + patientId + "/insurance", insurance);
            },
            update: function (patient) {
                return $http.put("/api/patient/" + patient.patientId, {
                    name: patient.name,
                    gender: patient.gender,
                    dateOfBirth: patient.dateOfBirth
                });
            }
        }
    }]);
})(window.app);