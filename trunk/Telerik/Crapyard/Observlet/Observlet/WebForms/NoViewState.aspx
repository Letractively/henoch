<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="True" 
CodeBehind="NoViewState.aspx.cs" Inherits="Observlet.WebForms.NoViewState" EnableViewState="false"%>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManagerProxy ID="Proxy" runat="server">
        <Scripts>
            <asp:ScriptReference Path="~/Scripts/jquery-1.4.4.min.js" />            
        </Scripts>
        <Services>
            <asp:ServiceReference Path="~/MyService.asmx" />
        </Services>
    </asp:ScriptManagerProxy>
    <telerik:radajaxmanager ID="RadAjaxManager1" runat="server">
        <ClientEvents OnRequestStart="OnRequestStart" OnResponseEnd="OnResponseEnd" />
        <AjaxSettings>
            <telerik:ajaxsetting AjaxControlID="RadAjaxManager1">
                <UpdatedControls>
                    <telerik:ajaxupdatedcontrol ControlID="RadGrid1" />
                </UpdatedControls>
            </telerik:ajaxsetting>
            <telerik:ajaxsetting AjaxControlID="RadGrid1">
                <UpdatedControls>
                    <telerik:ajaxupdatedcontrol ControlID="RadGrid1" />
                </UpdatedControls>
            </telerik:ajaxsetting>
        </AjaxSettings>
    </telerik:radajaxmanager>
    <div>
        <telerik:radajaxpanel ID="RadAjaxPanel1" runat="server" EnableAJAX="false">
            <br />
            <telerik:radbutton ID="RadButton1" runat="server" CssClass="search" Skin="Windows7"
                Text="Search" AutoPostBack="False">
            </telerik:radbutton>
            <telerik:radbutton ID="RadButton2" runat="server" Text="Hello" CssClass="subroutine"
                AutoPostBack="False" Skin="Windows7">
            </telerik:radbutton>
            <telerik:radbutton ID="RadButton3" runat="server" AutoPostBack="False" BackColor="Red"
                CssClass="error" ForeColor="Red" Skin="Telerik" Text="Error">
            </telerik:radbutton>
            <telerik:radbutton ID="RadButton4" runat="server" Text="Postback">
            </telerik:radbutton>
            <asp:TextBox ID="TextBox1" runat="server" CssClass="numericCssClass"></asp:TextBox>
            <asp:TextBox ID="TextBox2" runat="server"></asp:TextBox><cc1:filteredtextboxextender
                ID="FilteredTextBoxExtender1" runat="server" FilterType="Numbers" 
            TargetControlID="TextBox2">
            </cc1:filteredtextboxextender>
            <telerik:radgrid ID="RadGrid1" runat="server" OnDetailTableDataBind="RadGrid1_DetailTableDataBind"
                OnItemCreated="RadGrid1_ItemCreated" OnNeedDataSource="RadGrid1_NeedDataSource"
                Width="800px">
                <MasterTableView CommandItemDisplay="Top" DataKeyNames="ID">
                    <DetailTables>
                        <telerik:gridtableview CommandItemDisplay="Top" 
            DataKeyNames="ID">
                            <DetailTables>
                                <telerik:gridtableview CommandItemDisplay="Top" 
            DataKeyNames="ID, CategoryID" Name="RelatedItems">
                                    <DetailTables>
                                        <telerik:gridtableview CommandItemDisplay="Top" 
            DataKeyNames="ID" Name="InnerMost">
                                        </telerik:gridtableview></DetailTables>
                                </telerik:gridtableview></DetailTables>
                        </telerik:gridtableview></DetailTables>
                </MasterTableView></telerik:radgrid>
            <telerik:radgrid ID="RadGrid2" runat="server">
            </telerik:radgrid>
        </telerik:radajaxpanel>

        <telerik:radcodeblock ID="RadCodeBlock1" runat="server">            
            <script src="../Scripts/jqUtil.js" type="text/javascript"></script>
        </telerik:radcodeblock>
    </div>

   <asp:Label ID="Label1" runat="server" CssClass="log"></asp:Label>
        <div id="msg"></div>      
</asp:Content>

