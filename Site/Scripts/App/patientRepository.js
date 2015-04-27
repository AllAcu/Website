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
            intake: function(name) {
                return $http.post("/api/patient/", {
                    name: name
                });
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