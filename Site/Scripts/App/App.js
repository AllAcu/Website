﻿(function (exports, angular) {
    var app = angular.module('allAcuApp', [
        'ngRoute',
        'patientsApp',
        'providersApp',
        'claimsApp'
    ]);

    app.config([
        '$routeProvider',
        function ($routeProvider) {
            $routeProvider.
                when('/', {
                    templateUrl: '/Templates/Claims/claimsList.html',
                    controller: 'claimsList'
                }).
                when('/edit/:id', {
                    templateUrl: '/Templates/Claims/claimEdit.html',
                    controller: 'claimEdit'
                }).
                when('/create', {
                    templateUrl: '/Templates/Claims/claimEdit.html',
                    controller: 'claimCreate'
                }).
                when('/patient', {
                    templateUrl: '/Templates/Patients/patientList.html',
                    controller: 'patientList'
                }).
                when('/patient/intake', {
                    templateUrl: '/Templates/Patients/intake.html',
                    controller: 'patientIntake'
                }).
                when('/patient/:id/edit', {
                    templateUrl: '/Templates/Patients/updateVitalInfo.html',
                    controller: 'patientEdit'
                }).
                when('/patient/:id', {
                    templateUrl: '/Templates/Patients/details.html',
                    controller: 'patientDetails'
                }).
                when('/provider/create', {
                    templateUrl: '/Templates/Providers/providerEdit.html',
                    controller: 'providerCreate'
                }).
                when('/patient/:id/insurance/edit', {
                    templateUrl: '/Templates/Patients/recordInsurance.html',
                    controller: 'patientInsurance'
                }).
                otherwise({
                    redirectTo: '/'
                });
        }
    ]);

    app.controller('header', ['$scope', 'careProviderRepository', function ($scope, $providers) {
        $scope.providers = function () { return $providers.providers(); }
        $scope.currentProvider = function (provider) {
            return $providers.current(provider);
        }

        $scope.setProvider = function () {
            $providers.setCurrent($scope.currentProvider.id);
        }
    }]);


    app.directive('displayField', function () {
        return {
            restrict: "E",
            template: function (element, attrs) {
                return '<div class="row"><div class="col-md-2"><label>{{label}}</label></div><div class="col-md-4">{{{{field}}}}</div></div>'
                    .replace('{{label}}', attrs.label)
                    .replace('{{field}}', attrs.field);
            }
        };
    });

    app.directive('editField', function () {
        return {
            restrict: "E",
            template: function (element, attrs) {
                return '<div class="row"><div class="col-md-2"><label>{{label}}</label></div><div class="col-md-4"><input type="text" ng-model="{{field}}" /></div></div>'
                .replace('{{label}}', attrs.label)
                .replace('{{field}}', attrs.field);
            }
        };
    });

    exports.app = app;

})(window, angular)