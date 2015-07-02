(function (app) {
    app.factory('userCommands', ['$api', function ($api) {

        return {
            login: function (userName, password) {
                var loginData = {
                    grant_type: 'password',
                    username: userName,
                    password: password
                };

                return $api.auth.login(loginData);
            },
            register: function (registration) {
                return $api.users.register(registration);
            }
        }
    }]);
})(angular.module("loginApp"));