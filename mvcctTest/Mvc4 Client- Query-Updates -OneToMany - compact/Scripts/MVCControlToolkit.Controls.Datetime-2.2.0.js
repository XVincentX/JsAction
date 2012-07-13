/* ****************************************************************************
*
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

// DATE AND TIME FUNCTIONS

var defaultYear = 1970 + 0;
var defaultMonth = 0 + 0;
var defaultDay = 1 + 0;
var defaultHour = 0 + 0;
var defaultMinute = 0 + 0;
var defaultSecond = 0 + 0;


function DateInput_Initialize(id) {
    if (document.getElementById(id + "_Year") != null) {
        document.getElementById(id + "_Year").onkeypress = DateInputYearKeyVerify;
        document.getElementById(id + "_Year").onpaste = DateInputYearHandlePaste;
        document.getElementById(id + "_Year").ondrop = DateInputYearHandlePaste;
        document.getElementById(id + "_Year").onchange = DateInputChanged;
    }
    if (document.getElementById(id + "_Month") != null)
        document.getElementById(id + "_Month").onchange = DateInputChanged;

    if (document.getElementById(id + "_Day") != null)
        document.getElementById(id + "_Day").onchange = DateInputChanged;

    if (document.getElementById(id + "_Hours") != null)
        document.getElementById(id + "_Hours").onchange = DateInputChanged;

    if (document.getElementById(id + "_Minutes") != null)
        document.getElementById(id + "_Minutes").onchange = DateInputChanged;

    if (document.getElementById(id + "_Seconds") != null)
        document.getElementById(id + "_Seconds").onchange = DateInputChanged;

    if (eval(id + "_DateInCalendar")) {
        var options = eval(id + "_CalendarOptions");

        $('#' + id + '_Calendar').datepicker(options);
        


    }
    DateInputChanged(null, id, true, null, true);
    $('#' + id + '_Hidden').data('ready', true);

}

function DateInputGetNumDays(M, curYear) {
    M = M + 1;
    if (curYear % 4 == 0) {
        return (M == 9 || M == 4 || M == 6 || M == 11) ? 30 : (M == 2) ? 29 : 31;
    } else {
        return (M == 9 || M == 4 || M == 6 || M == 11) ? 30 : (M == 2) ? 28 : 31;
    }
}

function DateTimeAdjustYears(cmbInput, min, max) {
    if (cmbInput == null || cmbInput.tagName != 'SELECT') return;
    var j = 0;
    if (min == cmbInput.options[0].value && max == cmbInput.options[cmbInput.options.length - 1].value) return;
    var oldVar = cmbInput.value;
    cmbInput.options.length = 0;
    for (i = min; i <= max; i++) {
        if (i < 10)
            cmbInput.options[j] = new Option("   " + i, i);
        else if (i < 100)
            cmbInput.options[j] = new Option("  " + i, i);
        else if (i < 1000)
            cmbInput.options[j] = new Option(" " + i, i);
        else
            cmbInput.options[j] = new Option("" + i, i);
        j++;
    }
    MvcControlsToolKit_SetDateElement(cmbInput.id, oldVar);
}

function DateTimeAdjustMonthes(cmbInput, min, max) {
    if (cmbInput == null) return;
    var j = 0;
    if (min == cmbInput.options[0].value && max == cmbInput.options[cmbInput.options.length - 1].value) return;
    var oldVar = cmbInput.value;
    cmbInput.options.length = 0;
    for (i = min; i <= max; i++) {
        cmbInput.options[j] = new Option(DateTimeMonthes[i], i + 1);
        j++;
    }
    MvcControlsToolKit_SetDateElement(cmbInput.id, oldVar);
}

function DateTimeAdjustDays(cmbInput, min, max) {
    if (cmbInput == null) return;
    var j = 0;
    if (min == cmbInput.options[0].value && max == cmbInput.options[cmbInput.options.length - 1].value) return;
    var oldVar = cmbInput.value;
    cmbInput.options.length = 0;
    for (i = min; i <= max; i++) {
        if (i < 10)
            cmbInput.options[j] = new Option(" " + i, i);
        else
            cmbInput.options[j] = new Option("" + i, i);
        j++;
    }
    MvcControlsToolKit_SetDateElement(cmbInput.id, oldVar);
}
function DateTimeAdjustTimeElement(cmbInput, min, max) {
    if (cmbInput == null) return;
    var j = 0;
    if (min == cmbInput.options[0].value && max == cmbInput.options[cmbInput.options.length - 1].value) return;
    var oldVar = cmbInput.value;
    cmbInput.options.length = 0;
    for (i = min; i <= max; i++) {
        if (i < 10)
            cmbInput.options[j] = new Option("0" + i, i);
        else
            cmbInput.options[j] = new Option("" + i, i);
        j++;
    }
    MvcControlsToolKit_SetDateElement(cmbInput.id, oldVar);
}

function DateInputYearHandlePaste(evt) {

    evt = (evt) ? (evt) : ((window.event) ? (window.event) : null);
    if (evt == null) return true;

    var sorg = (evt.target) ? (evt.target) : ((event.srcElement) ? (event.srcElement) : null);
    if (sorg == null) return true;

    var val;
    if (evt.type == "paste")
        val = window.clipboardData.getData("Text");
    else if (evt.type == "drop")
        val = evt.dataTransfer.getData("Text");
    else
        return true;


    for (i = 0; i < val.length; i++) {
        keyCode = val.charCodeAt(i);

        if (keyCode == 13 || keyCode == 8)
            continue;

        if ((keyCode >= 48) && (keyCode <= 57))
            continue;
        else
            return false;

    }
    sorg.value = val;
    return false;
}

function DateInputYearKeyVerify(evt) {
    evt = (evt) ? (evt) : ((window.event) ? (window.event) : null);
    if (evt == null) return true;

    var sorg = (evt.target) ? (evt.target) : ((event.srcElement) ? (event.srcElement) : null);
    if (sorg == null) return true;

    var keyCode = ((evt.charCode || evt.initEvent) ? evt.charCode : ((evt.which) ? evt.which : evt.keyCode));


    if (keyCode == 0 || keyCode == 13 || keyCode == 8)
        return true;
    if ((keyCode >= 48) && (keyCode <= 57))
        return true;
    return false;
    var val = sorg.value;
}

function DateTimeInput_UpdateCalendar(clientId) {
    Nanno = document.getElementById(clientId + "_Year").value;
    Nmese = document.getElementById(clientId + "_Month").value;
    Ngiorno = document.getElementById(clientId + "_Day").value;

    var newDate = new Date(Nanno, Nmese - 1, Ngiorno);
    var dateHost = $('#' + clientId + "_Calendar");
    var format = dateHost.datepicker("option", "dateFormat");
    if (format == null) format = 'mm/dd/yy';

    dateHost.datepicker("setDate", $.datepicker.formatDate(format, newDate));

}

function DateTimeInput_UpdateFromCalendar(stringDate, clientId) {
    var dateHost = $('#' + clientId + "_Calendar");


    if (stringDate == null) return;

    var format = dateHost.datepicker("option", "dateFormat");
    if (format == null) format = 'mm/dd/yy';
    var date = null;
    try {
        date = $.datepicker.parseDate(format, stringDate);
    }
    catch (e) {
        date = new Date();
    }

    var stringDateMin = dateHost.datepicker("option", "minDate");
    var stringDateMax = dateHost.datepicker("option", "maxDate");

    var dateMin = null;
    var dateMax = null;

    if (stringDateMin != null) dateMin = $.datepicker.parseDate(format, stringDateMin);
    if (stringDateMax != null) dateMax = $.datepicker.parseDate(format, stringDateMax);

    if (dateMin != null && date < dateMin) {
        date = dateMin;
    }
    if (dateMax != null && date > dateMax) {
        date = dateMax;
    }


    document.getElementById(clientId + "_Year").value = date.getFullYear();
    document.getElementById(clientId + "_Month").value = date.getMonth() + 1;
    document.getElementById(clientId + "_Day").value = date.getDate();

    DateInputChanged(null, clientId, true);



}

function DateTimeInput_UpdateCalendarMinMax(clientId, minDate, maxDate) {
    var dateHost = $('#' + clientId + "_Calendar");
    var format = dateHost.datepicker("option", "dateFormat");
    if (format == null) format = 'mm/dd/yy';

    if (minDate != null) {
        dateHost.datepicker("option", "minDate", $.datepicker.formatDate(format, minDate));
    }
    else {
        dateHost.datepicker("option", "minDate", null);
    }
    if (maxDate != null) {
        dateHost.datepicker("option", "maxDate", $.datepicker.formatDate(format, maxDate));
    }
    else {
        dateHost.datepicker("option", "maxDate", null);
    }
}

function DateInputChanged(evt, cid, update, force, init) {

    var clientID;
    if (cid == null) {


        evt = (evt) ? (evt) : ((window.event) ? (window.event) : null);
        if (evt == null) {

            return false;
        }

        var sorg = (evt.target) ? (evt.target) : ((event.srcElement) ? (event.srcElement) : null);
        if (sorg == null) {

            return false;
        }
        clientID = sorg.id.substring(0, sorg.id.lastIndexOf("_"));
    }
    else {
        clientID = cid;
    }
    if (eval(clientID + "Recursive") == true && force == null) return;
    eval(clientID + "Recursive = true;");


    var Nanno;
    var Nmese;
    var Ngiorno;
    var NHours;
    var NMinutes;
    var NSeconds;
    var CurrDate = eval(clientID + "_Curr");
    var CurrDay = CurrDate.getDate();
    var CurrMonth = CurrDate.getMonth();
    var CurrYear = CurrDate.getFullYear();
    var CurrHours = CurrDate.getHours();
    var CurrMinutes = CurrDate.getMinutes();
    var CurrSeconds = CurrDate.getSeconds();

    var currMin = eval(clientID + "_MinDate");
    var currMax = eval(clientID + "_MaxDate");

    var dynamicMin = null;
    if (eval("(typeof " + clientID + "_ClientDynamicMin !== 'undefined') && (" + clientID + "_ClientDynamicMin != null)") == true) dynamicMin = eval(clientID + "_ClientDynamicMin()");

    var dynamicMax = null;
    if (eval("(typeof " + clientID + "_ClientDynamicMax !== 'undefined') && (" + clientID + "_ClientDynamicMax != null)") == true) dynamicMax = eval(clientID + "_ClientDynamicMax()");

    if (dynamicMin != null && (currMin == null || dynamicMin > currMin)) {
        if (currMax != null && dynamicMin > currMax)
            currMin = currMax;
        else
            currMin = dynamicMin;
    }
    if (dynamicMax != null && (currMax == null || dynamicMax < currMax)) {
        if (currMin != null && dynamicMax < currMin)
            currMax = currMin;
        else
            currMax = dynamicMax;
    }

    if (document.getElementById(clientID + "_Year") != null) {
        Nanno = document.getElementById(clientID + "_Year").value;
    }
    else {
        Nanno = CurrYear;
    }

    if (document.getElementById(clientID + "_Month") != null)
        Nmese = document.getElementById(clientID + "_Month").value;
    else
        Nmese = CurrMonth;

    if (document.getElementById(clientID + "_Day") != null)
        Ngiorno = document.getElementById(clientID + "_Day").value;
    else
        Ngiorno = CurrDay;

    if (document.getElementById(clientID + "_Hours") != null)
        NHours = document.getElementById(clientID + "_Hours").value;
    else
        NHours = CurrHours;

    if (document.getElementById(clientID + "_Minutes") != null)
        NMinutes = document.getElementById(clientID + "_Minutes").value;
    else
        NMinutes = CurrMinutes;

    if (document.getElementById(clientID + "_Seconds") != null)
        NSeconds = document.getElementById(clientID + "_Seconds").value;
    else
        NSeconds = CurrSeconds;

    var TempNewDate = new Date(Nanno, Nmese - 1, Ngiorno, NHours, NMinutes, NSeconds);

    if (currMax != null && currMax < TempNewDate) TempNewDate = currMax;
    if (currMin != null && currMin > TempNewDate) TempNewDate = currMin;

    Nanno = TempNewDate.getFullYear() + "";
    Nmese = (TempNewDate.getMonth() + 1) + "";
    Ngiorno = TempNewDate.getDate() + "";
    NHours = TempNewDate.getHours() + "";
    NMinutes = TempNewDate.getMinutes() + "";
    NSeconds = TempNewDate.getSeconds() + "";

    var NewYear;
    var NewMonth;
    var NewDay;
    var NewHours;
    var NewMinutes;
    var NewSeconds;
    var MaxYear;
    var MinYear;
    var MaxMonth;
    var MinMonth;
    var MinDay;
    var MaxDay;
    var MinHours;
    var MaxHours;
    var MinMinutes;
    var MaxMinutes;
    var MinSeconds;
    var MaxSeconds;
    eval(clientID + "_Valid = true");


    //year processing
    NewYear = parseInt(Nanno);
    if (!isNaN(NewYear)) {
        if (currMax == null) {
            MaxYear = null;
        }
        else {
            MaxYear = currMax.getFullYear();
        }
        if (currMin == null) {
            MinYear = null;
        }
        else {
            MinYear = currMin.getFullYear();
        }
        if (MaxYear != null && MaxYear < NewYear) NewYear = MaxYear;
        if (MinYear != null && MinYear > NewYear) NewYear = MinYear;
        if (document.getElementById(clientID + "_Year") != null && !eval(clientID + "_DateHidden") && !eval(clientID + "_DateInCalendar"))
            DateTimeAdjustYears(document.getElementById(clientID + "_Year"), MinYear, MaxYear);

        if ((MaxYear == null || MaxYear >= NewYear) && (MinYear == null || MinYear <= NewYear)) {

            //Month Processing
            MaxMonth = 11;
            MinMonth = 0;
            if (MaxYear == NewYear) {
                MaxMonth = currMax.getMonth();
            }
            if (MinYear == NewYear) {
                MinMonth = currMin.getMonth();
            }
            NewMonth = parseInt(Nmese);
            if (!isNaN(NewMonth)) {
                NewMonth = NewMonth - 1;
                if (MinMonth > NewMonth) {
                    NewMonth = MinMonth;
                }
                if (MaxMonth < NewMonth) {
                    NewMonth = MaxMonth;
                }
                if (init || CurrYear == MinYear || CurrYear == MaxYear || NewYear == MinYear || NewYear == MaxYear)
                    if (document.getElementById(clientID + "_Month") != null && !eval(clientID + "_DateHidden") && !eval(clientID + "_DateInCalendar"))
                        DateTimeAdjustMonthes(document.getElementById(clientID + "_Month"), MinMonth, MaxMonth);
                //day processing
                MinDay = 1;
                MaxDay = DateInputGetNumDays(NewMonth, NewYear);
                if (MaxYear == NewYear && MaxMonth == NewMonth) {
                    MaxDay = currMax.getDate();

                }
                if (MinYear == NewYear && MinMonth == NewMonth) {
                    MinDay = currMin.getDate();
                }
                NewDay = parseInt(Ngiorno);
                if (!isNaN(NewDay)) {
                    if (MinDay > NewDay) {
                        NewDay = MinDay;
                    }
                    if (MaxDay < NewDay) {
                        NewDay = MaxDay;
                    }
                    if (document.getElementById(clientID + "_Day") != null && !eval(clientID + "_DateHidden") && !eval(clientID + "_DateInCalendar"))
                        DateTimeAdjustDays(document.getElementById(clientID + "_Day"), MinDay, MaxDay);
                    // Hours Processing
                    MinHours = 0;
                    MaxHours = 23;
                    if (MaxYear == NewYear && MaxMonth == NewMonth && NewDay == MaxDay) {
                        MaxHours = currMax.getHours();
                    }
                    if (MinYear == NewYear && MinMonth == NewMonth && NewDay == MinDay) {
                        MinHours = currMin.getHours();
                    }
                    NewHours = parseInt(NHours);
                    if (!isNaN(NewHours)) {
                        if (MaxHours < NewHours) NewHours = MaxHours;
                        if (NewHours < MinHours) NewHours = MinHours;
                        if (document.getElementById(clientID + "_Hours") != null)
                            DateTimeAdjustTimeElement(document.getElementById(clientID + "_Hours"), MinHours, MaxHours);
                        // Minutes Processing
                        MinMinutes = 0;
                        MaxMinutes = 59;
                        if (MaxYear == NewYear && MaxMonth == NewMonth && NewDay == MaxDay && MaxHours == NewHours)
                            MaxMinutes = currMax.getMinutes();
                        if (MinYear == NewYear && MinMonth == NewMonth && NewDay == MinDay && MinHours == NewHours)
                            MinMinutes = currMin.getMinutes();
                        NewMinutes = parseInt(NMinutes);
                        if (!isNaN(NewMinutes)) {
                            if (MaxMinutes < NewMinutes) NewMinutes = MaxMinutes;
                            if (MinMinutes > NewMinutes) NewMinutes = MinMinutes;
                            if (document.getElementById(clientID + "_Minutes") != null)
                                DateTimeAdjustTimeElement(document.getElementById(clientID + "_Minutes"), MinMinutes, MaxMinutes);
                            // Seconds Processing
                            MinSeconds = 0;
                            MaxSeconds = 59;
                            if (MaxYear == NewYear && MaxMonth == NewMonth && NewDay == MaxDay && MaxHours == NewHours && MaxMinutes == NewMinutes)
                                MaxSeconds = currMax.getSeconds();
                            if (MinYear == NewYear && MinMonth == NewMonth && NewDay == MinDay && MinHours == NewHours && MinMinutes == NewMinutes)
                                MinSeconds = currMin.getSeconds();
                            NewSeconds = parseInt(NSeconds);
                            if (!isNaN(NewSeconds)) {
                                if (MaxSeconds < NewSeconds) NewSeconds = MaxSeconds;
                                if (NewSeconds < MinSeconds) NewSeconds = MinSeconds;
                                if (document.getElementById(clientID + "_Seconds") != null)
                                    DateTimeAdjustTimeElement(document.getElementById(clientID + "_Seconds"), MinSeconds, MaxSeconds);
                            }
                            else {
                                eval(clientID + "_Valid = false");
                            }
                        }

                        else {
                            eval(clientID + "_Valid = false");
                        }
                    }
                    else {
                        eval(clientID + "_Valid = false");
                    }
                }
                else {
                    eval(clientID + "_Valid = false");
                }
            }
            else {
                eval(clientID + "_Valid = false");
            }
        }
    }
    else {
        eval(clientID + "_Valid = false");
    }
    if (eval(clientID + "_DateInCalendar")) {
        DateTimeInput_UpdateCalendarMinMax(
        clientID,
        currMin,
        currMax);
    }
    var AChange = false;
    if (eval(clientID + "_Valid")) {
        if (update == true || (cid == null &&
            (CurrYear != NewYear || CurrMonth != NewMonth || CurrDay != NewDay ||
             CurrHours != NewHours || CurrMinutes != NewMinutes || CurrSeconds != NewSeconds)))
            AChange = true;
        CurrYear = NewYear;
        CurrMonth = NewMonth;
        CurrDay = NewDay;
        CurrHours = NewHours;
        CurrMinutes = NewMinutes;
        CurrSeconds = NewSeconds;
    }
    if (!AChange) {
        eval(clientID + "Recursive = false;")
        return true;
    }

    eval(clientID + "_Curr = new Date(CurrYear, CurrMonth, CurrDay, CurrHours, CurrMinutes, CurrSeconds)");

    if (document.getElementById(clientID + "_Year") != null) {
        MvcControlsToolKit_SetDateElement(clientID + "_Year", CurrYear);

    }
    if (document.getElementById(clientID + "_Month") != null) {
        MvcControlsToolKit_SetDateElement(clientID + "_Month", CurrMonth + 1);
    }
    if (document.getElementById(clientID + "_Day") != null) {
        MvcControlsToolKit_SetDateElement(clientID + "_Day", CurrDay);
    }
    if (eval(clientID + "_DateInCalendar")) {
        DateTimeInput_UpdateCalendar(clientID);
    }
    if (document.getElementById(clientID + "_Hours") != null) {
        MvcControlsToolKit_SetDateElement(clientID + "_Hours", CurrHours);
    }
    if (document.getElementById(clientID + "_Minutes") != null) {
        MvcControlsToolKit_SetDateElement(clientID + "_Minutes", CurrMinutes);
    }
    if (document.getElementById(clientID + "_Seconds") != null) {
        MvcControlsToolKit_SetDateElement(clientID + "_Seconds", CurrSeconds);
    }

    var currDate = eval(clientID + "_Curr");
    RefreshDependencies(clientID);
    eval(clientID + "_ClientDateChanged(" + currDate.getTime() + ")");
    $("#" + clientID + "_Hidden").trigger("DateTimeInput_Changed");
    eval(clientID + "Recursive = false;");
    return true;
}
function MvcControlsToolKit_SetDateElement(id, value) {
    var element = document.getElementById(id);
    if (element.tagName == 'SELECT') {
        value = parseInt(value);
        for (var i = element.options.length - 1; i >= 0; i--) {
            if (parseInt(element.options[i].value) <= value) {
                element.selectedIndex = i;
                return;
            }
        }
        element.selectedIndex = 0;
    } else {
        if ((value === null) || (value === undefined))
            value = "";
        element.value = value;
    }
}
function SetDateInput(id, value, cType) {
    if (eval("typeof " + id + "_Curr === 'undefined'") == true) return;
    var currDate = eval(id + "_Curr");

    if (currDate == null) return;
    var currDateInMilliseconds = currDate.getTime();

    if (cType == 1 && value >= currDateInMilliseconds) {
        return;
    }
    if (cType == 2 && value <= currDateInMilliseconds) {
        return;
    }
    var DateInFormat = new Date(value);
    var currMin = eval(id + "_MinDate");
    var currMax = eval(id + "_MaxDate");
    if (currMin != null && DateInFormat < currMin) DateInFormat = currMin;
    if (currMax != null && DateInFormat > currMax) DateInFormat = currMax;

    if (document.getElementById(id + "_Hours") != null) {
        if (document.getElementById(id + "_Year") != null) {
            MvcControlsToolKit_SetDateElement(id + "_Year", DateInFormat.getFullYear());
            DateInputChanged(null, id, false, true);
        }
        if (document.getElementById(id + "_Month") != null) {
            MvcControlsToolKit_SetDateElement(id + "_Month", DateInFormat.getMonth() + 1);
            DateInputChanged(null, id, false, true);
        }
        if (document.getElementById(id + "_Day") != null) {
            MvcControlsToolKit_SetDateElement(id + "_Day", DateInFormat.getDate());
            DateInputChanged(null, id, false, true);
        }
        if (document.getElementById(id + "_Hours") != null) {
            MvcControlsToolKit_SetDateElement(id + "_Hours", DateInFormat.getHours());
            DateInputChanged(null, id, false, true);
        }
        if (document.getElementById(id + "_Minutes") != null) {
            MvcControlsToolKit_SetDateElement(id + "_Minutes", DateInFormat.getMinutes());
            DateInputChanged(null, id, false, true);
        }
        if (document.getElementById(id + "_Seconds") != null) {
            MvcControlsToolKit_SetDateElement(id + "_Seconds", DateInFormat.getSeconds());
            DateInputChanged(null, id, true, true);
        }
    }
    else {
        if (document.getElementById(id + "_Year") != null) {
            MvcControlsToolKit_SetDateElement(id + "_Year", DateInFormat.getFullYear());
            DateInputChanged(null, id, false, true);
        }
        if (document.getElementById(id + "_Month") != null) {
            MvcControlsToolKit_SetDateElement(id + "_Month", DateInFormat.getMonth() + 1);
            DateInputChanged(null, id, false, true);
        }
        if (document.getElementById(id + "_Day") != null) {
            MvcControlsToolKit_SetDateElement(id + "_Day", DateInFormat.getDate());
            DateInputChanged(null, id, true, true);
        }
    }
    if (eval(id + "_DateInCalendar")) {
        DateTimeInput_UpdateCalendar(id);
    }

}

function GetDateInput(id) {
    return eval(id + "_Curr");
}

function MvcControlsToolkit_DateTimeInput_SetString(sorg, value) {
    clientID = sorg.id.substring(0, sorg.id.lastIndexOf("_"));
    var ob = null;
    if (value == null || value == "") {
        ob = new Date();
    }
    else {
        ob = MvcControlsToolkit_Parse(value, MvcControlsToolkit_DataType_DateTime);
    }
    SetDateInput(clientID, ob.getTime(), 3);
}

function MvcControlsToolkit_DateTimeInput_Set(sorg, value, format, valueType) {
    clientID = sorg.id.substring(0, sorg.id.lastIndexOf("_"));
    if ($('#' + sorg.id).length == 0 || eval("typeof " + clientID + "_Curr") === "undefined" || (!($('#' + clientID + '_Hidden').data('ready') || false))) {
        var retry = function () { MvcControlsToolkit_DateTimeInput_Set(sorg, value, format, valueType); };
        setTimeout(retry, 0);
        return;
    }
    if (value == null || value == "") value = new Date();
    SetDateInput(clientID, value.getTime(), 3);
}
function MvcControlsToolkit_DateTimeInput_SetById(id, value, format, valueType) {
    if (value == null || value == "") value = new Date();
    SetDateInput(id + '__', value.getTime(), 3);
}
function MvcControlsToolkit_DateTimeInput_Get(sorg, valueType) {
    clientID = sorg.id.substring(0, sorg.id.lastIndexOf("_"));
    if (eval("typeof " + clientID + "_Curr") === "undefined") return null;
    if (!($('#' + clientID + '_Hidden').data('ready') || false)) return null;
    return eval(clientID + "_Curr");
}
function MvcControlsToolkit_DateTimeInput_GetById(id, valueType) {
    return eval(id + "___Curr");
}
function MvcControlsToolkit_DateTimeInput_BindChange(id, handler) {
    $("#" + id + "___Hidden").bind("DateTimeInput_Changed", handler);
}
function MvcControlsToolkit_DateTimeInput_UnbindChange(id, handler) {
    $("#" + id + "___Hidden").unbind("DateTimeInput_Changed", handler);
}
function MvcControlsToolkit_DateTimeInput_GetString(sorg) {
    clientID = sorg.id.substring(0, sorg.id.lastIndexOf("_"));
    return MvcControlsToolkit_Format(GetDateInput(clientID), 's', MvcControlsToolkit_DataType_DateTime, '', '');
}

function AddToUpdateList(id, toAdd) {
    if (id == null || toAdd == null) return;

    var currIndex = eval(id + "_UpdateListIndex");
    eval(id + "_UpdateList[" + currIndex + "] = '" + toAdd + "';");
    currIndex++;
    eval(id + "_UpdateListIndex = " + currIndex + ";");
}

function RefreshDependencies(id) {
    if (eval("typeof " + id + "_UpdateListIndex === 'undefined'") == true) return;
    var length = eval(id + "_UpdateListIndex");
    if (length == null) return;
    for (var i = 0; i < length; i++) {
        DateInputChanged(null, eval(id + "_UpdateList[" + i + "]"), true);
    }
}



