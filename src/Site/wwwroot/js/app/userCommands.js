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
            signup: function(email) {
                return $api.users.signup(email);
            },
            invite: function (provider, email) {
                return $api.users.invite(provider, email);
            },
            register: function (token, name, password) {
                return $api.users.register(token, name, password);
            }
        }
    }]);
})(angular.module("app"));