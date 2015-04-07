(function (app) {
    app.factory('claimsRepository', ['$http', function ($http) {

        var claimRepository = {
            findAll: function () {
                return $http.get("/api/claim");
            },
            find: function (id) {
                return $http.get("/api/claim/" + id);
            },
            update: function (draft) {
                return $http.put("/api/claim/", draft);
            }
        }

        return claimRepository;
    }
    ]);
})(window.app);