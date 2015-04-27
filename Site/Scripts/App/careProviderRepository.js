(function (app) {
    app.factory('careProviderRepository', ['$http', function ($http) {
        var _providers = [];
        var _currentProvider = null;

        function setCurrentProvider(id) {
            var previous = _currentProvider;
            _currentProvider = _providers.filter(function (p) { return p.id === id })[0];
            if (previous && previous !== _currentProvider) {
                return $http.get("/api/provider/be/" + _currentProvider.id)
                .then(function () {
                    location.reload();
                });
            }
        }

        $http.get("/api/provider")
             .success(function (data) {
                 _providers = data;
                 $http.get("/api/provider/who")
                     .success(function (current) {
                         setCurrentProvider(current);
                     });
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
            setCurrent: setCurrentProvider
        };
    }]);
})(window.app);