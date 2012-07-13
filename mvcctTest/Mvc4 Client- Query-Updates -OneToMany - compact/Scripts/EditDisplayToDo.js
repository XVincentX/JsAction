/// <reference path="MVCControlToolkit.JsQueryable-2.1.0.js" />

var DestinationViewModel = {};
$(document).ready(function () {
    ClientToDoView.childUpdater = ClientToDoView.updater.addChildUpdateManager({ expression: 'Tasks', external: 'FatherId' }, 'Key', "TaskCS");
    ClientToDoView.childUpdater.options({
        isoDate: true,
        prepareCallback: function (item) {
            var prev = false;
            function subscription() {
                ClientToDoView.childUpdater.modified(item, true, true);
                if (prev && !item._modified())
                    detailToDo.undoTask(item);
                prev = item._modified();
            };
            item.Name.subscribe(subscription);
            item.WorkingDays.subscribe(subscription)
        }
    });
    ClientToDoView.updater.options({
        isoDate: true,
        updateCallback: function (e, result, status) { alert("status: " + status); },
        prepareCallback: function (item) { item.tasksChanged = ClientToDoView.updater.arrayChanged(item.Tasks); }
    });
    

    detailToDo.Tasks = ko.observableArray([]);
    detailToDo.edit = function (item) {
        if (this._save()) {
            mvcct.utils.restoreEntity(item, this, true);
            this.DetailOf(item);
            ClientToDoView.updater.refreshErrors($("#detailForm"));
        }
    };
    detailToDo.remove = function (item) {
        this.resetIfSelected(item);
        ClientToDoView.updater.deleted(ClientToDoView.DataPage.ToDoList, item);
    };
    detailToDo.removeTask = function (item) {
        ClientToDoView.updater.deleted(detailToDo.Tasks, item);
    };
    detailToDo.undo = function (item) {
        this.resetIfSelected(item);
        ClientToDoView.updater.reset(item, $('#mainForm'));
    };
    detailToDo.undoTask = function (item) {
        ClientToDoView.childUpdater.reset(item, $('#mainForm'), this.Tasks);
        ClientToDoView.childUpdater.refreshErrors($('#detailForm'));
    };
    detailToDo.resetIfSelected = function (item) {
        if (item == this.DetailOf()) this.reset();
    };
    detailToDo.reset = function () {
        this.DueDate(new Date());
        this.Description("");
        this.Name("");
        this.DetailOf(null);
        this.Tasks([]);
        ClientToDoView.updater.clearErrors($('#detailForm'));
    };
    detailToDo.save = function () {
        var res = this._save();
        if (!res) return false;
        this.reset();
        return true;
    };
    detailToDo._save = function () {
        var item = this.DetailOf();
        if (!item) return true;
        if (!$('#detailForm').validate().form()) return false;
        mvcct.utils.restoreEntity(this, item, true);
        ClientToDoView.updater.modified(item, true, true);
        if ((!item._modified()) && (!item.tasksChanged()))
            ClientToDoView.childUpdater.refreshErrors($('#mainForm'), null, item);
        return true;
    };
    detailToDo.saveAsNew = function () {
        if (!$('#detailForm').validate().form()) return;
        var item = {
            DueDate: ko.observable(this.DueDate()),
            Name: ko.observable(this.Name()),
            Description: ko.observable(this.Description()),
            id: ko.observable(null),
            Tasks: ko.observableArray(this.Tasks())
        };
        ClientToDoView.updater.prepare(item, true); //newly created entity prepare it
        ClientToDoView.updater.inserted(ClientToDoView.DataPage.ToDoList, item);
        this.DetailOf(item);
    };
    detailToDo.createTask = function () {
        var item = {
            Name: ko.observable(''),
            WorkingDays: ko.observable(0),
            Key: ko.observable(null),
            FatherId: ko.observable(null)
        };
        //newly created entity preparation is done when adding to father with addChild
        ClientToDoView.updater.addChild(this, 'Tasks', item, true);
    };
    ClientToDoView.sumbitModifications = false;

    ClientToDoView.undoAll = function () {
        ClientToDoView.updater.resetAll($('#mainForm'));

    };
    ClientToDoView.save = function () {
        if (!detailToDo.save()) return;
        ClientToDoView.updater.update($('#mainForm'));
    };
});

