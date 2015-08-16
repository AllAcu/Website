(function (app) {
    app.factory('patientRepository', ['$api', function ($api) {

        return {
            findAll: function () {
                return $api.patients.getAll();
            },
            details: function (id) {
                return $api.patients.get(id);
            },
            edit: function (id) {
                return $api.patients.edit(id);
            }
        }
    }]);
})(window.app);