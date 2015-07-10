(function (app) {
    app.factory('careProviderRepository', ['$api', function ($api) {
        var _providers = [];
        var _currentProvider = null;

        function setCurrentProvider(id) {
            var previous = _currentProvider;
            _currentProvider = _providers.filter(function (p) { return p.id === id })[0];

            if (previous !== _currentProvider) {
                $api.providers.be(_currentProvider.id).success(function () {
                    location.reload();
                });
            }
        }

        function loadProviders() {
            $api.providers.get()
                .success(function (data) {
                    _providers = data;
                    if (_providers && _providers.length) {
                        $api.providers.who()
                            .success(function (current) {
                                _currentProvider = _providers.filter(function (p) { return p.id === current })[0];
                                if (!_currentProvider) {
                                    setCurrentProvider(_providers[0].id);
                                }
                            });
                    }
                })
            .error(function () {
                _providers = [];
            });
        }

        loadProviders();

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
                return $api.providers.create(provider);
            },
            refresh: function () {
                loadProviders();
            }
        };
    }]);
})(window.app);