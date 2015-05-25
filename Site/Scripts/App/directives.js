(function (app) {

    app.directive('displayField', function () {
        return {
            restrict: "E",
            scope: {
                ngModel: "=ngModel",
                label: "@label"
            },
            template: '<div class="row"><div class="col-md-2"><label>{{label}}</label></div><div class="col-md-4">{{ngModel}}</div></div>'
        };
    });

    app.directive('editField', function () {
        return {
            restrict: "E",
            scope: {
                label: "@label",
                ngModel: "=ngModel",
                type: "@type"
            },
            template: '<div class="row"><div class="col-md-2"><label>{{label}}</label></div><div class="col-md-4"><input type="{{type}}" ng-model="ngModel" /></div></div>'
        };
    });

    app.directive('editText', function () {
        return {
            restrict: "E",
            scope: {
                label: "@label",
                ngModel: "=ngModel"
            },
            template: '<div class="row"><div class="col-md-2"><label>{{label}}</label></div><div class="col-md-4"><textarea style="width:100%;max-width:100%;" ng-model="ngModel"></textarea></div></div>'
        };
    });

    app.directive('editChoice', function () {
        return {
            restrict: "E",
            require: 'ngModel',
            scope: {
                label: "@label",
                ngModel: "=ngModel",
                options: "=options"
            },
            template: '<div class="row">' +
                    '<div class="col-md-2">' +
                        '<label>{{label}}</label>' +
                    '</div>' +
                    '<label class="col-md-3" ng-repeat="(key, value) in options">' +
                            '<input type="radio" ng-model="$parent.ngModel" ng-value="{{value.value}}">' +
                            '{{value.label}}' +
                    '</label>' +
                '</div>'
        };
    });

    app.directive('editYesNo', function () {
        return {
            restrict: "E",
            require: 'ngModel',
            scope: {
                label: "@label",
                ngModel: "=ngModel",
                options: "=options"
            },
            template: "<edit-choice ng-model='ngModel' options='[{ label: \"Yes\", value: \"true\"}, { label: \"No\", value: \"false\" }]'></edit-choice>"
        };
    });

    app.directive('displayAddress', function () {
        return {
            restrict: "E",
            scope: {
                address: "=ngModel"
            },
            template: '<div class="row">' +
                            '<div class="col-md-2">' +
                                '<label>Address</label>' +
                            '</div>' +
                            '<div class="col-md-4">' +
                                '<div>{{address.address1}}</div>' +
                                '<div>{{address.address2}}</div>' +
                                '<div>{{address.city}}, {{address.state}} {{address.postalCode}}</div>' +
                            '</div>' +
                        '</div>'
        };
    });

    app.directive('editAddress', function () {
        return {
            restrict: "E",
            scope: {
                address: "=ngModel"
            },
            template: '<div>' +
                    '<edit-field label="Address" ng-model="address.address1"></edit-field>' +
                    '<edit-field ng-model="address.address2"></edit-field>' +
                    '<edit-field label="City" ng-model="address.city"></edit-field>' +
                    '<edit-field label="State" ng-model="address.state"></edit-field>' +
                    '<edit-field label="Postal Code" ng-model="address.postalCode"></edit-field>' +
                    '</div>'
        };
    });

})(window.app)
