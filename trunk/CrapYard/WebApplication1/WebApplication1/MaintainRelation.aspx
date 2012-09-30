

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

            </script>
        </telerik:RadScriptBlock>

        <telerik:RadAjaxManager runat="server" ID="RadAjaxManager1">
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
        <telerik:RadAjaxLoadingPanel runat="server" ID="RadAjaxLoadingPanel1" />
            <telerik:RadAjaxPanel ID="Panel1" runat="server">

            <div style="background: url(Img/bg.gif) no-repeat; padding: 115px 0px 0px 15px;">
                <div style="width: 180px; float: left;">
                    <span class="label">Corporate Structure 1</span>
                    <telerik:RadTextBox ID="RadTextBox1" runat="server" EnableSingleInputRendering="True"
                        LabelWidth="64px" Text="zoekwoord" Style="top: 0px; left: 0px">
                    </telerik:RadTextBox>
                    <telerik:RadButton ID="RadButton1" runat="server" Text="Zoek" 
                        Style="top: 0px; left: 0px" onclick="RadButton1_Click">
                    </telerik:RadButton>
                    <telerik:RadTreeView ID="RadTreeView1" runat="server" EnableDragAndDrop="True" OnNodeDrop="RadTreeView1_HandleDrop"
                        CssClass ="RadTreeView1"
                        OnClientNodeDropping="onNodeDropping" 
                        OnClientNodeDragging="onNodeDragging" MultipleSelect="true" 
                        EnableDragAndDropBetweenNodes="true" Skin="Outlook">
                    </telerik:RadTreeView>
                </div>
                <div style="width: 180px; float: left;">
                    <span class="label">Corporate Structure 2</span>
                    <telerik:RadTextBox ID="RadTextBox2" runat="server" EnableSingleInputRendering="True"
                        LabelWidth="64px" Text="zoekwoord" Style="top: 0px; left: 0px">
                    </telerik:RadTextBox>
                    <telerik:RadButton ID="RadButton2" runat="server" Text="Zoek" 
                        Style="top: 0px; left: 0px" onclick="RadButton2_Click">
                    </telerik:RadButton>
                    <telerik:RadTreeView ID="RadTreeView2" runat="server" EnableDragAndDrop="True" OnNodeDrop="RadTreeView1_HandleDrop"
                        CssClass ="RadTreeView2"
                        OnClientNodeDropping="onNodeDropping" 
                        OnClientNodeDragging="onNodeDragging" MultipleSelect="true" 
                        EnableDragAndDropBetweenNodes="true" Skin="Outlook">
                    </telerik:RadTreeView>
                </div>

                <div style="clear: both">
                </div>
            </div>
            <div id="msg">
            </div>
        </telerik:RadAjaxPanel>

</asp:Content>
