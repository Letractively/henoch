<%@ Page Language="C#" AutoEventWireup="True"  Inherits="Tickets_327691_Default" 
    Codebehind="PersistExpandedState20.aspx.cs" culture="auto" meta:resourcekey="PageResource1" uiculture="auto" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>    
    <script src="Scripts/jquery-1.4.4.min.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
    <div id="msg"></div>
    <asp:ScriptManager ID="ScriptManager1" runat="server">
        <Scripts>
            
            <asp:ScriptReference Path="~/Scripts/jquery-1.4.4.min.js" />
        </Scripts>
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
            <telerik:RadGrid ID="RadGrid1" runat="server" Width="800px"
                OnNeedDataSource="RadGrid1_NeedDataSource"
                OnDetailTableDataBind="RadGrid1_DetailTableDataBind"
                OnItemCreated="RadGrid1_ItemCreated">
                
                   
                
                <MasterTableView DataKeyNames="ID" CommandItemDisplay="Top" >
                    <DetailTables>
                        <telerik:GridTableView DataKeyNames="ID"
                            CommandItemDisplay="Top">
                            <DetailTables>
                                <telerik:GridTableView DataKeyNames="ID, CategoryID"
                                    CommandItemDisplay="Top" Name="RelatedItems" >
                                    <DetailTables>
                                        <telerik:GridTableView DataKeyNames="ID" Name="InnerMost"
                                            CommandItemDisplay="Top" >
                                        </telerik:GridTableView>
                                    </DetailTables>
                                </telerik:GridTableView>
                            </DetailTables>
                        </telerik:GridTableView>
                    </DetailTables>
                </MasterTableView>
            </telerik:RadGrid>
        </telerik:RadAjaxPanel>
        <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
            <script src="Scripts/jqUtil.js" type="text/javascript"></script>

        </telerik:RadCodeBlock>
    </div>
    </form>
</body>
</html>
