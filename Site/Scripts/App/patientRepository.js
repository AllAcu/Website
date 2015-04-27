(function (app) {
    app.factory('patientRepository', ['$http', function ($http) {

        return {
            findAll: function () {
                return $http.get("/api/patient");
            },
            find: function (id) {
                return $http.get("/api/patient/" + id).success(function (data) {
                    data.personalInformation.dateOfBirth = new Date(data.personalInformation.dateOfBirth);
                    return data;
                });
            },
            intake: function(patient) {
                return $http.post("/api/patient/", patient);
            },
            update: function(patient) {
                return $http.put("/api/patient/" + patient.id, {
                    name: patient.name,
                    gender: patient.gender,
                    dateOfBirth: patient.dateOfBirth
                });
            }
        }

    }]);
})(window.app);