if (typeof JsActions == 'undefined') {
    var JsActions = {};
    JsActions.WebApi = {};
}
JsActions.Home = {
    Index: function (options) { ///<param name="options" type="ajaxSettings">[OPTIONAL] AjaxOptions partial object; it will be mergend with the one sent to .ajax jQuery function</param><returns type="jqXHR"/>
    },
    oas: function (options) { ///<param name="options" type="ajaxSettings">[OPTIONAL] AjaxOptions partial object; it will be mergend with the one sent to .ajax jQuery function</param><returns type="jqXHR"/>
    },
    Index: function (options) { ///<param name="options" type="ajaxSettings">[OPTIONAL] AjaxOptions partial object; it will be mergend with the one sent to .ajax jQuery function</param><returns type="jqXHR"/>
    },
    oas: function (options) { ///<param name="options" type="ajaxSettings">[OPTIONAL] AjaxOptions partial object; it will be mergend with the one sent to .ajax jQuery function</param><returns type="jqXHR"/>
    }
};
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
    },
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