

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
                 <asp:ScriptReference Path="~/Scripts/Util.js" />
            </Scripts>
        </telerik:RadScriptManager>

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
