<%@ Page Language="C#" AutoEventWireup="True"  Inherits="Tickets_327691_Default" 
    Codebehind="PersistExpandedState20.aspx.cs" culture="auto" meta:resourcekey="PageResource1" uiculture="auto" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>    
    <script src="Scripts/jquery-1.4.4.min.js" type="text/javascript"></script>
    <link href="Styles/Site.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <div id="msg"></div>
    <asp:ScriptManager ID="ScriptManager1" runat="server">
        <Scripts>
            
            <asp:ScriptReference Path="~/Scripts/jquery-1.4.4.min.js" />
        </Scripts>
        <Services>
            <asp:ServiceReference Path="~/MyService.asmx"/>
        </Services>
    </asp:ScriptManager>

    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
        <ClientEvents OnRequestStart="OnRequestStart" OnResponseEnd="OnResponseEnd"/>
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="RadAjaxManager1">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid1"  />
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
            <telerik:RadButton ID="RadButton1" runat="server" CssClass="search" 
                Skin="Windows7" Text="Search" AutoPostBack="False">
            </telerik:RadButton>
            <telerik:RadButton ID="RadButton2" runat="server" Text="Hello" 
                CssClass="subroutine" AutoPostBack="False" Skin="Windows7">
            </telerik:RadButton>
            <telerik:RadButton ID="RadButton3" runat="server" AutoPostBack="False" 
                BackColor="Red" CssClass="error" ForeColor="Red" Skin="Telerik" Text="Error">
            </telerik:RadButton>
            <telerik:RadGrid ID="RadGrid1" runat="server" 
                OnDetailTableDataBind="RadGrid1_DetailTableDataBind" 
                OnItemCreated="RadGrid1_ItemCreated" OnNeedDataSource="RadGrid1_NeedDataSource" 
                Width="800px">
                <mastertableview commanditemdisplay="Top" datakeynames="ID">
                    <detailtables>
                        <telerik:GridTableView CommandItemDisplay="Top" DataKeyNames="ID">
                            <detailtables>
                                <telerik:GridTableView CommandItemDisplay="Top" DataKeyNames="ID, CategoryID" 
                                    Name="RelatedItems">
                                    <detailtables>
                                        <telerik:GridTableView CommandItemDisplay="Top" DataKeyNames="ID" 
                                            Name="InnerMost">
                                        </telerik:GridTableView>
                                    </detailtables>
                                </telerik:GridTableView>
                            </detailtables>
                        </telerik:GridTableView>
                    </detailtables>
                </mastertableview>
            </telerik:RadGrid>
        </telerik:RadAjaxPanel>
        <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
            <script src="Scripts/jqUtil.js" type="text/javascript"></script>

        </telerik:RadCodeBlock>
    </div>
    </form>
</body>
</html>
