/// <reference path="jquery-1.4.4.min.js" />
$(document).ready(function () {
    $.ajaxSetup({
        error: function (x, e) {
            if (x.status == 0) {
                alert('You are offline!!\n Please Check Your Network.');
            } else if (x.status == 4014) {
                //alert('Requested URL not found.');
                $("#msg").ajaxComplete(function (request, settings) {
                    $(this).append("<li>Requested URL not found.</li>");
                });
            } else if (x.status == 500) {
                alert('Internel Server Error.');
            } else if (e == 'parsererror') {
                alert('Error.\nParsing JSON Request failed.');
            } else if (e == 'timeout') {
                alert('Request Time out.');
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

    $("#msg").ajaxStart(function () {
        $(this).append("<li class= 'log'>Ajax request beginning.</li>");
    });

    $("#msg").ajaxComplete(function (request, settings) {
        $(this).append("<li class= 'log'>Ajax request completed.</li>");
    });

    $(".search").click(function () {
        var empId = 1;
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
        //Create iframe
        $('<iframe id="myframe1"/>').attr('src', 'WebForm1.aspx').appendTo('body');
    });
    $(".subroutine").click(function () {
        $.ajax({
            type: "POST",
            dataType: "json",
            contentType: "application/json",
            url: "MyService.asmx/HelloWorld",
            data: "",
            success: function (data) {
                alert("Message: " + data.d);
            },
            error: function () {
                alert("Error calling the web service.");
            }
        });

        //Create iframe
        $('<iframe id="myframe2"/>').attr('src', 'WebForm1.aspx').appendTo('body');
        // possibly excessive use of jQuery - but I've got a live working example in production 
        $('#myframe2').load(function () {
            if (typeof callback == 'function') {
                callback($('body', this.contentWindow.document).html());
            }
            setTimeout(function () { $('#myframe2').remove(); }, 50);

            //reload
            location.reload();
         }); 
    });

    $(".error").click(function () {
        $.ajax({
            type: "POST",
            dataType: "json",
            contentType: "application/json",
            url: "MyService.asmx/ErrorFunction",
            data: "",
            success: function (data) {
                alert("Message: " + data.d);
            },
            error: function () {
                alert("Error calling the web service.");
            }
        });
    });

    $("#msg").dblclick(function () {
        $("#msg").children('.log').remove(); 
    });
});
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
        
        
        var detailsTable = $(args.EventTargetElement)// $('#RadGrid1_ctl00_ctl09_Detail20_ctl06_Detail10_ctl04_GECBtnExpandColumn')
            .closest('.rgDetailTable'); //$("#RadGrid1_ctl00_ctl09_Detail20");

        if (detailsTable != null && detailsTable.length == 1) {
            var str = args.EventTargetElement.id.toString();
            $("#msg").append("<li class= 'log'>OnResponseEnd (EventTargetElement):  " + str + ".</li>");

            var re = new RegExp(window.prompt("Please input a regex.", 
                "^RadGrid1(([_][A-Za-z_0-9]*)*([_]Detail)+([_][A-Za-z_0-9]*)*)+"), "gi");
            var result = re.exec(str);

            if (result != null) {
                var target = $(".log:last-child");
                var cloned = detailsTable.clone(true);              
                //cloned.insertAfter(target);
            }
        }
        

    } catch (e) {
        if (e!=null) {

            $("#msg").append("<li class= 'log'>ERROR:  " + e.description + ".</li>");
        }
        else {
            $("#msg").append("<li class= 'log'>ERROR:  " + e + ".</li>");
        }
        
    }
    finally {
       
    }
    
}

function RowSelecting(sender, eventArgs) {
    //$("#msg").append("<li>RowSelecting:  " + eventArgs.get_itemIndexHierarchical() + ".</li>");
    //alert("Selecting row: " + eventArgs.get_itemIndexHierarchical());
}

function RowClick(sender, eventArgs) {
    //IKEA 116020
    __doPostBack('ctl00$ContentPlaceHolderMain$gvMoveOverview$ctl00$ctl07$GECBtnExpandColumn', '');
    
    //$("#msg").append("<li>RowClick:  " + eventArgs.get_itemIndexHierarchical() + ".</li>");
    //alert("Click on row instance: " + eventArgs.get_itemIndexHierarchical());
}

function SelectedIndexChanged(sender, eventArgs) {

    $("#msg").append("<li>SelectedIndexChanged:  " + eventArgs.get_itemIndexHierarchical() + ".</li>");
}
function ColumnClick(sender, eventArgs) {

    $("#msg").append("<li>ColumnClick:  " +  eventArgs.get_gridColumn().get_element().cellIndex + ".</li>");
    //alert("Click on column-header: " + eventArgs.get_gridColumn().get_element().cellIndex);
}



function HierarchyCollapsing(sender, eventArgs) {
    $("#msg").append("<li>HierarchyCollapsing:  " + eventArgs.get_itemIndexHierarchical() + ".</li>");
    //alert("Row: " + eventArgs.get_itemIndexHierarchical() + " is being collapsed");
}


function GroupExpanding(sender, eventArgs) {

    alert("GroupExpanding is fired");

}


function GetFirstDataItemKeyValues() {
    var firstDataItem = $find("<%=RadGrid1.ClientID %>").get_masterTableView().get_dataItems()[0];
    var keyValues =
     'CustomerID: "' +
      firstDataItem.getDataKeyValue("CustomerID") + '"' +
      ' \r\n' +
      'CompanyName: "' +
      firstDataItem.getDataKeyValue("CompanyName") + '"';
    alert(keyValues);    
}
