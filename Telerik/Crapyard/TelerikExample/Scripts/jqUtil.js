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

    $("#msg").append("<li>Doc is Ready.</li>");



});
function OnRequestStart(sender, args) {
    //$("#msg").append("<li class= 'log'>OnRequestStart: " + args.get_eventTarget() + "</li>");
    $("#msg").append("<li class= 'btnExpandColumn'>" + args.get_eventTarget() + "</li>");
    //$(".btnExpandColumn").hide();
    //$(".log").hide();

}

function OnResponseEnd(sender, args) {
    $(".btnExpandColumn:contains('ExpandColumn')").filter(".btnExpandColumn:contains('Detail')").each(function (index) {
                        
            //var parent = $(document).find("'#" + $(this).text() + "'").closest('rgDetailTable');

            //var cloned = parent.clone(true);

            $('#msg').append("<li  class= 'log'>cloning element: " + $(this).text() + ".</li>");

        });
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
