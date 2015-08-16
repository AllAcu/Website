(function (app) {
    app.factory('careProviderRepository', ['$api', 'userSession', function ($api, session) {

        function setCurrentProvider(id) {
            var previous = session().currentProvider;
            session().currentProvider = session().providers.filter(function (p) { return p.id === id })[0];

            if (previous !== session().currentProvider) {
                $api.providers.be(session().currentProvider.id).success(function () {
                    location.reload();
                });
            }
        }

        function refresh() {
            return $api.providers.get()
                .success(function (data) {
                    session().providers = data;
                    if (session().providers && session().providers.length) {
                        $api.providers.who()
                            .success(function (current) {
                                session().currentProvider = session().providers.filter(function (p) { return p.id === current })[0];
                                if (!session().currentProvider) {
                                    setCurrentProvider(session().providers[0].id);
                                }
                            });
                    }
                })
            .error(function () {
                session().providers = [];
            });
        }

        refresh();

        return {
            providers: function () {
                if (!session().providers) {
                    session().providers = [];
                    refresh();
                }
                return session().providers;
            },
            current: function (provider) {
                if (provider) {
                    setCurrentProvider(provider.id);
                }
                return session().currentProvider;
            },
            edit: function(id) {
                return $api.providers.get(id);
            },
            setCurrent: setCurrentProvider,
            create: function (provider) {
                return $api.providers.create(provider);
            },
            refresh: function () {
                return refresh();
            }
        };
    }]);
})(window.app);