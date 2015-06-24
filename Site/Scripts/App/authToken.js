(function (app) {
    app.factory('authToken', function () {

        return {
            get: function() {
                return window.sessionStorage.getItem("accessToken");
            },
            set: function(token) {
                window.sessionStorage.setItem("accessToken", token);
            }
        }
    });
}(angular.module("loginApp")));