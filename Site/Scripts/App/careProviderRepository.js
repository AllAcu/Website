(function (app) {
    app.factory('careProviderRepository', ['$http', function ($http) {
        var careProviderRepository = {
            findAll: function() {
                return $http.get("/api/provider");
            },
            create: function(provider) {
                return $http.post("/api/provider/new", provider);
            },
            setCurrent: function(id) {
                return $http.get("/api/provider/be/" + id);
            },
            getCurrent: function() {
                return $http.get("/api/provider/who");
            }
        }

        return careProviderRepository;
    }]);
})(window.app);