(function (app) {
    app.factory('registrationCommands', ['$api', function ($api) {

        return {
            signup: function (email) {
                return $api.registration.signup(email);
            },
            invite: function() {
                
            },
            register: function (registration) {
                return $api.registration.register(registration);
            }
        }
    }]);
})(angular.module("registrationApp"));