<%@ Page Language="C#" AutoEventWireup="True" Inherits="Tickets_327691_Default" CodeBehind="PersistExpandedState20.aspx.cs"
    Culture="auto" meta:resourcekey="PageResource1" UICulture="auto"  MasterPageFile="~/Site.Master"%>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
   <asp:ScriptManagerProxy ID="Proxy" runat="server">
        <Scripts>
            <asp:ScriptReference Path="~/Scripts/jquery-1.4.4.min.js" />            
        </Scripts>
        <Services>
            <asp:ServiceReference Path="~/MyService.asmx" />
        </Services>
    </asp:ScriptManagerProxy>
    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
        <ClientEvents OnRequestStart="OnRequestStart" OnResponseEnd="OnResponseEnd" />
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="RadAjaxManager1">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="RadGrid1">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
    <div>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" EnableAJAX="false">
            <br />
            <telerik:RadButton ID="RadButton1" runat="server" CssClass="search" Skin="Windows7"
                Text="Search" AutoPostBack="False">
            </telerik:RadButton>
            <telerik:RadButton ID="RadButton2" runat="server" Text="Hello" CssClass="subroutine"
                AutoPostBack="False" Skin="Windows7">
            </telerik:RadButton>
            <telerik:RadButton ID="RadButton3" runat="server" AutoPostBack="False" BackColor="Red"
                CssClass="error" ForeColor="Red" Skin="Telerik" Text="Error">
            </telerik:RadButton>
            <telerik:RadButton ID="RadButton4" runat="server" Text="Postback">
            </telerik:RadButton>
            <asp:TextBox ID="TextBox1" runat="server" CssClass="numericCssClass"></asp:TextBox>
            <asp:TextBox ID="TextBox2" runat="server"></asp:TextBox><cc1:FilteredTextBoxExtender
                ID="FilteredTextBoxExtender1" runat="server" FilterType="Numbers" TargetControlID="TextBox2">
            </cc1:FilteredTextBoxExtender>
            <telerik:RadGrid ID="RadGrid1" runat="server" OnDetailTableDataBind="RadGrid1_DetailTableDataBind"
                OnItemCreated="RadGrid1_ItemCreated" OnNeedDataSource="RadGrid1_NeedDataSource"
                Width="800px">
                <MasterTableView CommandItemDisplay="Top" DataKeyNames="ID">
                    <DetailTables>
                        <telerik:GridTableView CommandItemDisplay="Top" DataKeyNames="ID">
                            <DetailTables>
                                <telerik:GridTableView CommandItemDisplay="Top" DataKeyNames="ID, CategoryID" Name="RelatedItems">
                                    <DetailTables>
                                        <telerik:GridTableView CommandItemDisplay="Top" DataKeyNames="ID" Name="InnerMost">
                                        </telerik:GridTableView></DetailTables>
                                </telerik:GridTableView></DetailTables>
                        </telerik:GridTableView></DetailTables>
                </MasterTableView></telerik:RadGrid>
        </telerik:RadAjaxPanel>

        <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">            
            <script src="../Scripts/jqUtil.js" type="text/javascript"></script>
        </telerik:RadCodeBlock>
    </div>

   <asp:Label ID="Label1" runat="server" CssClass="log"></asp:Label>
        <div id="msg"></div>      
</asp:Content>

