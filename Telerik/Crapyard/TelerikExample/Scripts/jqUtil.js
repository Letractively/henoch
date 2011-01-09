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
        $(this).append("<li>Error requesting page " + settings.url + "</li>");
    });

   

    $("#msg").ajaxComplete(function (request, settings) {
        $(this).append("<li>Request Complete.</li>");
        alert('request completed');
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
        $(".log").remove();
        $("#msg").append("<li class= 'log'>OnResponseEnd (EventTarget):  " + args.EventTarget + ".</li>");
        $("#msg").append("<li class= 'log'>OnResponseEnd (EventArgument):  " + args.EventArgument + ".</li>");
        $("#msg").append("<li class= 'log'>OnResponseEnd (EventTargetElement):  " + args.EventTargetElement + ".</li>");

        var str = args.EventTarget.toString();
        var detailsTable = $(args.EventTarget)// $('#RadGrid1_ctl00_ctl09_Detail20_ctl06_Detail10_ctl04_GECBtnExpandColumn')
            .closest('table'); //$("#RadGrid1_ctl00_ctl09_Detail20");

        if (detailsTable != null && detailsTable.length == 1) {
            //var match = /s(amp)le/str.exec("Sample text");
            var re = new RegExp(window.prompt("Please input a regex.", "/^RadGrid1(\$(\w)+)+/gi"), "g");
            var result = re.exec(str);

            var target = $(".log").parents("#msg");
            var cloned = detailsTable.clone(true);
            cloned.insertAfter(target);
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
