<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebForm1.aspx.cs" Inherits="TelerikExample.WebForm1" %>

<%@ Register assembly="Telerik.Web.UI" namespace="Telerik.Web.UI" tagprefix="telerik" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        <Scripts>
            
            <asp:ScriptReference Path="~/Scripts/jquery-1.4.4.min.js" />
        </Scripts>
        <Services>
            <asp:ServiceReference Path="~/MyService.asmx"/>
        </Services>
    </asp:ScriptManager>
        <telerik:RadButton ID="RadButton1" runat="server" Text="Postback" 
            onclick="RadButton1_Click">
        </telerik:RadButton>
        <br />
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            
            <ContentTemplate>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="RadButton2" EventName="Click" />
            </Triggers>
        </asp:UpdatePanel>
        <telerik:RadButton ID="RadButton2" runat="server" AutoPostBack="False" 
            Text="Ajax Request">
        </telerik:RadButton>

    </div>
    </form>
</body>
</html>
