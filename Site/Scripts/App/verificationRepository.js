(function (app) {
    app.service('verificationRepository', ['$api', 'userSession', function ($api, session) {

        function refresh() {
            if (!session().verifications) {
                session().verifications = {};
            }
            return $api.verifications.getAll()
                .success(function (data) {
                    session().verifications = data;
                });
        }

        refresh();

        return {
            getVerification: function (id) {
                if (session().verifications && session().verifications[id] && session().verifications[id].request) {
                    return {
                        success: function (callback) {
                            callback(session().verifications[id]);
                        }
                    }
                };

                return $api.verifications.get(id)
                            .success(function (verification) {
                                session().verifications[verification.verificationId] = verification;
                            });
            },
            verifications: function () {
                if (!session().verifications) {
                    session().verifications = {};
                    refresh();
                }
                return session().verifications;
            }
        }
    }]);
})(window.app);