(function (app) {
    app.factory('claimsRepository', ['$http', function ($http) {

        var claimRepository = {
            findAll: function () {
                return $http.get("/api/claims");
            },
            find: function(id) {
                return $http.get("/api/claim/" + id);
            }
        }

        return claimRepository;
    }
    ]);
})(window.app);