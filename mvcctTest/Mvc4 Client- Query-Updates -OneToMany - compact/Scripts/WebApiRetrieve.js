

$(document).ready(function () {
    if (!window['ClientToDoView']) return;
    var pageSize = 4;
    ClientToDoView.Refresh = function (type, page, sorting, prevPage) {
        if (window['detailToDo']) detailToDo.save();
        if (!type) {
            type = 'filter'; page = 1;
            ClientToDoView.DataPage.CurrPage(1);
            //filter changed, we need reset pager to the first page
        }
        query.setPaging(page, pageSize);
        //sorting changed 
        //we don't need to reset pager to the first page, sorting control do it automatically
        if (type == 'sort') query.importSorting(sorting);
        else if (type == 'filter') {
            query.resetFilter()
            .importClauses('ToDoFilter');
        }
        if (ClientToDoView['updater']) {//clear previous errors, since entities shown on the screen will change
            ClientToDoView.updater.clearErrors($('#mainForm'));
        }
        query.execute(function (x) {
            if (!x) x = { results: [], __count: 0 };
            else if (mvcct.utils.isArray(x)) x = { results: x, __count: (page + 1) * pageSize}//no count infos
            if (x.results.length == 0) {
                if (prevPage) { //pages are finished! remain on previous page and update total pages count
                    ClientToDoView.DataPage.CurrPage(prevPage);
                    ClientToDoView.DataPage.TotalPages(page - 1);
                    return;
                }
                else {
                    x.__count = 0; //no result update total pages
                }
            }
            var newEntities = ko.mapping.fromJS(x.results)();
            if (ClientToDoView['updater']) {//if entities may be modified and sent back to the server, prepare them
                ClientToDoView.updater.prepare(newEntities, true);
            }
            ClientToDoView.DataPage.ToDoList(newEntities);
            var totPages = x.__count / pageSize;
            if (x.__count % pageSize) totPages++;
            ClientToDoView.DataPage.TotalPages(totPages);

        },
        function (x) {

            var exception = $.parseJSON(x.responseText);
            var message = mvcct.utils.isString(exception) ? exception : (exception.Message || "Internal Server Error");
            alert("status code: " + x.status + "; " + message);
        }
        )


    }


    $('#root').bind('queryChanged', function (e, data) {
        ClientToDoView.Refresh(data.type, data.page, data['sortString'], data['previousPage']);
    });
    //populate initial results
    ClientToDoView.DataPage.CurrPage(1);
    ClientToDoView.Refresh('page', 1);
});