var _Path = location.host;
var urlEnviroment;

if (_Path.indexOf('localhost') >= 0 && _Path.indexOf(':') >= 0) {
    urlEnviroment = '';
}
else {
    var pathName = window.location.pathname;
    var virtualDir = pathName.split('/');
    urlEnviroment = virtualDir[1];
    urlEnviroment = '/' + urlEnviroment;
}

var _CallMethod = function (_Controller, _Method, _Parameters, _fnSucces) {
    var url = urlEnviroment + '/' + _Controller + '/' + _Method;
    url = url.replace('/undefined', '');
    $.ajax({
        url: url,
        data: _Parameters,
        cache: false,
        type: "POST",
        success: function (data) { _fnSucces(data); },
        error: function (xhr, ajaxOptions, thrownError) {
            alert('Method: ' + _Method + '; Status: ' + xhr.status + '; ThrowError: ' + thrownError);
        },
        //    beforeSend: function () { addLoader(); },
        //    complete: function () { removeLoader(); }
    });
};

var addLoader = function () {
    var circle1 = $('<div />').addClass('circle loader');
    var circle2 = $('<div />').addClass('circle r-loader');
    var imgDuti = $('<img />').prop('src', srcImage).prop('alt', '');
    var divLoader = $('<div />').attr('id', 'divLoader101880').addClass('backdrop').append(circle1).append(circle2);
    $($(document).find('body')).append(divLoader);
}

var removeLoader = function () { $('#divLoader101880').remove(); }

var _ConvertToDatatable = function (tableName, fileName, indexColumnSort) {
    var table = $('#' + tableName).addClass('table table-bordered table-hover table-striped w-100');
    var theadtr = table.find('thead tr').addClass('bg-primary-600');
    table.find('tbody tr').addClass('text-nowrap');
    theadtr.find('th').addClass('align-top');
    table.dataTable({
        bDestroy: true,
        lengthChange: true,
        responsive: true,
        searching: true,
        lengthMenu: [[5, 10, 25, 50, 100, 500, -1], [5, 10, 25, 50, 100, 500, "All"]],
        pageLength: 10,
        order: [[indexColumnSort, "desc"]],
        dom:
            "<'row mb-3'<'col-sm-12 col-md-6 d-flex align-items-center justify-content-start'f><'col-sm-12 col-md-6 d-flex align-items-center justify-content-end'B>>" +
            "<'row'<'col-sm-12'tr>>" +
            "<'row'<'col-md-3'i><'col-md-2'l><'col-md-6'p>>",
        oLanguage: { sProcessing: "<div id='loader'></div>" },
        responsive: true,
        buttons: [
            {
                extend: 'pdfHtml5',
                text: 'PDF',
                titleAttr: 'Generate PDF',
                className: 'btn-outline-danger btn-sm mr-1'
            },
            {
                extend: 'excelHtml5',
                text: 'Excel',
                titleAttr: 'Generate Excel',
                className: 'btn-outline-success btn-sm mr-1'
            },
            {
                extend: 'copyHtml5',
                text: 'Copy',
                titleAttr: 'Copy to clipboard',
                className: 'btn-outline-primary btn-sm mr-1'
            },
            {
                extend: 'print',
                text: 'Print',
                titleAttr: 'Print Table',
                className: 'btn-outline-primary btn-sm'
            }
        ],
        processing: true,
        fixedHeader: true,
        orderCellsTop: true
    });
}

var _ConvertToDatatableWithButtons = function (tableName, fileName, indexColumnSort) {
    var table = $('#' + tableName).addClass('table table-bordered table-hover table-striped w-100');
    var theadtr = table.find('thead tr').addClass('bg-primary-600');
    table.find('tbody tr').addClass('text-nowrap');
    table.find('tbody tr td:first-child').attr("style", "width:10px");
    theadtr.find('th').addClass('align-top');

    if (indexColumnSort == 0) indexColumnSort = 1;

    table.dataTable({
        bDestroy: true,
        lengthChange: true,
        responsive: true,
        searching: true,
        lengthMenu: [[5, 10, 25, 50, 100, 500, -1], [5, 10, 25, 50, 100, 500, "All"]],
        pageLength: 10,
        order: [[indexColumnSort, "desc"]],
        'columnDefs': [{
            "targets": 0,
            "orderable": false
        }],
        dom:
            "<'row mb-3'<'col-sm-12 col-md-6 d-flex align-items-center justify-content-start'f><'col-sm-12 col-md-6 d-flex align-items-center justify-content-end'B>>" +
            "<'row'<'col-sm-12'tr>>" +
            "<'row'<'col-md-3'i><'col-md-2'l><'col-md-6'p>>",
        oLanguage: { sProcessing: "<div id='loader'></div>" },
        responsive: true,
        buttons: [
            {
                extend: 'pdfHtml5',
                text: 'PDF',
                titleAttr: 'Generate PDF',
                className: 'btn-outline-danger btn-sm mr-1'
            },
            {
                extend: 'excelHtml5',
                text: 'Excel',
                titleAttr: 'Generate Excel',
                className: 'btn-outline-success btn-sm mr-1'
            },
            {
                extend: 'copyHtml5',
                text: 'Copy',
                titleAttr: 'Copy to clipboard',
                className: 'btn-outline-primary btn-sm mr-1'
            },
            {
                extend: 'print',
                text: 'Print',
                titleAttr: 'Print Table',
                className: 'btn-outline-primary btn-sm'
            }
        ],
        processing: true,
        fixedHeader: true,
        orderCellsTop: true
    });
}

var _AlertSideXC = function (message, color) {
    var icon = color == "success" ? "fa fa-check-circle" : "fa fa-exclamation-circle";
    bootbox.alert({
        title: "<span class='" + icon + " text-" + color + " mr-2'></span> <span class='text-" + color + " fw-500'>SideXC Alerts</span>",
        message: "<span>" + message + "</span>",
        centerVertical: true,
        className: "modal-alert",
        closeButton: false
    });

}