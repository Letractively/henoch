
/// <reference path="jquery-1.4.1-vsdoc.js" />


/// <reference path="jquery-1.4.4.min.js" />

$(document).ready(function () {
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

    $("#msg").ajaxError(function (event, request, settings) {
        $(this).append("<li class= 'log'>Error requesting page " + settings.url + "</li>");
        $(this).append("<li class= 'log'>" + request.responseText + "</li>");
        //alert('Ajax error!');
    });
    /***********************************************************************/

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

    $('.log').dblclick(function () {
        $(this).remove();
    });
    $("#msg").dblclick(function () {
        $("#msg").children('.log').remove();
    });

    $(".error").click(function () {
        $.ajax({
            type: "POST",
            dataType: "json",
            contentType: "application/json",
            url: "../MyService.asmx/ErrorFunction",
            data: "",
            success: function (data) {
                alert("Message: " + data.d);
            },
            error: function () {
                alert("Error calling the web service.");
            }
        });
    }); 

    $(".search").click(function () {
        var empId = 1;
        $.ajax({
            type: "POST",
            dataType: "json",
            contentType: "application/json",
            url: "../MyService.asmx/GetEmployee",
            data: "{'employeeId': '" + empId + "'}",
            success: function (data) {
                alert("Employee name: " + data.d);
            },
            error: function () {
                alert("Error calling the web service.");
            }
        });
        //Create iframe
    });
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

function OnRequestStart(sender, args) {
    //$("#msg").append("<li class= 'log'>OnRequestStart: " + args.get_eventTarget() + "</li>");
    //$("#msg").append("<li class= 'btnExpandColumn'>" + args.get_eventTarget() + "</li>");
    //$(".btnExpandColumn").hide();
    //$(".log").hide();

}

function OnResponseEnd(sender, args) {

    try {
        //clear all log messages.
        //$(".log").remove();
        var children = $("#msg").children('.rgDetailTable');
        //children.remove();

        $("#msg").append("<li class= 'log'>OnResponseEnd (EventTarget):  " + args.EventTarget + ".</li>");
        $("#msg").append("<li class= 'log'>OnResponseEnd (EventArgument):  " + args.EventArgument + ".</li>");

    } catch (e) {
        if (e != null) {

            $("#msg").append("<li class= 'log'>ERROR:  " + e.description + ".</li>");
        }
        else {
            $("#msg").append("<li class= 'log'>ERROR:  " + e + ".</li>");
        }

    }
    finally {

    }

}