<%@ Page Language="C#" AutoEventWireup="true" Async="true" CodeBehind="AsyncViewer2.aspx.cs" Inherits="Observlet.WebForms.AsyncViewer2" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI, Version=2010.3.1215.35, Culture=neutral, PublicKeyToken=121fae78165ba3d4" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>

<body>
     <form id="Form1"  runat="server">
     <asp:ScriptManager ID="ScriptManager1" runat="server">
         <Scripts>
            <asp:ScriptReference Path="~/Scripts/jquery-1.4.4.min.js"/>
             
            <asp:ScriptReference Path="~/Scripts/jqUtil.js"/>
         </Scripts>
     </asp:ScriptManager>


     
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
                <table style="width: 100%;">
        <tr>
            <td>
                
                <asp:Button ID="Button1" runat="server" CssClass="async" 
                    onclick="Button1_Click" Text="Start Async" />&nbsp;&nbsp;&nbsp;
                <asp:Button ID="Button2" runat="server" CssClass="interactasync" Enabled="false" 
                    onclick="Button2_Click" Text="Interact with Async Worker" />
            </td>
            <td>
                &nbsp;
                </td>
            <td>
                &nbsp;
                </td>
        </tr>
        <tr>
            <td>
               
                <asp:Label ID="Label1" runat="server"></asp:Label> &nbsp; &nbsp;
                <asp:Label ID="Label2" runat="server"></asp:Label>
            </td>
            <td>
                &nbsp;
                </td>
            <td>
                &nbsp;
            </td>
        </tr>
        <tr>
            <td>
               
                <asp:Label ID="Label3" CssClass="result" runat="server"></asp:Label>
                 &nbsp;&nbsp;
                <telerik:RadNumericTextBox ID="RadNumericTextBox1" Runat="server" 
                    Skin="Windows7" NumberFormat-DecimalSeparator="." NumberFormat-KeepTrailingZerosOnFocus ="true">
                    <NumberFormat DecimalDigits="2"/>
                </telerik:RadNumericTextBox> &nbsp;&nbsp;</td>
            <td>
               
            </td>
            <td>
                &nbsp;
            </td>
        </tr>
    </table>    
        <asp:Timer ID="Timer1" runat="server" Interval="500" ontick="Timer1_Tick" 
            Enabled="False">
        </asp:Timer>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="Button1" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="Button2" EventName="Click" />
        </Triggers>
    </asp:UpdatePanel>

<asp:TextBox ID="numericTextBox" 
                    CssClass="numericCssClass" runat="server"></asp:TextBox>
            

    </form>

</body> 
</html>
