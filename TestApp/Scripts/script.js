/// <reference path="jquery-1.7.1-vsdoc.js" />
/// <reference path="qunit.js" />

function err(jqXHR, textStatus, errorThrown)
{ ok(false, textStatus + " " + errorThrown); }

module('JsAction');
test('Basic CRL type return', function () {
    JsActions.Home.GetInt({ error: err }).then(function (data) { equal(data, 1, 'Int return'); });
    JsActions.Home.GetFloat({ error: err }).then(function (data) { equal(data, 3.3, 'Float return'); });
    JsActions.Home.GetString({ error: err }).then(function (data) { equal(data, "This is a test string", 'String return'); });
});

test('Json object return', function () {
    var retobj = { theint: 3, thefloat: 3.3, thestring: "This is a string" };
    JsActions.Home.JsonObject({ error: err }).then(function (data) { same(data, retobj, 'Json object return'); equals(JSON.stringify(data), JSON.stringify(retobj), 'Json stringified object return'); });

    var obj = { integer: 3, thestring: 'Whatever your want, you can write it here.' };
    JsActions.Home.ComplexObjectSendAndReturn(obj, { error: err }).then(function (data) { equals(JSON.stringify(data), JSON.stringify(obj), 'Complex Json object send and retrieve'); });

});

test('JsAction generic features', function () {
    JsActions.Home.JsCode({ error: err });

    JsActions.Home.CachedTicks({ error: err }).then(function (data1) {
        JsActions.Home.CachedTicks({ error: err }).then(function (data2) {
            //Since request are cached, second time the first value will be returned.
            equal(data1, data2, 'Cached request');
        });
    });

    JsActions.Home.NonChachedTicks({ error: err }).then(function (data1) {
        JsActions.Home.NonChachedTicks({ error: err }).then(function (data2) {
            notEqual(data1, data2, 'Not cached request');
        });
    });

    ok(typeof (JsActions.Home.smpl10) != "undefined", 'JsAction renaming: smpl10 method existance');
    JsActions.Home.smpl10({ error: err }).then(function (data) { ok(true, 'Call to smpl10 forwarded to' + data); });

    stop();
    JsActions.Home.AsyncMethod({ error: err }).then(function (data) { ok(true, 'Async method called with "' + data + '" returned'); start(); });

    equal(typeof (JsActions.Home.NotMappedMethod), 'undefined', 'Skipped function due to NonAction attribute');

    var obj = [{ integer: 1, thestring: 'thestring' }, { integer: 2, thestring: 'bone'}];
    JsActions.Home.ListOfComplex(obj, { error: err }).then(function (data) { ok(true, 'List of complex objects'); });

    JsActions.Home.DateTimeObj(new Date(), { error: err }).then(function (data) { ok(true, 'DateTime send and receive ' + data.dt + '. Works also in subobjects: ' + data.wz.dt); });
});

test('JsAction WebApi support', function () {
    stop();
    JsActions.Api.Student.GetStudentList().then(function (data) { ok(typeof data != 'undefined', 'WebApi data retrieving'); start(); });
    stop();
    JsActions.Api.Student.GetById(2).then(function (data) { ok(typeof data != 'undefined', 'WebApi data retrieving 2'); start(); });
    stop();
    JsActions.Api.Student.PostStudent({ id: 3, Name: "Francisco", Surname: "Franco", BirthDay: new Date(), Exams: 15 }, { statusCode: { 200: function () { ok(true, 'New element inserted'); start(); } } });
    stop();
    JsActions.Api.Student.DeleteStudent(500, { statusCode: { 200: function () { ok(false, 'This is not good'); start(); }, 404: function () { ok(true, 'Element not found'); start(); } } });

});