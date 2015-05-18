(function (app) {
    app.factory('authCommands', ['$http', function ($http) {

        return {
            login: function (userName, password) {
                return $http.post("login", {
                    userName: userName,
                    password: password
                });
            }
        }
    }]);
})(angular.module("loginApp"));