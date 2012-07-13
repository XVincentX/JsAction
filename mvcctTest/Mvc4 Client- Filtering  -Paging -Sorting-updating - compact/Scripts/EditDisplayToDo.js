/// <reference path="MVCControlToolkit.JsQueryable-2.0.0.js" />

$(document).ready(function () {
    detailToDo.edit = function (item) {
        this.reset();
        mvcct.utils.restoreEntity(item, this, false);
        this.DetailOf(item);
        $("#detailForm").validate().form();
    };
    detailToDo.remove = function (item) {
        this.resetIfSelected(item);
        ClientToDoView.updater.deleted(ClientToDoView.DataPage.ToDoList, item);
    };
    detailToDo.undo = function (item) {
        this.resetIfSelected(item);
        ClientToDoView.updater.reset(item);
    };
    detailToDo.resetIfSelected = function (item) {
        if (item == this.DetailOf()) this.reset();
    };
    detailToDo.reset = function () {
        this.DueDate(new Date());
        this.Description("");
        this.Name("");
        this.DetailOf(null);
        ClientToDoView.updater.clearErrors($('#detailForm'));
    };
    detailToDo.save = function () {
        var item = this.DetailOf();
        if (!item) return;
        if (!$('#detailForm').validate().form()) return;
        mvcct.utils.restoreEntity(this, item);
        ClientToDoView.updater.modified(item, true, true);
        this.reset();
    };
    detailToDo.saveAsNew = function () {
        if (!$('#detailForm').validate().form()) return;
        var item = ko.mapping.fromJS({
            DueDate: this.DueDate(),
            Name: this.Name(),
            Description: this.Description(),
            id: null
        });
        ClientToDoView.updater.prepare(item, true);//newly created entity prepare it
        ClientToDoView.updater.inserted(ClientToDoView.DataPage.ToDoList, item);
        this.reset();
    };
    ClientToDoView.sumbitModifications = false;
    ClientToDoView.undoAll = function () {
        ClientToDoView.updater.resetAll($('#mainForm'));

    };
    ClientToDoView.save = function () {
        ClientToDoView.updater.update($('#mainForm'));
    };
});

