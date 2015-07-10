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
            }
        };

        return authToken;
    });
}(angular.module("authApp")));