(function (app) {

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
                return '<div class="row"><div class="col-md-2"><label>{{label}}</label></div><div class="col-md-4"><input type="{{type}}" ng-model="{{field}}" /></div></div>'
                    .replace('{{label}}', attrs.label || "")
                    .replace('{{field}}', attrs.field)
                    .replace('{{type}}', attrs.type || "text");
            }
        };
    });

    app.directive('displayAddress', function () {
        return {
            restrict: "E",
            scope: {
                address: "=field"
            },
            template: function (element, attrs) {
                return ('<div class="row">' +
                            '<div class="col-md-2">' +
                                '<label>Address</label>' +
                            '</div>' +
                            '<div class="col-md-4">' +
                                '<div>{{address.address1}}</div>' +
                                '<div>{{address.address2}}</div>' +
                                '<div>{{address.city}}, {{address.state}} {{address.postalCode}}</div>' +
                            '</div>' +
                        '</div>');
            }
        };
    });

    app.directive('editAddress', function () {
        return {
            restrict: "E",
            scope: {
                address: "=field"
            },
            template: function (element, attrs) {
                return ('<div>' +
                    '<edit-field label="Address" field="address.address1"></edit-field>' +
                    '<edit-field field="address.address2"></edit-field>' +
                    '<edit-field label="City" field="address.city"></edit-field>' +
                    '<edit-field label="State" field="address.state"></edit-field>' +
                    '<edit-field label="Postal Code" field="address.postalCode"></edit-field>' +
                    '</div>');
            }
        };
    });



})(window.app)