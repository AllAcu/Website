(function (app) {
    app.factory('careProviderRepository', ['$http', function ($http) {
        var _providers = [];
        var _currentProvider = null;

        function setCurrentProvider(id) {
            var previous = _currentProvider;
            _currentProvider = _providers.filter(function (p) { return p.id === id })[0];

            if (previous !== _currentProvider) {
                $http.get("/api/provider/be/" + _currentProvider.id).then(function () {
                    location.reload();
                });
            }
        }

        $http.get("/api/provider")
             .success(function (data) {
                 _providers = data;
                 if (_providers) {
                     $http.get("/api/provider/who")
                         .success(function (current) {
                             _currentProvider = _providers.filter(function (p) { return p.id === current })[0];
                             if (!_currentProvider) {
                                 setCurrentProvider(_providers[0].id);
                             }
                         });
                 }
             });

        return {
            providers: function () {
                return _providers;
            },
            current: function (provider) {
                if (provider) {
                    setCurrentProvider(provider.id);
                }
                return _currentProvider;
            },
            setCurrent: setCurrentProvider,
            create: function (provider) {
                return $http.post("/api/provider/new", provider);
            }
        };
    }]);
})(window.app);