(function (app) {
    app.factory('claimsRepository', ['$http', function ($http) {

        var claimRepository = {
            findAll: function () {
                return $http.get("/api/claim")
                    .success(function (data) {
                        data.forEach(function (item) {
                            item.visit.dateOfService = new Date(item.visit.dateOfService).toDateString();
                        });
                        return data;
                    });
            },
            find: function (id) {
                return $http.get("/api/claim/" + id)
                    .success(function (data) {
                        data.visit.dateOfService = new Date(data.visit.dateOfService);
                        return data;
                    });
            },
            create: function (draft) {
                return $http.post("/api/claim", draft);
            },
            update: function (draft) {
                return $http.put("/api/claim/", draft);
            },
            submit: function (draft) {
                return $http.post("/api/claim/submit/" + draft.id);
            }
        }

        return claimRepository;
    }
    ]);
})(window.app);