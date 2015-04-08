(function (app) {
    app.factory('claimsRepository', ['$http', function ($http) {

        var claimRepository = {
            findAll: function () {
                return $http.get("/api/claim");
            },
            find: function (id) {
                return $http.get("/api/claim/" + id);
            },
            create: function (draft) {
                return $http.post("/api/claim", draft);
            },
            update: function (draft) {
                return $http.put("/api/claim/", draft);
            },
            submit: function(draft) {
                return $http.post("/api/claim/submit/" + draft.id);
            }
        }

        return claimRepository;
    }
    ]);
})(window.app);