<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CRMTree.aspx.cs" Inherits="WebApplication1.CRMTree" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager ID="RadScriptManager1" runat="server">
        <Scripts>
            <%--Needed for JavaScript IntelliSense in VS2010--%>
            <%--For VS2008 replace RadScriptManager with ScriptManager--%>
            <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.Core.js" />
            <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQuery.js" />
            <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQueryInclude.js" />
        </Scripts>
    </telerik:RadScriptManager>
    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" />
        <telerik:RadTextBox ID="RadTextBox1" Runat="server">
        </telerik:RadTextBox>
                <telerik:RadButton ID="RadButton1" runat="server" Text="RadButton">
        </telerik:RadButton>
    <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server">
        <telerik:RadTreeView ID="RadTreeView1" Runat="server" 
            DataSourceID="ObjectDataSource1">
        </telerik:RadTreeView>
        <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" 
            SelectMethod="GetSubsidiaries" TypeName="Repository.ShareHolders">
            <SelectParameters>
                <asp:ControlParameter ControlID="RadTextBox1" Name="shareHolder" 
                    PropertyName="Text" Type="String" />
            </SelectParameters>
        </asp:ObjectDataSource>
        <asp:LinqDataSource ID="LinqDataSource1" runat="server" EntityTypeName="">
        </asp:LinqDataSource>
    </telerik:RadAjaxPanel>

    </form>
</body>
</html>
