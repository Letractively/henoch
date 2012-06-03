<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebForm1.aspx.cs" Inherits="TelerikExample.WebForm1" %>

<%@ Register assembly="Telerik.Web.UI" namespace="Telerik.Web.UI" tagprefix="telerik" %>

<%@ Register assembly="Telerik.Web.UI" namespace="Telerik.Charting" tagprefix="telerik" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link href="styles/kendo.common.min.css" rel="stylesheet" type="text/css" />    
    <link href="styles/kendo.default.min.css" rel="stylesheet" type="text/css" />
    <%--<link href="styles/kendo.dataviz.min.css" rel="stylesheet" type="text/css" />--%>
     <script src="Scripts/jquery.min.js" type="text/javascript"></script>    
     <script src="Scripts/kendo.web.min.js" type="text/javascript"></script>
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
                <telerik:RadChart ID="RadChart1" runat="server" DefaultType="Line">
                    <series>
                        <telerik:ChartSeries Name="Series 1" Type="Line">
                            <appearance>
                                <fillstyle maincolor="213, 247, 255">
                                </fillstyle>
                            </appearance>
                        </telerik:ChartSeries>
                    </series>
                </telerik:RadChart>
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
