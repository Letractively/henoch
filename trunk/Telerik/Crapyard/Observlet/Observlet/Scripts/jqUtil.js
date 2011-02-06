
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

    /*
    NUMERICTEXT BOX
    */
    $(".numericCssClass").keypress(function (event) {
        //$("#msg").append("<li class= 'log'>EventTarget:  " + event.target.value + ".</li>");

        var key = String.fromCharCode(event.keyCode);

        if (isNaN(key) && key != '.') {
            //prevents from showing NaN except periods and numbers
            event.preventDefault();
        }
//        if (key == '.') {
//            var decimalseparators = event.target.value.toString().split(".", 4);
//            if (decimalseparators.length > 1) {
//                var inputField = '';
//                try {
//                    inputField = $(this)[0].value.toString().replace(".", "");
//                    $(this)[0].value = inputField;
//                    //event.preventDefault();
//                } catch (e) {
//                    //ignore
//                }
//            }
//        }
    });

    try {
        $(".numericCssClass").focusout(function (event) {
            var eventTarget = parseFloat(event.target.value);

            if (!isNaN(eventTarget)) {
                $(this).valueOf().get(0).value = eventTarget.toFixed(2);
            }
        });
    } catch (e) {
        if (e != null) {

            //$("#msg").append("<li class= 'log'>ERROR in focusout " + e.description + ".</li>");
        }
        else {
            //$("#msg").append("<li class= 'log'>ERROR in focusout:  " + e + ".</li>");
        }
    }
    $(".numericCssClass").keyup(function (event) {
        //$("#msg").append("<li class= 'log'>EventTarget:  " + event.target.value + ".</li>");
        regex = /\d*(\.?)(\d{0,2})/g;
        result = regex.exec(event.target.value);

        var eventTarget = parseFloat(event.target.value);
        if (!isNaN(eventTarget)) {

            regex = /^[0]+$/g;
            zeros = regex.exec(event.target.value);
            if (zeros != null && zeros[0].length == zeros.lastIndex && zeros[0].length > 1) {
                //assert there are only zero's.
                $(this).valueOf().get(0).value = 0;
                //assert there is at most 1 period left in string.
                return;
            }

            //todo remove leftmost zeros(on lostfocus?).                
        }
        if (result != null && $(this).valueOf().get(0).value != result[0]) {
            event.preventDefault();
            //check for periods and use only leftmost period (.
            var numbers = event.target.value.toString().split(".", 4);

            //find first occurence of a floating point. ( 0.23345.67 => 0.23345)
            regex = /\d*(\.?)(\d{0,3})/g;
            var result1 = regex.exec(event.target.value);

            var numeric = 0;
            if (result1 != null && result1.length > 2)
                numeric = parseFloat(result1[0] + numbers[2]); //(0.2334567 )
            else
                numeric = parseFloat(event.target.value);

            if (isNaN(numeric)) numeric = 0;
            $(this).valueOf().get(0).value = numeric.toFixed(2);
        }

        //assert there is at most1 period left in string.
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