(function (app) {
    app.factory('authToken', function () {

        var authToken = {
            loggedIn: function() {
                return !!authToken.get();
            },
            get: function() {
                return window.sessionStorage.getItem("accessToken");
            },
            set: function(token) {
                window.sessionStorage.setItem("accessToken", token);
            },
            clear: function() {
                window.sessionStorage.removeItem("accessToken");
            }
        };

        return authToken;
    });
}(angular.module("app")));