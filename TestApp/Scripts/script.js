/// <reference path="jquery-1.7.1-vsdoc.js" />
/// <reference path="qunit.js" />

function err(jqXHR, textStatus, errorThrown)
{ ok(false, textStatus + " " + errorThrown); }

module('JsAction');
test('Basic CRL type return', function () {
    JsActions.GetInt({ error: err }).then(function (data) { equal(data, 1, 'Int return'); });
    JsActions.GetFloat({ error: err }).then(function (data) { equal(data, 3.3, 'Float return'); });
    JsActions.GetString({ error: err }).then(function (data) { equal(data, "This is a test string", 'String return'); });
});

test('Json object return', function () {
    var retobj = { theint: 3, thefloat: 3.3, thestring: "This is a string" };
    JsActions.JsonObject({ error: err }).then(function (data) { same(data,retobj,'Json object return'); equals(JSON.stringify(data), JSON.stringify(retobj), 'Json stringified object return'); });

    var obj = { integer: 3, thestring: 'Whatever your want, you can write it here.' };
    JsActions.ComplexObjectSendAndReturn(obj, { error: err }).then(function (data) { equals(JSON.stringify(data), JSON.stringify(obj), 'Complex Json object send and retrieve'); });

});

test('JsAction generic features', function () {
    JsActions.JsCode({ error: err });

    JsActions.CachedTicks({ error: err }).then(function (data1) {
        JsActions.CachedTicks({ error: err }).then(function (data2) {
            //Since request are cached, second time the first value will be returned.
            equal(data1, data2, 'Cached request');
        });
    });

    JsActions.NonChachedTicks({ error: err }).then(function (data1) {
        JsActions.NonChachedTicks({ error: err }).then(function (data2) {
            notEqual(data1, data2, 'Not cached request');
        });
    });
    
    ok(typeof(JsActions.smpl10) != "undefined", 'JsAction renaming: smpl10 method existance');
    JsActions.smpl10({ error: err }).then(function (data) { ok(true, 'Call to smpl10 forwarded to' + data); });

    stop();
    JsActions.AsyncMethod({ error: err }).then(function (data) { ok(true, 'Async method called with "' + data + '" returned'); start(); });

    equal(typeof(JsActions.NotMappedMethod), 'undefined', 'Skipped function due to NonAction attribute');
});

