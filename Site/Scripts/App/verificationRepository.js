(function (app) {
    app.service('verificationRepository', ['$api', function ($api) {

        var verifications = {};

        function refresh() {
            return $api.verifications.getAll()
                .success(function (data) {
                    verifications = data;
                });
        }

        function refreshAuth() {
            return $api.verifications.getAuth();

            //return $http({
            //    method: 'GET',
            //    url: '/api/insurance/verification',
            //    headers: {
            //        "Authorization": "Bearer " + window.sessionStorage.getItem("accessToken")
            //    },
            //    transformRequest: function (obj) {
            //        var str = [];
            //        for (var p in obj)
            //            str.push(encodeURIComponent(p) + "=" + encodeURIComponent(obj[p]));
            //        return str.join("&");
            //    }
            //});
        }

        refresh();

        return {
            getVerification: function (id) {
                if (verifications[id] && verifications[id].request) {
                    return {
                        success: function (callback) {
                            callback(verifications[id]);
                        }
                    }
                };

                return $api.verifications.get(id)
                            .success(function (verification) {
                                verifications[verification.verificationId] = verification;
                            });
            },
            verifications: function () {
                return verifications;
            },
            refresh: function () {
                return refreshAuth().success(function (data) {
                    verifications = data;
                });
            }
        }
    }]);
})(window.app);