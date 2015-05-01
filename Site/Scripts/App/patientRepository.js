(function (app) {
    app.factory('patientRepository', ['$http', function ($http) {

        return {
            findAll: function () {
                return $http.get("/api/patient");
            },
            details: function (id) {
                return $http.get("/api/patient/" + id);
            },
            edit: function (id) {
                return $http.get("/api/patient/edit/" + id).success(function (data) {
                    data.dateOfBirth = new Date(data.dateOfBirth);
                    return data;
                });
            }
        }
    }]);
})(window.app);