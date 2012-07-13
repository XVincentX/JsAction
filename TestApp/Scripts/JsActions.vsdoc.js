if (typeof JsActions == 'undefined') {
    var JsActions = {};
    JsActions.WebApi = {};
}
JsActions.Home = {
    GetInt: function (options) { ///<param name="options" type="ajaxSettings">[OPTIONAL] AjaxOptions partial object; it will be mergend with the one sent to .ajax jQuery function</param><returns type="jqXHR"/>
    },
    GetFloat: function (options) { ///<param name="options" type="ajaxSettings">[OPTIONAL] AjaxOptions partial object; it will be mergend with the one sent to .ajax jQuery function</param><returns type="jqXHR"/>
    },
    GetString: function (options) { ///<param name="options" type="ajaxSettings">[OPTIONAL] AjaxOptions partial object; it will be mergend with the one sent to .ajax jQuery function</param><returns type="jqXHR"/>
    },
    JsonObject: function (options) { ///<param name="options" type="ajaxSettings">[OPTIONAL] AjaxOptions partial object; it will be mergend with the one sent to .ajax jQuery function</param><returns type="jqXHR"/>
    },
    ComplexObjectSendAndReturn: function (obj, options) { ///<param name="obj" type = "Complex"></param>
        ///<param name="options" type="ajaxSettings">[OPTIONAL] AjaxOptions partial object; it will be mergend with the one sent to .ajax jQuery function</param><returns type="jqXHR"/>
    },
    JsCode: function (options) { ///<param name="options" type="ajaxSettings">[OPTIONAL] AjaxOptions partial object; it will be mergend with the one sent to .ajax jQuery function</param><returns type="jqXHR"/>
    },
    CachedTicks: function (options) { ///<param name="options" type="ajaxSettings">[OPTIONAL] AjaxOptions partial object; it will be mergend with the one sent to .ajax jQuery function</param><returns type="jqXHR"/>
    },
    NonChachedTicks: function (options) { ///<param name="options" type="ajaxSettings">[OPTIONAL] AjaxOptions partial object; it will be mergend with the one sent to .ajax jQuery function</param><returns type="jqXHR"/>
    },
    smpl10: function (options) { ///<param name="options" type="ajaxSettings">[OPTIONAL] AjaxOptions partial object; it will be mergend with the one sent to .ajax jQuery function</param><returns type="jqXHR"/>
    },
    smpl20: function (options) { ///<param name="options" type="ajaxSettings">[OPTIONAL] AjaxOptions partial object; it will be mergend with the one sent to .ajax jQuery function</param><returns type="jqXHR"/>
    },
    AsyncMethod: function (options) { ///<param name="options" type="ajaxSettings">[OPTIONAL] AjaxOptions partial object; it will be mergend with the one sent to .ajax jQuery function</param><returns type="jqXHR"/>
    },
    ListOfComplex: function (listComplex, options) { ///<param name="listComplex" type = "ComplexList"></param>
        ///<param name="options" type="ajaxSettings">[OPTIONAL] AjaxOptions partial object; it will be mergend with the one sent to .ajax jQuery function</param><returns type="jqXHR"/>
    },
    DateTimeObj: function (dt, options) { ///<param name="dt" type = "DateTime"></param>
        ///<param name="options" type="ajaxSettings">[OPTIONAL] AjaxOptions partial object; it will be mergend with the one sent to .ajax jQuery function</param><returns type="jqXHR"/>
    }
};
if (typeof Complex == 'undefined') {
    function Complex(integer, thestring) {
        this.integer = integer;
        this.thestring = thestring;
    }
}
if (typeof DateTime == 'undefined') {
    function DateTime(Date, Day, DayOfWeek, DayOfYear, Hour, Kind, Millisecond, Minute, Month, Now, UtcNow, Second, Ticks, TimeOfDay, Today, Year) {
        this.Date = Date;
        this.Day = Day;
        this.DayOfWeek = DayOfWeek;
        this.DayOfYear = DayOfYear;
        this.Hour = Hour;
        this.Kind = Kind;
        this.Millisecond = Millisecond;
        this.Minute = Minute;
        this.Month = Month;
        this.Now = Now;
        this.UtcNow = UtcNow;
        this.Second = Second;
        this.Ticks = Ticks;
        this.TimeOfDay = TimeOfDay;
        this.Today = Today;
        this.Year = Year;
    }
}
if (typeof JsActions == 'undefined') {
    var JsActions = {};
    JsActions.WebApi = {};
}
JsActions.WebApi.Student = {
    GetStudentList: function (options) { ///<param name="options" type="ajaxSettings">[OPTIONAL] AjaxOptions partial object; it will be mergend with the one sent to .ajax jQuery function</param><returns type="jqXHR"/>
    },
    GetById: function (id, options) { ///<param name="id" type = "Int32"></param>
        ///<param name="options" type="ajaxSettings">[OPTIONAL] AjaxOptions partial object; it will be mergend with the one sent to .ajax jQuery function</param><returns type="jqXHR"/>
    },
    GetByName: function (name, options) { ///<param name="name" type = "String"></param>
        ///<param name="options" type="ajaxSettings">[OPTIONAL] AjaxOptions partial object; it will be mergend with the one sent to .ajax jQuery function</param><returns type="jqXHR"/>
    },
    GetBySurname: function (surname, options) { ///<param name="surname" type = "String"></param>
        ///<param name="options" type="ajaxSettings">[OPTIONAL] AjaxOptions partial object; it will be mergend with the one sent to .ajax jQuery function</param><returns type="jqXHR"/>
    },
    PostStudent: function (st, options) { ///<param name="st" type = "Student"></param>
        ///<param name="options" type="ajaxSettings">[OPTIONAL] AjaxOptions partial object; it will be mergend with the one sent to .ajax jQuery function</param><returns type="jqXHR"/>
    },
    DeleteStudent: function (id, options) { ///<param name="id" type = "Int32"></param>
        ///<param name="options" type="ajaxSettings">[OPTIONAL] AjaxOptions partial object; it will be mergend with the one sent to .ajax jQuery function</param><returns type="jqXHR"/>
    }
};
if (typeof Student == 'undefined') {
    function Student(id, Name, Surname, BirthDay, Exams) {
        this.id = id;
        this.Name = Name;
        this.Surname = Surname;
        this.BirthDay = BirthDay;
        this.Exams = Exams;
    }
}