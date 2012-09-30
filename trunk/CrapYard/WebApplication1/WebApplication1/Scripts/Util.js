/// <reference path="jquery-1.7.1.js" />

$(document).ready(function () {
    var loaded = 0;
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
        $('<iframe />').attr('src', 'WebForm1.aspx').appendTo('body');
    });
    $(".subroutine").click(function () {
        if (loaded==0) {
            $(this).attr("disabled", true);
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

            //Create iframe with inner frame
            $('<iframe id="myframe" src="WebForm1.aspx"/>').appendTo('body');
            // possibly excessive use of jQuery - but I've got a live working example in production 
            $('#myframe').load(function () {
                alert('loading...');
                loaded++;
                //reload
                //i.e. use counter 
                if (loaded > 1) window.location.href = "PersistExpandedState20.aspx";
            });
        }
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
    $('.log').dblclick(function () {
        $(this).remove();
    });
    $("#msg").dblclick(function () {
        $("#msg").children('.log').remove();
    });

  
    $("#msg").append("<li class= 'log'>Doc is loaded.</li>");
});
function OnRequestStart(sender, args) {
    $("#msg").append("<li class= 'log'>OnRequestStart: " + args.get_eventTarget() + "</li>");
    //$("#msg").append("<li class= 'btnExpandColumn'>" + args.get_eventTarget() + "</li>");
    //$(".btnExpandColumn").hide();
    //$(".log").hide();

}

function OnResponseEnd(sender, args) {

    try {

        $("#msg").append("<li class= 'log'>OnResponseEnd (EventTarget):  " + args.EventTarget + ".</li>");
        $("#msg").append("<li class= 'log'>OnResponseEnd (EventArgument):  " + args.EventArgument + ".</li>");
        
       
        

    } catch (e) {
        ErrorHandler(e);
    }
    finally {
       
    }
    
}

function ErrorHandler(e) {
    if (e != null) {

        $("#msg").append("<li class= 'log'>ERROR:  " + e.description + ".</li>");
    }
    else {
        $("#msg").append("<li class= 'log'>ERROR:  " + e + ".</li>");
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

////////////////////////////////////////////////////////////////////////////////////
/*

Region Treeview draf and drop

*/
////////////////////////////////////////////////////////////////////////////////////

function onNodeDragging(sender, args) {
    var target = args.get_htmlElement();

    if (!target) return;

    if (target.tagName == "INPUT") {
        target.style.cursor = "hand";
    }
}

function dropOnHtmlElement(args) {
    if (droppedOnInput(args))
        return;

    return;
}

function droppedOnInput(args) {
    var target = args.get_htmlElement();
    if (target.tagName == "INPUT") {
        target.style.cursor = "default";
        target.value = args.get_sourceNode().get_text();
        args.set_cancel(true);
        return true;
    }
}

function dropOnTree(args) {
    var text = "";

    if (args.get_sourceNodes().length) {
        var i;
        for (i = 0; i < args.get_sourceNodes().length; i++) {
            var node = args.get_sourceNodes()[i];
            text = text + ', ' + node.get_text();
        }
    }
}

function clientSideEdit(sender, args) {
    var destinationNode = args.get_destNode();

    if (destinationNode) {
        var firstTreeView = $('.RadTreeView1');
        var secondTreeView = $('.RadTreeView2');

        firstTreeView.trackChanges();
        secondTreeView.trackChanges();
        var sourceNodes = args.get_sourceNodes();
        var dropPosition = args.get_dropPosition();

        //Needed to preserve the order of the dragged items
        if (dropPosition == "below") {
            for (var i = sourceNodes.length - 1; i >= 0; i--) {
                var sourceNode = sourceNodes[i];
                sourceNode.get_parent().get_nodes().remove(sourceNode);

                insertAfter(destinationNode, sourceNode);
            }
        }
        else {
            for (var i = 0; i < sourceNodes.length; i++) {
                var sourceNode = sourceNodes[i];
                sourceNode.get_parent().get_nodes().remove(sourceNode);

                if (dropPosition == "over")
                    destinationNode.get_nodes().add(sourceNode);
                if (dropPosition == "above")
                    insertBefore(destinationNode, sourceNode);
            }
        }
        destinationNode.set_expanded(true);
        firstTreeView.commitChanges();
        secondTreeView.commitChanges();
    }
}

function insertBefore(destinationNode, sourceNode) {
    var destinationParent = destinationNode.get_parent();
    var index = destinationParent.get_nodes().indexOf(destinationNode);
    destinationParent.get_nodes().insert(index, sourceNode);
}

function insertAfter(destinationNode, sourceNode) {
    var destinationParent = destinationNode.get_parent();
    var index = destinationParent.get_nodes().indexOf(destinationNode);
    destinationParent.get_nodes().insert(index + 1, sourceNode);
}

function onNodeDropping(sender, args) {
    var dest = args.get_destNode();
    if (dest) {
        var clientSide = true; //document.getElementById('ChbClientSide').checked;

        if (clientSide) {
            clientSideEdit(sender, args);
            args.set_cancel(true);
            return;
        }

        dropOnTree(args);
    }
    else {
        dropOnHtmlElement(args);
    }
}
////////////////////////////////////////////////////////////////////////////////////
/*

End Region Treeview draf and drop

*/
////////////////////////////////////////////////////////////////////////////////////