/* ****************************************************************************
*  MvcControlToolkit.Bindings-2.0.0.js
* Copyright (c) Francesco Abbruzzese. All rights reserved.
* francesco@dotnet-programming.com
* http://www.dotnet-programming.com/
* 
* This software is subject to the the license at http://mvccontrolstoolkit.codeplex.com/license  
* and included in the license.txt file of this distribution.
* 
* You must not remove this notice, or any other, from this software.
*
* ***************************************************************************/
///////////////////////ClientViewModel Methods definition//////////
(function () {
    window["mvcct"] = window["mvcct"] || {};
    mvcct.utils = mvcct["utils"] || {};
    var reISO = /^(\d{4})-(\d{2})-(\d{2})T(\d{2}):(\d{2}):(\d{2}(?:\.\d*)?)Z$/;
    var dateRewriteOut = /\\\\\/Date\((\d+)(?:[-+]\d+)?\)\\\\\//ig;
    var dateRewriteIn = /\\\/Date\((\d+)(?:[-+]\d+)?\)\\\//ig;
    function stringify(json, isoDate) {

        var res = JSON.stringify(json, function (key, value) {
            if (typeof value == "string") {
                dateRewriteIn.lastIndex = 0;
                var a = dateRewriteIn.exec(value);
                if (a) {
                    var mill = parseInt(a[1]);
                    var corrected = mill;
                    if (isoDate) {
                        value = ISODateString(new Date(corrected));
                    }
                    else {
                        var date = new Date(corrected);
                        value = '\\/Date(' + Date.UTC(date.getFullYear(), date.getMonth(), date.getDate(), date.getHours(), date.getMinutes(), date.getSeconds(), date.getMilliseconds()) + ')\\/';
                    }
                    this[key] = value;
                    return value;
                }
            }
            return value;
        });
        if (!isoDate) res = res.replace(dateRewriteOut, "\\/Date($1)\\/");
        return res;
    }
    function ISODateString(d, simplified) {
        function pad(n) { return n < 10 ? '0' + n : n }
        return d.getUTCFullYear() + '-'
              + pad(d.getUTCMonth() + 1) + '-'
              + pad(d.getUTCDate()) + 'T'
              + pad(d.getUTCHours()) + ':'
              + pad(d.getUTCMinutes()) + ':'
              + pad(d.getUTCSeconds()) + (simplified ? '' : 'Z')
    }
    function property(item, name, wholeObservable) {
        if (!name) return item;
        if (name.charAt(0) == "#") {
            name = name.substring(1);
            item = window;
        }
        name = name.split(".");
        if (name.length == 0) return item;
        var res = item;
        for (var i = 0; i < name.length - 1; i++) {
            res = ko.utils.unwrapObservable(res[name[i]]);
        }
        res = res[name[name.length - 1]];
        if (wholeObservable) return res;
        else return ko.utils.unwrapObservable(res);
    }
    function propertySet(item, name, value, changeObservable) {
        name = name.split(".");
        var res = item;
        for (var i = 0; i < name.length - 1; i++) {
            var nRes = ko.utils.unwrapObservable(res[name[i]]);
            if (!nRes) { nRes = {}; res[name[i]] = nRes; }
            res = nRes;
        }
        if ((!changeObservable) && ko.isObservable(res[name[name.length - 1]]))
            res[name[name.length - 1]](value);
        else
            res[name[name.length - 1]] = value;
    }
    mvcct.utils.property = property;
    mvcct.utils.propertySet = propertySet;
    mvcct.utils.classof = function (o) {
        if (o === null) {
            return "null";
        }
        if (o === undefined) {
            return "undefined";
        }
        return Object.prototype.toString.call(o).slice(8, -1).toLowerCase();
    };
    mvcct.utils.isDate = function (o) {
        return mvcct.utils.classof(o) === "date";
    };
    mvcct.utils.isFunction = function (o) {
        return mvcct.utils.classof(o) === "function";
    };
    mvcct.utils.isObject = function (o) {
        return mvcct.utils.classof(o) === "object";
    };
    mvcct.utils.isString = function (o) {
        return (typeof o === "string");
    };
    mvcct.utils.isGuid = function (o) {
        return (typeof o === "string") && /[a-fA-F\d]{8}-(?:[a-fA-F\d]{4}-){3}[a-fA-F\d]{12}/.test(o); ;
    };
    mvcct.utils.isArray = function (o) {
        return mvcct.utils.classof(o) === "array";
    };
    mvcct.utils.stringify = function (obj, isoDate) {
        return stringify(obj, isoDate);
    };
    mvcct.utils.ISODate = function (obj, simplified) {
        return ISODateString(obj, simplified);
    };
    mvcct.utils.idFromName = function (name) {
        return !name ? name : name.replace(/[\$\[\]\.]/g, '_');
    };
    mvcct.utils.changeIndex = function (prefixId, newPrefixId, id, newIndex) {
        prefixId = prefixId + '['; //add [
        newPrefixId = newPrefixId + '['; //add 
        if (!(id.indexOf(prefixId) === 0)) return null;
        id = id.slice(prefixId.length);
        var closeSquare = id.indexOf(']');
        if (closeSquare <= 0) return null;
        var index = id.slice(0, closeSquare);
        index = parseInt(index);
        if (isNaN(index)) return null;

        if (mvcct.utils.isFunction(newIndex)) index = newIndex(index);
        else index = newIndex;

        return newPrefixId + index + id.slice(closeSquare);
    };
    mvcct.utils.cloneEntity = function (x, visitRelation, cloneArray) {
        if (!x) return x;
        var obs = ko.isObservable(x);
        x = ko.utils.unwrapObservable(x);
        var type = this.classof(x);
        if (type == "object") {
            var y = {};
            for (var property in x) {
                if (property == '__ko_mapping__' || ((!ko.isObservable(x[property])) && (this.classof(x[property]) == 'function'))) continue;
                y[property] = this.cloneEntity(x[property],
                    this.isFunction(visitRelation) ? visitRelation(property) : visitRelation,
                    this.isFunction(cloneArray) ? cloneArray(property) : cloneArray);
            }
            return obs ? ko.observable(y) : y;
        }
        else if (type == "array") {
            if (!visitRelation) return null;
            y = [];
            for (var i = 0; i < x.length; i++) {
                if (cloneArray) y.push(this.cloneEntity(x[i], visitRelation, cloneArray));
                y.push(x[i]);
            }
            return obs ? ko.observableArray(y) : y;
        }
        else {
            y = x;
            return obs ? ko.observable(y) : y;
        }

    };
    mvcct.utils.updateCopy = function (x) {
        if (!x['_oldValue_']) return x;
        var tData = x._oldValue_();
        return mvcct.utils.cloneEntity(x, tData.vr, tData.ca);
    }
    mvcct.utils.restoreEntity = function (x, y, visitRelation) {
        var obs = ko.isObservable(x);
        var orOb = x;
        x = ko.utils.unwrapObservable(x);
        var z = ko.utils.unwrapObservable(y);
        var type;
        if (x) type = this.classof(x);
        else type = this.classof(z);
        if (type == "object") {
            for (var property in x) {
                if (property == '__ko_mapping__' || ((!ko.isObservable(x[property])) && (this.classof(x[property]) == 'function'))) continue;
                if (y[property] !== undefined || property == "ModelPrefix" || property == "ModelId" || property == "_tag") {
                    y[property] = this.restoreEntity(x[property], y[property], this.isFunction(visitRelation) ? visitRelation(property) : visitRelation);
                }
            }
            return y;
        }
        else if (type == "array") {
            if (!visitRelation) return y;
            if (obs) y(x);
            else y = x;
            return y;
        }
        else {
            if (obs) y(x);
            else y = x;
            return y;
        }

    };
    mvcct.utils.Track = function (x, visitRelation, cloneArray) {
        if (x['_oldValue_']) return;
        var tData = {
            value: mvcct.utils.cloneEntity(x, visitRelation, cloneArray),
            vr: visitRelation,
            ca: cloneArray
        };
        x['_oldValue_'] = function () { return tData };
    };
    mvcct.utils.accept = function (x) {
        if (!x['_oldValue_']) return;
        x._oldValue_ = false;
    };
    mvcct.utils.undo = function (x) {
        if (!x['_oldValue_']) return;
        var tData = x._oldValue_();
        mvcct.utils.restoreEntity(tData.value, x, tData.vr);
    };
    mvcct.utils.changed = function (x) {
        if (!x['_oldValue_']) return true;
        var tData = x._oldValue_();
        return mvcct.utils.compareEntities(tData.value, x, tData.vr, tData.ca);
    }
    mvcct.utils.compareEntities = function (x, y, visitRelation, verifyRelation) {
        y = ko.utils.unwrapObservable(y);
        x = ko.utils.unwrapObservable(x);
        if (x === y) return false;
        var type;
        if (x) type = this.classof(x);
        else type = this.classof(y);
        if (type == "object") {
            for (var property in x) {
                if (property == '_modified' || property == '__ko_mapping__' || ((!ko.isObservable(x[property])) && (this.classof(x[property]) == 'function'))) continue;
                if (this.compareEntities(x[property], y[property],
                    this.isFunction(visitRelation) ? visitRelation(property) : visitRelation,
                    this.isFunction(verifyRelation) ? verifyRelation(property) : verifyRelation
                )) return true;
            }
            return false;
        }
        else if (type == "array") {
            if (!visitRelation) return false;
            if (x.length != y.length) return true;
            for (var i = 0; i < x.length; i++) {
                if (x[i] != y[i]) return true;
                if (verifyRelation) {
                    if (this.compareEntities(x[i], y[i], visitRelation, verifyRelation)) return true;
                }
            }
            return false;
        }
        else if (type == "date") {
            if (this.classof(y) != "date") return true;
            return x.getTime() != y.getTime();
        }
        else {
            return y !== x;
        }

    };
})();
//////////////////////////////////////////////////////////////////

function MvcControlsToolkit_ClientViewModel_Init(viewModel, jsonHiddenId, validationType) {
    viewModel.save = function () {
        document.getElementById(jsonHiddenId).value = mvcct.utils.stringify(ko.mapping.toJS(this));
    };
    viewModel.validateAndSave = function () {
        if (MvcControlsToolkit_FormIsValid(jsonHiddenId, validationType)) {
            document.getElementById(jsonHiddenId).value = ko.mapping.toJSON(this);
            return true;
        }
        return false;
    }
    viewModel.saveAndSubmit = function () {
        if (this.validateAndSave()) {
            $('#' + jsonHiddenId).parents('form').submit();
        }
    };
    viewModel.saveAndSubmitAlone = function (formId) {
        if (MvcControlsToolkit_FormIsValid(formId, validationType)) {
            this.save();
            $('#' + jsonHiddenId).parents('form').submit();
        }
    }
    $(document).ready(function () {
        $('#' + jsonHiddenId).parents('form').submit(function () {
            viewModel.save();
            return true;
        });
    });
}
///////////////////////Template Names//////////////////////////////

function MvcControlsToolkit_NewTemplateName(item, data, override) {
    if (!data['ModelPrefix']) {
        data.ModelId = item.ModelId;
        data.ModelPrefix = item.ModelPrefix;
    }
    if (eval("typeof _MvcControlsToolkit_" + data.ModelId + " === 'undefined'")) {
        jQuery.globalEval("var _MvcControlsToolkit_" + data.ModelId + " = 0;");
        data._tag = data["_tag"] || eval("_MvcControlsToolkit_" + data.ModelId);
    }
    else {
        var tag = data["_tag"];
        if (tag === undefined || override) {
            eval("_MvcControlsToolkit_" + data.ModelId + "++ ;");
            data._tag = eval("_MvcControlsToolkit_" + data.ModelId)
        }
    }
    if (!data['_inserted']) data._inserted = ko.observable(false);
    if (!data['_modified']) data._modified = ko.observable(false);
    return "";
}
function MvcControlsToolkit_TemplateName(item, data) {
    return item["Single"] || false ?
            data.ModelPrefix :
            data.ModelPrefix + "[" + data["_tag"] + "]" + item.ItemPrefix;
}
function MvcControlsToolkit_TemplateId(item, data) {
    return item["Single"] || false ?
            data.ModelId :
            data.ModelId + "_" + data["_tag"] + "_" + item.ItemPrefix;
}
///////////////////////////Templates/////////////////////////////
var MvcControlsToolkit_TemplatingLevel = 0;
function MvcControlsToolkit_InitializeCreatedNodes(elements, unobtrusiveValidation, noJavaScript, applyValidation, delayedBatch) {
    MvcControlsToolkit_TemplatingLevel--;
    if (elements == null || noJavaScript) return;
    toApply = function () {
        for (var i = 0; i < elements.length; i++) {
            var jel = $(elements[i]);
            if (jel.data('isCached') || false) return;
            if (unobtrusiveValidation && applyValidation) {
                jQuery.validator.unobtrusive.parseExt(elements[i]);
            }
            if (!noJavaScript) GlobalEvalScriptAndDestroy(elements[i]);


        }
        //        if (!noInput) {
        //            for (var i = 0; i < elements.length; i++) {
        //                    var jel = $(elements[i]);
        //                    var sons = jel.children();
        //                    sons.trigger('blur');
        //                    sons.trigger('change');
        //            }
        //        }
    };
    toApply();
}

function MvcControlsToolkit_ServerErrors(elements) {
    var ToApply = function () {
        for (var i = 0; i < elements.length; i++) {
            var currElement = elements[i];
            if (currElement == '') continue;
            var currDom = $('#' + currElement.id);
            if (currDom.length == 0) continue;
            var currForm = currDom.parents('form').first();
            if (currForm.length == 0) continue;

            if (!currDom.hasClass('input-validation-error'))
                currDom.addClass('input-validation-error');
            var attr = currDom.attr('data-companionpostfix');
            if (typeof attr !== 'undefined' && attr !== false) {
                var companion = $('#' + currElement.id + attr);
                if (companion.length > 0 && !companion.hasClass('input-validation-error'))
                    companion.addClass('input-validation-error');
            }
            var currDisplay = $(currForm).find("[data-valmsg-for='" + currElement.name + "']");
            if (currDisplay.length > 0) {
                currDisplay.removeClass("field-validation-valid").addClass("field-validation-error");
                replace = $.parseJSON(currDisplay.attr("data-valmsg-replace")) !== false;
                if (replace) {
                    currDisplay.empty();
                    $(currElement.errors[0]).appendTo(currDisplay);
                }
            }
        }
    };
    setTimeout(ToApply, 0);
}

(function () {
    var mappingProperty = "__ko_mapping__";
    var defaultOptions;
    var _defaultOptions = {
        include: ["_destroy"],
        ignore: [],
        copy: []
    };
    function fillOptions(options, otherOptions) {
        options = options || {};

        // Is there only a root-level mapping present?
        if ((options.create instanceof Function) || (options.update instanceof Function) || (options.key instanceof Function) || (options.arrayChanged instanceof Function)) {
            options = {
                "": options
            };
        }

        if (otherOptions) {
            options.ignore = mergeArrays(otherOptions.ignore, options.ignore);
            options.include = mergeArrays(otherOptions.include, options.include);
            options.copy = mergeArrays(otherOptions.copy, options.copy);
        }
        options.ignore = mergeArrays(options.ignore, defaultOptions.ignore);
        options.include = mergeArrays(options.include, defaultOptions.include);
        options.copy = mergeArrays(options.copy, defaultOptions.copy);

        options.mappedProperties = options.mappedProperties || {};
        return options;
    }

    function mergeArrays(a, b) {
        if (!(a instanceof Array)) {
            if (typeof a === "undefined") a = [];
            else a = [a];
        }
        if (!(b instanceof Array)) {
            if (typeof b === "undefined") b = [];
            else b = [b];
        }
        return a.concat(b);
    }
    ko.mapping.toJS = function (rootObject, options) {
        defaultOptions = ko.mapping.defaultOptions();
        if (!defaultOptions) ko.mapping.defaultOptions(
            {
                include: _defaultOptions.include.slice(0),
                ignore: _defaultOptions.ignore.slice(0),
                copy: _defaultOptions.copy.slice(0)
            }
        );

        if (arguments.length == 0) throw new Error("When calling ko.mapping.toJS, pass the object you want to convert.");
        if (!(defaultOptions.ignore instanceof Array)) throw new Error("ko.mapping.defaultOptions().ignore should be an array.");
        if (!(defaultOptions.include instanceof Array)) throw new Error("ko.mapping.defaultOptions().include should be an array.");
        if (!(defaultOptions.copy instanceof Array)) throw new Error("ko.mapping.defaultOptions().copy should be an array.");

        // Merge in the options used in fromJS
        options = fillOptions(options, rootObject[mappingProperty]);

        // We just unwrap everything at every level in the object graph
        return ko.mapping.visitModel(rootObject, function (x) {
            var res = ko.utils.unwrapObservable(x);
            if (Object.prototype.toString.call(res) === '[object Date]')
                res = '\\/Date(' + res.getTime() + ')\\/';
            return res;
        }, options);
    };
})();
ko.utils.arrayBestIndexOf = function (array, item, isFloat) {
    if (!isFloat) return ko.utils.arrayIndexOf(array, item);
    var j = array.length;
    if (j == 0) return -1;
    if (item === undefined) return 0;
    var besterror = Math.abs(array[0] - item);
    var bestIndex = 0;
    if (array[0] === undefined) {
        if (array.length <= 1) return -1;
        besterror = Math.abs(array[1] - item);
        bestIndex = 1;
    }
    var currError;
    for (var i = 1; i < j; i++) {
        currError = Math.abs(array[i] - item);
        if (currError < besterror) {
            bestIndex = i;
            besterror = currError;
        }
    }
    return bestIndex;
};

ko.bindingHandlers.value = {
    'init': function (element, valueAccessor, allBindingsAccessor) {
        var valueType = allBindingsAccessor()["valueType"] || MvcControlsToolkit_DataType_String;
        var elementType = $(element).attr("data-element-type") || "";
        var eventsToCatch = ["change"];
        var requestedEventsToCatch = allBindingsAccessor()["valueUpdate"];
        if (elementType != "") eventsToCatch = [elementType + "_Changed"];
        if (requestedEventsToCatch) {
            if (typeof requestedEventsToCatch == "string") // Allow both individual event names, and arrays of event names
                requestedEventsToCatch = [requestedEventsToCatch];
            ko.utils.arrayPushAll(eventsToCatch, requestedEventsToCatch);
            eventsToCatch = ko.utils.arrayGetDistinctValues(eventsToCatch);
        }

        ko.utils.arrayForEach(eventsToCatch, function (eventName) {
            // The syntax "after<eventname>" means "run the handler asynchronously after the event"
            // This is useful, for example, to catch "keydown" events after the browser has updated the control
            // (otherwise, ko.selectExtensions.readValue(this) will receive the control's value *before* the key event)
            var handleEventAsynchronously = false;
            if (elementType == "" && eventName.length > 7 && ko.utils.stringStartsWith(eventName, "after")) {
                handleEventAsynchronously = true;
                eventName = eventName.substring("after".length);
            }
            var runEventHandler = handleEventAsynchronously ? function (handler) { setTimeout(handler, 0) }
                                                        : function (handler) { handler() };

            ko.utils.registerEventHandler(element, eventName, function () {
                runEventHandler(function () {
                    var modelValue = valueAccessor();
                    var elementValue = null;
                    if (elementType != "") {
                        elementValue = eval("MvcControlsToolkit_" + elementType + "_Get(element, valueType)");
                    }
                    else {
                        elementValue = MvcControlsToolkit_Parse(
                        ko.selectExtensions.readValue(element),
                        valueType);
                    }
                    if (isNaN(elementValue) && elementType == "") elementValue = ko.selectExtensions.readValue(element);
                    if (ko.isWriteableObservable(modelValue))
                        modelValue(elementValue);
                    else {
                        var allBindings = allBindingsAccessor();
                        if (allBindings['_ko_property_writers'] && allBindings['_ko_property_writers']['value'])
                            allBindings['_ko_property_writers']['value'](elementValue);
                    }
                });
            });
        });
    },
    'update': function (element, valueAccessor, allBindingsAccessor) {
        var valueType = allBindingsAccessor()["valueType"] || MvcControlsToolkit_DataType_String;
        var formatString = allBindingsAccessor()["formatString"] || '';
        var elementType = $(element).attr("data-element-type") || "";
        if (elementType != "") eventName = elementType + "_changed";

        var newValue = ko.utils.unwrapObservable(valueAccessor());

        var elementValue = null;
        if (elementType != "") {
            try {
                elementValue = eval("MvcControlsToolkit_" + elementType + "_Get(element, valueType)");
            }
            catch (ex) { }
        }
        else {
            elementValue = MvcControlsToolkit_Parse(
                    ko.selectExtensions.readValue(element),
                    valueType);
        }
        if (isNaN(elementValue) && elementType == "") elementValue = ko.selectExtensions.readValue(element);
        var valueHasChanged = (newValue != elementValue);
        if (elementValue instanceof Array) {
            if (newValue instanceof Array) {
                if (newValue.length != elementValue.length) valueHasChanged = true;
                else {
                    valueHasChanged = false;
                    for (var i = 0, j = newValue.length; i < j; i++) {
                        if (newValue[i] != elementValue[i]) {
                            valueHasChanged = true;
                            break;
                        }
                    }
                }
            }
            else valueHasChanged = true;
        }
        // JavaScript's 0 == "" behavious is unfortunate here as it prevents writing 0 to an empty text box (loose equality suggests the values are the same). 
        // We don't want to do a strict equality comparison as that is more confusing for developers in certain cases, so we specifically special case 0 != "" here.
        else if ((newValue === 0) && (elementValue !== 0) && (elementValue !== "0"))
            valueHasChanged = true;

        if (valueHasChanged) {
            var convertedValue = null;
            var applyValueAction = null;
            if (elementType != "") {
                applyValueAction = function () {
                    eval("MvcControlsToolkit_" + elementType + "_Set(element, newValue, formatString, valueType);");
                    MvcControlsToolkit_RefreshWidget(element);
                };
            }
            else {
                convertedValue = MvcControlsToolkit_ToString(newValue, formatString, valueType);
                applyValueAction = function () {
                    if (element.tagName == 'SELECT' && valueType == MvcControlsToolkit_DataType_Float) {
                        if (element.options.length > 0) {
                            var besterror = Math.abs(MvcControlsToolkit_Parse(ko.selectExtensions.readValue(element.options[0]), valueType) - newValue);
                            var bestIndex = 0;
                            for (var i = element.options.length - 1; i >= 0; i--) {
                                var currError = Math.abs(MvcControlsToolkit_Parse(ko.selectExtensions.readValue(element.options[i]), valueType) - newValue);
                                if (currError < besterror) {
                                    besterror = currError;
                                    bestIndex = i;
                                }
                            }
                            element.selectedIndex = bestIndex;
                        }
                    }
                    else
                        ko.selectExtensions.writeValue(element, convertedValue);
                    MvcControlsToolkit_RefreshWidget(element);
                    ko.utils.triggerEvent(element, "blur");
                };
            }
            applyValueAction();

            // Workaround for IE6 bug: It won't reliably apply values to SELECT nodes during the same execution thread
            // right after you've changed the set of OPTION nodes on it. So for that node type, we'll schedule a second thread
            // to apply the value as well.
            var alsoApplyAsynchronously = element.tagName == "SELECT";
            if (alsoApplyAsynchronously)
                setTimeout(applyValueAction, 0);
        }

        // For SELECT nodes, you're not allowed to have a model value that disagrees with the UI selection, so if there is a
        // difference, treat it as a change that should be written back to the model

        if (element.tagName == "SELECT") {
            if (elementType != "") {
                elementValue = eval("MvcControlsToolkit_" + elementType + "_Get(element, valueType)");
            }
            else {
                elementValue = MvcControlsToolkit_Parse(
                    ko.selectExtensions.readValue(element),
                    valueType);
            }
            if (elementValue !== newValue)
                ko.utils.triggerEvent(element, "change");
        }

    }
};
ko.bindingHandlers.selectedOptions = {
    getSelectedValuesFromSelectNode: function (selectNode, valueType, all) {
        var result = [];
        var nodes = selectNode.childNodes;
        for (var i = 0, j = nodes.length; i < j; i++) {
            var node = nodes[i];
            if (node.tagName == "OPTGROUP") {
                var opts = node.childNodes;
                for (var n = 0, l = opts.length; n < l; n++) {
                    var opt = opts[n];
                    if ((opt.tagName == "OPTION") && (opt.selected || all != null))
                        result.push(MvcControlsToolkit_Parse(ko.selectExtensions.readValue(opt), valueType));
                }
            }
            else if ((node.tagName == "OPTION") && (node.selected || all != null))
                result.push(MvcControlsToolkit_Parse(ko.selectExtensions.readValue(node), valueType));
        }
        return result;
    },
    'init': function (element, valueAccessor, allBindingsAccessor) {
        var valueType = allBindingsAccessor()["valueType"] || MvcControlsToolkit_DataType_String;
        ko.utils.registerEventHandler(element, "change", function () {
            var value = valueAccessor();
            var newValue = ko.bindingHandlers.selectedOptions.getSelectedValuesFromSelectNode(this, valueType);
            if (ko.isWriteableObservable(value))
                value(newValue);
            else {
                var allBindings = allBindingsAccessor();
                if (allBindings['_ko_property_writers'] && allBindings['_ko_property_writers']['value'])
                    allBindings['_ko_property_writers']['value'](newValue);
            }
        });
    },
    'update': function (element, valueAccessor, allBindingsAccessor) {
        if (element.tagName != "SELECT")
            throw new Error("values binding applies only to SELECT elements");
        var valueType = allBindingsAccessor()["valueType"] || MvcControlsToolkit_DataType_String;
        var newValue = ko.utils.unwrapObservable(valueAccessor());
        if (newValue && typeof newValue.length == "number") {
            var allElements = ko.bindingHandlers.selectedOptions.getSelectedValuesFromSelectNode(element, valueType, true);
            var chosenIdexes = [];
            for (var i = 0, j = newValue.length; i < j; i++) chosenIdexes.push(ko.utils.arrayBestIndexOf(allElements, newValue[i], (valueType == MvcControlsToolkit_DataType_Float)));
            var nodes = element.childNodes;
            var opCount = 0;
            for (var i = 0, j = nodes.length; i < j; i++) {
                var node = nodes[i];
                if (node.tagName == "OPTGROUP") {
                    var opts = node.childNodes;
                    for (var n = 0, l = opts.length; n < l; n++) {
                        var opt = opts[n];
                        if (opt.tagName == "OPTION") {

                            opt.selected = ko.utils.arrayBestIndexOf(chosenIdexes, opCount, false) >= 0;
                            opCount++;
                        }
                    }
                }
                else if (node.tagName == "OPTION") {

                    node.selected = ko.utils.arrayBestIndexOf(chosenIdexes, opCount, false) >= 0;
                    opCount++;
                }
            }
        }
        MvcControlsToolkit_RefreshWidget(element);
    }
};
ko.bindingHandlers.checked = {
    'init': function (element, valueAccessor, allBindingsAccessor) {
        var valueType = allBindingsAccessor()["valueType"] || MvcControlsToolkit_DataType_String;
        var updateHandler = function () {
            var valueToWrite;
            if (element.type == "checkbox") {
                valueToWrite = element.checked;
            } else if ((element.type == "radio") && (element.checked)) {
                valueToWrite = MvcControlsToolkit_Parse(element.value, valueType);
            } else {
                return; // "checked" binding only responds to checkboxes and selected radio buttons
            }

            var modelValue = valueAccessor();
            if ((element.type == "checkbox") && (ko.utils.unwrapObservable(modelValue) instanceof Array)) {
                // For checkboxes bound to an array, we add/remove the checkbox value to that array
                // This works for both observable and non-observable arrays
                var elementValue = MvcControlsToolkit_Parse(element.value, valueType);
                var existingEntryIndex = ko.utils.arrayIndexOf(ko.utils.unwrapObservable(modelValue), MvcControlsToolkit_Parse(elementValue, valueType));
                if (element.checked && (existingEntryIndex < 0))
                    modelValue.push(elementValue);
                else if ((!element.checked) && (existingEntryIndex >= 0))
                    modelValue.splice(existingEntryIndex, 1);
            } else if (ko.isWriteableObservable(modelValue)) {
                if (modelValue() !== valueToWrite) { // Suppress repeated events when there's nothing new to notify (some browsers raise them)
                    modelValue(valueToWrite);
                }
            } else {
                var allBindings = allBindingsAccessor();
                if (allBindings['_ko_property_writers'] && allBindings['_ko_property_writers']['checked']) {
                    allBindings['_ko_property_writers']['checked'](valueToWrite);
                }
            }
            return true;
        };
        ko.utils.registerEventHandler(element, "click", updateHandler);

        // IE 6 won't allow radio buttons to be selected unless they have a name
        if ((element.type == "radio") && !element.name)
            ko.bindingHandlers['uniqueName']['init'](element, function () { return true });
    },
    'update': function (element, valueAccessor, allBindingsAccessor) {
        var value = ko.utils.unwrapObservable(valueAccessor());
        var valueType = allBindingsAccessor()["valueType"] || MvcControlsToolkit_DataType_String;
        if (element.type == "checkbox") {
            if (value instanceof Array) {
                // When bound to an array, the checkbox being checked represents its value being present in that array
                element.checked = ko.utils.arrayIndexOf(value, MvcControlsToolkit_Parse(element.value, valueType)) >= 0;
            } else {
                // When bound to anything other value (not an array), the checkbox being checked represents the value being trueish
                element.checked = value;
            }

            // Workaround for IE 6 bug - it fails to apply checked state to dynamically-created checkboxes if you merely say "element.checked = true"
            if (value && ko.utils.isIe6)
                element.mergeAttributes(document.createElement("<input type='checkbox' checked='checked' />"), false);
        } else if (element.type == "radio") {
            var nodeValue = MvcControlsToolkit_Parse(element.value, valueType);
            element.checked = (nodeValue == value);

            // Workaround for IE 6/7 bug - it fails to apply checked state to dynamically-created radio buttons if you merely say "element.checked = true"
            if ((nodeValue == value) && (ko.utils.isIe6 || ko.utils.isIe7))
                element.mergeAttributes(document.createElement("<input type='radio' checked='checked' />"), false);
        }
        MvcControlsToolkit_RefreshWidget(element);
    }
};
function MvcControlsToolkit_GetArrayString(value, arrayName, isNullable) {
    var allValues = eval(arrayName);
    if (value === null) return allValues[0];
    var index = 0;
    if (value === false) index = 0;
    else if (value === true) index = 1;
    else index = value;
    if (isNullable) index++;
    return allValues[index];
}
ko.bindingHandlers['template']["getCachedNodes"] = function (value) { return null; };
///////////////native engine modifications///////////////////////
(function () {
    ko.nativeTemplateEngine.prototype['renderTemplateSource'] = function (templateSource, bindingContext, options) {
        var data = bindingContext['$data'];
        MvcControlsToolkit_TemplatingLevel++;
        var cachedNodes = ko.bindingHandlers['template']['getCachedNodes'](data);
        if (cachedNodes != null) {
            $(cachedNodes).data('isCached', true);
            return cachedNodes;
        }
        var templateText = templateSource.text();
        templateText = templateText
            .replace(/agkcvriopjvss/g, "script");
        if (options['templateOptions'] && options.templateOptions['ModelPrefix']) {
            MvcControlsToolkit_NewTemplateName(options.templateOptions, data);
            var itemName = MvcControlsToolkit_TemplateName(options.templateOptions, data);
            var itemId = MvcControlsToolkit_TemplateId(options.templateOptions, data);
            var tsn = new RegExp(options.templateOptions['templateSymbol'] + "\\.A", "g");
            var tsi = new RegExp(options.templateOptions['templateSymbol'] + "_A", "g");
            templateText = templateText
            .replace(tsn, itemName)
            .replace(tsi, itemId);
        }
        return ko.utils.parseHtmlFragment(templateText);
    };

    ko.nativeTemplateEngine.instance = new ko.nativeTemplateEngine();
    ko.setTemplateEngine(ko.nativeTemplateEngine.instance);

})();
//////////////////////////////////////////////////////////
//////////// jquery plugin template engine extended//////////////////
ko.jqueryTmplTemplateEngineExt = function () {
    // Detect which version of jquery-tmpl you're using. Unfortunately jquery-tmpl 
    // doesn't expose a version number, so we have to infer it.
    // Note that as of Knockout 1.3, we only support jQuery.tmpl 1.0.0pre and later,
    // which KO internally refers to as version "2", so older versions are no longer detected.
    var jQueryTmplVersion = this.jQueryTmplVersion = (function () {
        if ((typeof (jQuery) == "undefined") || !(jQuery['tmpl']))
            return 0;
        // Since it exposes no official version number, we use our own numbering system. To be updated as jquery-tmpl evolves.
        try {
            if (jQuery['tmpl']['tag']['tmpl']['open'].toString().indexOf('__') >= 0) {
                // Since 1.0.0pre, custom tags should append markup to an array called "__"
                return 2; // Final version of jquery.tmpl
            }
        } catch (ex) { /* Apparently not the version we were looking for */ }

        return 1; // Any older version that we don't support
    })();

    function ensureHasReferencedJQueryTemplates() {
        if (jQueryTmplVersion < 2)
            throw new Error("Your version of jQuery.tmpl is too old. Please upgrade to jQuery.tmpl 1.0.0pre or later.");
    }

    function executeTemplate(compiledTemplate, data, jQueryTemplateOptions) {
        return jQuery['tmpl'](compiledTemplate, data, jQueryTemplateOptions);
    }
    this['allowTemplateRewriting'] = false;
    this['renderTemplateSource'] = function (templateSource, bindingContext, options) {
        options = options || {};
        var data = bindingContext['$data'];
        MvcControlsToolkit_TemplatingLevel++;
        var cachedNodes = ko.bindingHandlers['template']['getCachedNodes'](data);
        if (cachedNodes != null) {
            $(cachedNodes).data('isCached', true);
            return cachedNodes;
        }
        ensureHasReferencedJQueryTemplates();

        // Ensure we have stored a precompiled version of this template (don't want to reparse on every render)
        var precompiled = templateSource['data']('precompiled');
        if (!precompiled) {
            var templateText = templateSource.text() || "";
            var tsn = new RegExp(options.templateOptions['templateSymbol'] + "\\.A", "g");
            var tsi = new RegExp(options.templateOptions['templateSymbol'] + "_A", "g");
            templateText = templateText
            .replace(/agkcvriopjvss/g, "script")
            .replace(tsn, "${MvcControlsToolkit_TemplateName($item, $data)}")
            .replace(tsi, "${MvcControlsToolkit_TemplateId($item, $data)}");
            // Wrap in "with($whatever.koBindingContext) { ... }"
            templateText = "{{ko_with $item.koBindingContext}} ${MvcControlsToolkit_NewTemplateName($item, $data) }" + templateText + "{{/ko_with}}";

            precompiled = jQuery['template'](null, templateText);
            templateSource['data']('precompiled', precompiled);
        }

        var data = [data]; // Prewrap the data in an array to stop jquery.tmpl from trying to unwrap any arrays
        var jQueryTemplateOptions = jQuery['extend']({ 'koBindingContext': bindingContext }, options['templateOptions']);

        var resultNodes = executeTemplate(precompiled, data, jQueryTemplateOptions);
        resultNodes['appendTo'](document.createElement("div")); // Using "appendTo" forces jQuery/jQuery.tmpl to perform necessary cleanup work
        jQuery['fragments'] = {}; // Clear jQuery's fragment cache to avoid a memory leak after a large number of template renders
        return resultNodes;
    };

    this['createJavaScriptEvaluatorBlock'] = function (script) {
        return "{{ko_code ((function() { return " + script + " })()) }}";
    };

    this['addTemplate'] = function (templateName, templateMarkup) {
        document.write("<script type='text/html' id='" + templateName + "'>" + templateMarkup + "</script>");
    };

    if (jQueryTmplVersion > 0) {
        jQuery['tmpl']['tag']['ko_code'] = {
            open: "__.push($1 || '');"
        };
        jQuery['tmpl']['tag']['ko_with'] = {
            open: "with($1) {",
            close: "} "
        };
    }
};
(function () {
    ko.jqueryTmplTemplateEngineExt.prototype = new ko.templateEngine();
    // Use this one by default *only if jquery.tmpl is referenced*
    ko.jqueryTmplTemplateEngineExt.instance = new ko.jqueryTmplTemplateEngineExt();
    if (ko.jqueryTmplTemplateEngineExt.instance.jQueryTmplVersion > 0)
        ko.setTemplateEngine(ko.jqueryTmplTemplateEngineExt.instance);
})();


//////////

//////////////////////////////////////////////////////////
//////////// undescore.js plugin template engine extended//////////////////
ko.underscoreTemplateEngineExt = function () { }
ko.underscoreTemplateEngineExt.prototype = ko.utils.extend(new ko.templateEngine(), {
    allowTemplateRewriting: false,
    renderTemplateSource: function (templateSource, bindingContext, options) {
        var data = bindingContext['$data'];
        MvcControlsToolkit_TemplatingLevel++;
        var cachedNodes = ko.bindingHandlers['template']['getCachedNodes'](data);
        if (cachedNodes != null) {
            $(cachedNodes).data('isCached', true);
            return cachedNodes;
        }
        // Precompile and cache the templates for efficiency
        var precompiled = templateSource['data']('precompiled');
        if (!precompiled) {
            var tsn = new RegExp(options.templateOptions['templateSymbol'] + "\\.A", "g");
            var tsi = new RegExp(options.templateOptions['templateSymbol'] + "_A", "g");
            var templateText = templateSource.text() || "";
            templateText = templateText
            .replace(/agkcvriopjvss/g, "script")
            .replace(tsn, "<%= MvcControlsToolkit_TemplateName($options, $data) %>")
            .replace(tsi, "<%= MvcControlsToolkit_TemplateId($options, $data) %>");
            precompiled = _.template("<% MvcControlsToolkit_NewTemplateName($options, $data) %> <% with($data) { %> " + templateText + " <% } %>");
            templateSource['data']('precompiled', precompiled);
        }
        var context = ko.utils.extend(ko.utils.extend({ $options: options["templateOptions"] || {} }, bindingContext), options["templateOptions"] || {});
        // Run the template and parse its output into an array of DOM elements
        var renderedMarkup = precompiled(context).replace(/\s+/g, " ");
        return ko.utils.parseHtmlFragment(renderedMarkup);
    },
    createJavaScriptEvaluatorBlock: function (script) {
        return "<%= " + script + " %>";
    }
});
(function () {
    if ((typeof _ != "undefined") && _["template"]) {
        ko.underscoreTemplateEngineExt.instance = new ko.underscoreTemplateEngineExt();
        ko.setTemplateEngine(ko.underscoreTemplateEngineExt.instance);
    }
})();
/////////////////////////////////////////////////////
ko.bindingHandlers['template']['originalUpdate'] = ko.bindingHandlers['template']['update'];
ko.bindingHandlers['template']['update'] = function (element, valueAccessor, allBindingsAccessor, viewModel, bindingContext) {
    var bindingValue = ko.utils.unwrapObservable(valueAccessor());
    var oldAfterRender = bindingValue["afterRender"];
    var processingOptions = bindingValue["processingOptions"] || {};
    var defaultOptions =
    { unobtrusiveClient: true,
        fastNoInput: false,
        fastNoJavaScript: false,
        applyClientValidation: true
    };
    ko.utils.extend(
        defaultOptions,
        processingOptions);
    processingOptions = defaultOptions;
    if (oldAfterRender) {
        bindingValue["afterRender"] = function (x, y) {
            MvcControlsToolkit_InitializeCreatedNodes(x, processingOptions.unobtrusiveClient, processingOptions.fastNoJavaScript, processingOptions.applyClientValidation);
            oldAfterRender(x, y);
        };
    }
    else {
        bindingValue["afterRender"] = function (x, y) {
            MvcControlsToolkit_InitializeCreatedNodes(x, processingOptions.unobtrusiveClient, processingOptions.fastNoJavaScript, processingOptions.applyClientValidation);
        };
    }

    ko.bindingHandlers['template']['originalUpdate'](element, function () { return bindingValue; }, allBindingsAccessor, viewModel, bindingContext);
    var oldDO = $(element).data("__mvcct_template_afterRender__") || null;
    if (oldDO != null && (typeof (oldDO.dispose) == 'function')) oldDO.dispose();
    $(element).data("__mvcct_template_afterRender__", null);
    if (typeof bindingValue['afterAllRender'] == 'function') {
        var dependentObservable = new ko.dependentObservable(
    function () {
        var unwrappedArray = ko.utils.unwrapObservable(bindingValue['foreach']);
        bindingValue['afterAllRender'](element, viewModel);
    },
    null,
    { 'disposeWhenNodeIsRemoved': element });
        $(element).data("__mvcct_template_afterRender__", dependentObservable)
    }

}


ko.bindingHandlers['options']['originalUpdate'] = ko.bindingHandlers['options']['update'];
ko.bindingHandlers['options']['update'] = function (element, valueAccessor, allBindingsAccessor, viewModel) {
    var value = ko.utils.unwrapObservable(valueAccessor());
    if (typeof value.length != "number") value = [value];
    var valueType = allBindingsAccessor()["valueType"] || MvcControlsToolkit_DataType_String;
    var formatString = allBindingsAccessor()["formatString"] || '';
    var textType = allBindingsAccessor()["textType"] || MvcControlsToolkit_DataType_String;
    var textFormatString = allBindingsAccessor()["textFormatString"] || '';
    var textPrefix = allBindingsAccessor()["textPrefix"] || '';
    var textPostfix = allBindingsAccessor()["textPostfix"] || '';
    var textNullString = allBindingsAccessor()["textNullString"] || '';
    var convertedValues = [];
    var allBindings = allBindingsAccessor();
    for (var i = 0, j = value.length; i < j; i++) {
        if (typeof allBindings['optionsValue'] == "string") {
            var optionValue = value[i][allBindings['optionsValue']];
            optionValue = ko.utils.unwrapObservable(optionValue);
            var optionsTextValue = allBindings['optionsText'];
            var item = {};
            item[allBindings['optionsValue']] = MvcControlsToolkit_ToString(ko.utils.unwrapObservable(value[i][allBindings['optionsValue']]), formatString, valueType)
            if (typeof optionsTextValue == "string") {
                var textValue = ko.utils.unwrapObservable(ko.utils.unwrapObservable(value[i])[optionsTextValue]);
                if (textValue === null)
                    item[optionsTextValue] = textNullString;
                else
                    item[optionsTextValue] = textPrefix + MvcControlsToolkit_ToString(textValue, textFormatString, textType) + textPostfix;
            }
            convertedValues.push(item);
        }
        else {

            convertedValues.push(MvcControlsToolkit_ToString(ko.utils.unwrapObservable(value[i]), formatString, valueType));
        }

    }
    ko.bindingHandlers['options']['originalUpdate'](element, function () { return convertedValues; }, allBindingsAccessor, viewModel);

}