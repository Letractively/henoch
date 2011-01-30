
/// <reference path="jquery-1.4.1-vsdoc.js" />


/// <reference path="jquery-1.4.4.min.js" />

$(document).ready(function () {

    $('.async').click(function () {
        $(this).attr('disabled', true);
        $('.interactasync').attr('disabled', false);
        //setTimeout('location.href = document.location', 3000)
        __doPostBack('Button1', '');

    });
    $('.interactasync').click(function () {
        $(this).attr('disabled', true);
        $('.async').attr('disabled', false);
        __doPostBack('Button2', '');
    });

    /********************************************************************
    Ajax setup
    *********************************************************************/
    $.ajaxSetup({
        error: function (x, e) {
            if (x.status == 0) {
                $("#msg").append("<li class= 'log'>You are offline!!\n Please Check Your Network.</li>");
            } else if (x.status == 404) {
                //alert('Requested URL not found.');
                $("#msg").ajaxComplete(function (request, settings) {
                    $(this).append("<li>Requested URL not found.</li>");
                });
            } else if (x.status == 500) {
                $("#msg").append("<li class= 'log'>Internel Server Error.</li>");
            } else if (e == 'parsererror') {
                $("#msg").append("<li class= 'log'>Parsing JSON Request failed.</li>");
            } else if (e == 'timeout') {
                $("#msg").append("<li class= 'log'>Request Time out.</li>");
            } else {
                $("#msg").ajaxComplete(function (request, settings) {
                    $(this).append("<li>Unknow Error:\n" + x.responseText + "</li>");
                });
            }
        }
    });
    /***********************************************************************/
});

function StatusReport() {
    $.ajax({
        type: "POST",
        dataType: "json",
        contentType: "application/json",
        url: "MyService.asmx/GetEmployee",
        data: "{'employeeId': '" + empId + "'}",
        success: function (data) {
            alert("Employee name: " + data.d);
        },
        error: function () {
            alert("Error calling the web service.");
        }
    });
}