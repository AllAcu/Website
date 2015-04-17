(function (app) {
    app.factory('patientRepository', ['$http', function ($http) {

        var claimRepository = {
            findAll: function () {
                return $http.get("/api/patient").success(function(data) {
                    data.dateOfBirth = new Date(data.dateOfBirth);
                    return data;
                });
            },
            find: function (id) {
                return $http.get("/api/patient/" + id).success(function (data) {
                    data.dateOfBirth = new Date(data.dateOfBirth);
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

        return claimRepository;
    }]);
})(window.app);