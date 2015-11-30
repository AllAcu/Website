(function (app) {
    app.factory('patientCommands', ['$http', function ($http) {

        return {
            intake: function (patient) {
                return $http.post("/api/patient/", patient);
            },
            addInsurance: function (patientId, insurance) {
                return $http.post("/api/patient/" + patientId + "/insurance", {
                    medicalInsurance: insurance.classification === "medical" ? insurance.medical : null,
                    personalInjuryProtection: insurance.classification === "pip" ? insurance.pip : null
                });
            },
            update: function (patient) {
                return $http.put("/api/patient/" + patient.patientId, {
                    name: patient.name,
                    gender: patient.gender,
                    dateOfBirth: patient.dateOfBirth
                });
            },
            updateContactInfo: function (patientId, contact) {
                return $http.put("/api/patient/" + patientId + "/contact", {
                    address: {
                        line1: contact.address.address1,
                        line2: contact.address.address2,
                        city: contact.address.city,
                        state: contact.address.state,
                        postalCode: contact.address.postalCode
                    },
                    phoneNumber: contact.phoneNumber
                });
            }
        }
    }]);
})(window.app);