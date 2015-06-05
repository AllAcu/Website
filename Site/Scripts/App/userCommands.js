(function (app) {
    app.factory('userCommands', ['$http', function ($http) {

        return {
            login: function (userName, password) {
                var loginData = {
                    grant_type: 'password',
                    username: userName,
                    password: password
                };

                return $http({
                    method: 'POST',
                    url: '/Token',
                    headers: {
                        'Content-Type': 'application/x-www-form-urlencoded'
                    },
                    transformRequest: function (obj) {
                        var str = [];
                        for (var p in obj)
                            str.push(encodeURIComponent(p) + "=" + encodeURIComponent(obj[p]));
                        return str.join("&");
                    },
                    data: loginData
                });
            },
            createUser: function (email, password, confirmPassword) {
                return $http.post('/api/Account/Register', {
                    Email: email,
                    Password: password,
                    ConfirmPassword: confirmPassword
                });
            }
        }
    }]);
})(angular.module("loginApp"));