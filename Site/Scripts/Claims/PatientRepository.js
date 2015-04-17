(function (app) {
    app.factory('patientRepository', ['$http', function ($http) {

        var claimRepository = {
            findAll: function () {
                return $http.get("/api/patient");
            },
            find: function (id) {
                return $http.get("/api/patient/" + id);
            },
            intake: function(name) {
                return $http.post("/api/patient/", {
                    name: name
                });
            },
            update: function(patient) {
                return $http.put("/api/patient/" + patient.id, {
                    name: patient.name
                });
            }
        }

        return claimRepository;
    }]);
})(window.app);