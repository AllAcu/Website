(function (app) {
    app.factory('careProviderCommands', ['$api', function ($api) {
        return {
            create: function (provider) {
                return $api.providers.create(provider);
            },
            update: function (provider) {
                return $api.providers.update(provider);
            }
        };
    }]);
})(window.app);