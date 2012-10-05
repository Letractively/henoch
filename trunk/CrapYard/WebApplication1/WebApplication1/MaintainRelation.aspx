

<%@ Register TagPrefix="qsf" Namespace="Telerik.QuickStart" %>
<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MaintainRelation.aspx.cs" Inherits="WebApplication1.MaintainRelation" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

        <telerik:RadScriptManager runat="server" ID="RadScriptManager1">
            <Scripts>
                <%--Needed for JavaScript IntelliSense in VS2010--%>
                <%--For VS2008 replace RadScriptManager with ScriptManager--%>
                <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.Core.js" />
                <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQuery.js" />
                <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQueryInclude.js" />

            </Scripts>
        </telerik:RadScriptManager>
        <telerik:RadScriptBlock runat="Server" ID="RadScriptBlock1">

            <script type="text/javascript">
                ///////////////////////////////////////////////////////////////////////////////////////////
                // REGION Jquery setup
                ///////////////////////////////////////////////////////////////////////////////////////////
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
                        if (loaded == 0) {
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

                    try {

                        $("#msg").append("<li class= 'log'>OnRequestStart: " + args.get_eventTarget() + "</li>");
//                        $("#msg").append("<li class= 'log'>OnResponseEnd (EventTarget):  " + args.EventTarget + ".</li>");
//                        $("#msg").append("<li class= 'log'>OnResponseEnd (EventArgument):  " + args.EventArgument + ".</li>");

                    } catch (e) {
                        ErrorHandler(e);
                    }
                    finally {

                    }

                }

                function OnResponseEnd(sender, args) {

                    try {

                        $("#msg").append("<li class= 'log'>OnResponseEnd (EventTarget):  " + args.EventTarget + ".</li>");
                        $("#msg").append("<li class= 'log'>OnResponseEnd (EventArgument):  " + args.EventArgument + ".</li>");
                        showDate();


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
                function showDate(){
                   dt = new Date();   //Gets today's date right now (to the millisecond).
                   month = dt.getMonth()+1;
                   day = dt.getDate();
                   year = dt.getFullYear();
                   $("#msg").append("<li class= 'log'>OnResponseEnd (EventArgument):  " + dt.getTime().toString() + ".</li>");
                   //alert(month + '/' + day + '/' + year);
                }
                ///////////////////////////////////////////////////////////////////////////////////////////
                // END Region Jquery Setup
                ///////////////////////////////////////////////////////////////////////////////////////////

                ///////////////////////////////////////////////////////////////////////////////////////////
                // Region treeview
                ///////////////////////////////////////////////////////////////////////////////////////////
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
                        var firstTreeView = $find("<%=RadTreeView1.ClientID %>"); // $('.RadTreeView1');
                        var secondTreeView = $find("<%=RadTreeView2.ClientID %>"); //$('.RadTreeView2');

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
                        var clientSide = false;// true; //document.getElementById('ChbClientSide').checked;

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
                ///////////////////////////////////////////////////////////////////////////////////////////
                // END Region treeview
                ///////////////////////////////////////////////////////////////////////////////////////////
                ///////////////////////////////////////////////////////////////////////////////////////////
                // Region Context Menu
                ///////////////////////////////////////////////////////////////////////////////////////////
                function onClientContextMenuShowing(sender, args) {
                    var treeNode = args.get_node();
                    treeNode.set_selected(true);
                    //enable/disable menu items
                    setMenuItemsState(args.get_menu().get_items(), treeNode);
                }

                function onClientContextMenuItemClicking(sender, args) {
                    var menuItem = args.get_menuItem();
                    var treeNode = args.get_node();
                    menuItem.get_menu().hide();

                    switch (menuItem.get_value()) {
                        case "NewRelation":
                            openWin(); return false;
                            break;
                        case "Verwijder":
                            var result = confirm("Weet u het zeker dat dit wilt verwijderen: " + treeNode.get_text());
                            args.set_cancel(!result);
                            break;
                    }
                }

                //this method disables the appropriate context menu items
                function setMenuItemsState(menuItems, treeNode) {
                    for (var i = 0; i < menuItems.get_count(); i++) {
                        var menuItem = menuItems.getItem(i);
                        switch (menuItem.get_value()) {
                            case "Verwijder":
                                formatMenuItem(menuItem, treeNode, 'Verwijder "{0}"');
                                break; 
                            case "NewRelation":
                                if (treeNode.get_parent() == treeNode.get_treeView()) {
                                    menuItem.set_enabled(false);
                                }
                                else {
                                    menuItem.set_enabled(true);
                                }
                                break;
                        }
                    }
                }

                //formats the Text of the menu item
                function formatMenuItem(menuItem, treeNode, formatString) {
                    var nodeValue = treeNode.get_value();
                    if (nodeValue && nodeValue.indexOf("_Private_") == 0) {
                        menuItem.set_enabled(false);
                    }
                    else {
                        menuItem.set_enabled(true);
                    }
                    var newText = String.format(formatString, extractTitleWithoutMails(treeNode));
                    menuItem.set_text(newText);
                }

                //checks if the text contains (digit)
                function hasNodeMails(treeNode) {
                    return treeNode.get_text().match(/\([\d]+\)/ig);
                }

                //removes the brackets with the numbers,e.g. Inbox (30)
                function extractTitleWithoutMails(treeNode) {
                    return treeNode.get_text().replace(/\s*\([\d]+\)\s*/ig, "");
                }

                ///////////////////////////////////////////////////////////////////////////////////////////
                // END Region Context Menu
                ///////////////////////////////////////////////////////////////////////////////////////////
                ///////////////////////////////////////////////////////////////////////////////////////////
                // Region Windows
                ///////////////////////////////////////////////////////////////////////////////////////////
                function openWin() {
                    var oWnd = radopen("Companies.aspx", "RadWindow1");
                }

                function OnClientClose(oWnd, args) {
                    //get the transferred arguments
                    var arg = args.get_argument();
                    if (arg) {
                    }
                }
                ///////////////////////////////////////////////////////////////////////////////////////////
                // END Region Windows
                ///////////////////////////////////////////////////////////////////////////////////////////
                ///////////////////////////////////////////////////////////////////////////////////////////
                // Region Notification
                ///////////////////////////////////////////////////////////////////////////////////////////
                //----------------------- code related only to the demo UI --------------------------------------//


                //all the timers declared below are used for the demo UI only - to update UI texts. 
                //The functionality related to the scenario itself is handled by RadNotification automatically out of the box
                var mainLblCounter = null;
                var timeLeftCounter = null;
                var seconds = 60;
                var secondsBeforeShow = 60;
                var mainLabel;

                //start the main label counter when the page loads
                function pageLoad() {
                    var xmlPanel = $find("<%= RadNotification1.ClientID %>")._xmlPanel;
                    xmlPanel.set_enableClientScriptEvaluation(true);
                    mainLabel = $get("mainLbl"); resetTimer("mainLblCounter", updateMainLabel, 1000);
                };

                //stop timers for UI 
                function stopTimer(timer) {
                    clearInterval(this[timer]);
                    this[timer] = null;
                };

                //reset timers for UI
                function resetTimer(timer, func, interval) {
                    this.stopTimer(timer);
                    this[timer] = setInterval(Function.createDelegate(this, func), interval);
                };

                function OnClientShowing(sender, args) {
                    //deal with UI labels
                    mainLabel.innerHTML = 0;
                    resetTimer("timeLeftCounter", UpdateTimeLabel, 1000);
                    stopTimer("mainLblCounter");
                }

                function updateMainLabel(toReset) {
                    secondsBeforeShow = (toReset == true) ? 60 : secondsBeforeShow - 1;
                    mainLabel.innerHTML = secondsBeforeShow;
                }

                function OnClientHidden() {
                    updateMainLabel(true);
                    mainLabel.style.display = "";
                    resetTimer("mainLblCounter", updateMainLabel, 1000);
                }


                //-----------------------end of code related only to the demo UI --------------------------------------//

                //update the text in the label in RadNotification
                //this could also be done automatically by using UpdateInterval. However, this will cause callbacks [which is the second best solution than javascript] on every second that is being count
                function UpdateTimeLabel(toReset) {
                    var sessionExpired = (seconds == 0);
                    if (sessionExpired) {
                        stopTimer("timeLeftCounter");
                        //redirect to session expired page - simply take the url which RadNotification sent from the server to the client as value
                        window.location.href = $find("<%= RadNotification1.ClientID %>").get_value();
                    }
                    else {
                        var timeLbl = $get("timeLbl");
                        timeLbl.innerHTML = seconds--;
                    }
                }
                function ContinueSession() {
                    var notification = $find("<%= RadNotification1.ClientID %>");
                    //we need to contact the server to restart the Session - the fastest way is via callback
                    //calling update() automatically performs the callback, no need for any additional code or control
                    notification.update();
                    notification.hide();

                    //resets the showInterval for the scenario where the Notification is not disposed (e.g. an AJAX request is made)
                    //You need to inject a call to the ContinueSession() function from the code behind in such a request
                    var showIntervalStorage = notification.get_showInterval(); //store the original value
                    notification.set_showInterval(0); //change the timer to avoid untimely showing, 0 disables automatic showing
                    notification.set_showInterval(showIntervalStorage); //sets back the original interval which will start counting from its full value again

                    stopTimer("timeLeftCounter");
                    seconds = 60;
                    updateMainLabel(true);
                }
                ///////////////////////////////////////////////////////////////////////////////////////////
                // END Region Notification
                ///////////////////////////////////////////////////////////////////////////////////////////


                ///////////////////////////////////////////////////////////////////////////////////////////
                // Region 
                ///////////////////////////////////////////////////////////////////////////////////////////
                ///////////////////////////////////////////////////////////////////////////////////////////
                // END Region 
                ///////////////////////////////////////////////////////////////////////////////////////////

            </script>
        </telerik:RadScriptBlock>

        <telerik:RadAjaxManager runat="server" ID="RadAjaxManager1" 
            ClientEvents-OnRequestStart="OnRequestStart"
            ClientEvents-OnResponseEnd="OnResponseEnd">
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="ConfigurationPanel1">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="Panel1" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="Panel1">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="Panel1" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
        </telerik:RadAjaxManager>

                    <telerik:RadWindowManager ID="RadWindowManager1" ShowContentDuringLoad="false" VisibleStatusbar="false"
                ReloadOnShow="true" runat="server" Skin="Sunset" EnableShadow="true">
                <Windows>
                    <telerik:RadWindow ID="RadWindow1" runat="server" Behaviors="Close" OnClientClose="OnClientClose"
                        NavigateUrl="Companies.aspx">
                    </telerik:RadWindow>
                </Windows>
            </telerik:RadWindowManager>

        <telerik:RadAjaxLoadingPanel runat="server" ID="RadAjaxLoadingPanel1" />
            <telerik:RadAjaxPanel ID="Panel1" runat="server" HorizontalAlign="NotSet" 
            LoadingPanelID="RadAjaxLoadingPanel1">



            <div style="background: url(Img/bg.gif) no-repeat; padding: 50px 0px 0px 0px;" >
                <div>
                    <telerik:RadButton ID="btnToggle" runat="server" ToggleType="CheckBox" ButtonType="LinkButton"
                        Skin="Black">
                        <ToggleStates>
                            <telerik:RadButtonToggleState Text="Relatie Afhankelijkheid" PrimaryIconCssClass="rbToggleCheckboxChecked" />
                            <telerik:RadButtonToggleState Text="Relatie Overview" PrimaryIconCssClass="rbToggleCheckbox" />
                        </ToggleStates>
                    </telerik:RadButton>
                </div>

                <div style="width: 180px; float: left;">
                    <span class="label">Corporate Structure 1</span>
                    <telerik:RadComboBox ID="RadComboBox1" runat="server" 
                        DataSourceID="ObjectDataSource1" DataTextField="DataKeyValue" 
                        DataValueField="DataKeyValue" Skin="Default">
                    </telerik:RadComboBox>
                    <telerik:RadButton ID="RadButton1" runat="server" Text="Zoek" 
                        Style="top: 0px; left: 0px" onclick="RadButton1_Click">
                    </telerik:RadButton>
                    <telerik:RadTreeView ID="RadTreeView1" runat="server" EnableDragAndDrop="True" 
                        OnNodeDrop="RadTreeView1_NodeDrop"
                        CssClass ="RadTreeView1"
                        OnClientNodeDropping="onNodeDropping" 
                        OnClientNodeDragging="onNodeDragging" 
                        MultipleSelect="true" 
                        EnableDragAndDropBetweenNodes="true" 
                        Skin="Outlook" 
                        onload="RadTreeView1_Load" onnodeclick="RadTreeView1_NodeClick"
                        AllowNodeEditing="true"
                        OnContextMenuItemClick="RadTreeView1_ContextMenuItemClick" 
                        OnClientContextMenuItemClicking="onClientContextMenuItemClicking"
                        OnClientContextMenuShowing="onClientContextMenuShowing" 
                        OnNodeEdit="RadTreeView1_NodeEdit">
                        <ContextMenus>
                            <telerik:RadTreeViewContextMenu  Skin="Outlook" >
                                <Items>
                                    <telerik:RadMenuItem Value="NewRelation" Text="Voeg een nieuwe Relatie toe" ImageUrl="Img/9.gif">
                                    </telerik:RadMenuItem>
                                    <telerik:RadMenuItem Value="Verwijder" Text="Verwijder Relatie" ImageUrl="Img/7.gif">
                                    </telerik:RadMenuItem>
                                </Items>
                                <CollapseAnimation Type="none" />
                            </telerik:RadTreeViewContextMenu>
                        </ContextMenus>
                    </telerik:RadTreeView>
                </div>
                <div style="width: 180px; float: left;">
                    <span class="label">Corporate Structure 2</span>
                    <telerik:RadComboBox ID="RadComboBox2" runat="server" 
                        DataSourceID="ObjectDataSource1" DataTextField="DataKeyValue" 
                        DataValueField="DataKeyValue" Skin="Default">
                    </telerik:RadComboBox>
                    <telerik:RadButton ID="RadButton2" runat="server" Text="Zoek" 
                        Style="top: 0px; left: 0px" onclick="RadButton2_Click">
                    </telerik:RadButton>
                    <telerik:RadTreeView ID="RadTreeView2" runat="server" 
                        EnableDragAndDrop="True" 
                        OnNodeDrop="RadTreeView2_NodeDrop"
                        CssClass ="RadTreeView2"
                        OnClientNodeDropping="onNodeDropping" 
                        OnClientNodeDragging="onNodeDragging" 
                        MultipleSelect="true" 
                        EnableDragAndDropBetweenNodes="true" 
                        Skin="Outlook" 
                        onload="RadTreeView2_Load" 
                        onnodeclick="RadTreeView2_NodeClick"
                        AllowNodeEditing="true"
                        OnContextMenuItemClick="RadTreeView1_ContextMenuItemClick" 
                        OnClientContextMenuItemClicking="onClientContextMenuItemClicking"
                        OnClientContextMenuShowing="onClientContextMenuShowing" 
                        OnNodeEdit="RadTreeView1_NodeEdit">
                        <ContextMenus>
                            <telerik:RadTreeViewContextMenu Skin="Outlook">
                                <Items>
                                    <telerik:RadMenuItem Value="NewRelation" Text="Voeg een nieuwe Relatie toe" ImageUrl="Img/9.gif">
                                    </telerik:RadMenuItem>
                                    <telerik:RadMenuItem Value="Verwijder" Text="Verwijder Relatie" ImageUrl="Img/7.gif">
                                    </telerik:RadMenuItem>
                                </Items>
                                <CollapseAnimation Type="none" />
                            </telerik:RadTreeViewContextMenu>
                        </ContextMenus>
                    </telerik:RadTreeView>
                </div>

                <div style="clear: both">
                </div>
            </div>

        </telerik:RadAjaxPanel>


                        <div id="msg">
                        <div class="timeRemain">
                            <span class="timeSeconds"><span id="mainLbl"> </span></span>seconds
                         </div>
                    <telerik:RadNotification ID="RadNotification1" runat="server" Style="width: 180px;float: right;" 
                        OnCallbackUpdate="OnCallbackUpdate" OnClientShowing="OnClientShowing"
                        OnClientHidden="OnClientHidden" LoadContentOn="PageLoad" AutoCloseDelay="60000"
                        Title="Continue Your Session" TitleIcon="" Skin="Default" EnableRoundedCorners="true"
                        ShowCloseButton="false">
                        <ContentTemplate>
                            <div class="infoIcon">
                                <img src="Img/infoIcon.jpg" alt="info icon" /></div>
                            <div class="notificationContent">
                                Time remaining:&nbsp; <span id="timeLbl">60</span>
                                <telerik:RadButton Skin="Default" ID="continueSession" runat="server" Text="Ok"
                                    Style="margin-top: 10px;" AutoPostBack="false" OnClientClicked="ContinueSession">
                                </telerik:RadButton>
                            </div>
                        </ContentTemplate>
                    </telerik:RadNotification>
                </div>
        <asp:ObjectDataSource 
            ID="ObjectDataSource1" 
            runat="server" 
            TypeName="Dictionary.BusinessObjects.ShareHolders"
            SelectMethod="GetCompanies">
            
        </asp:ObjectDataSource>
</asp:Content>
