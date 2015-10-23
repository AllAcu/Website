(function (app) {
    app.factory('userSession',
        function () {
            var session = reset();

            function reset() {
                return {
                    logout: function() {
                        session = reset();
                    }
                };
            }

            return function() {
                return session;
            }
        }
    );
})(window.app);
