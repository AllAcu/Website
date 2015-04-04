(function (app) {
    app.factory('claimsRepository', ['$http', function ($http) {

        var claimRepository = {
            findAll: function () {
                return $http.get("/api/claims");
            }
        }

        return claimRepository;
    }
    ]);
})(window.app);