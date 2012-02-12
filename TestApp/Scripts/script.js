/// <reference path="jquery-1.7.1-vsdoc.js" />
/// <reference path="qunit.js" />

module('JsAction');
test('CRL type return', function () {
    JsActions.GetInt().then(function (data) { equal(data, 1, 'Int return'); });
    JsActions.GetFloat().then(function (data) { equal(data, 3.3, 'Float return'); });
    JsActions.GetString().then(function (data) { equal(data, "This is a test string", 'String return'); });
    JsActions.JsonObject().then(function (data) { equal(data, 2, 'Json Object return'); });

});
