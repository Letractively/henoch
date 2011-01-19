<%@ Page Language="C#" Trace="true" Async="true" AutoEventWireup="true"  CodeBehind="Tracer.aspx.cs" Inherits="Observlet.WebForms.Tracer" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
    <script type="text/javascript">
    
      function pageLoad() {
      }
    

        
    </script>
    <style type="text/css">
        .style2
        {
            width: 495px;
        }
        .style3
        {
            width: 92px;
        }
        .style4
        {
            width: 427px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:ScriptManager ID="ScriptManager1" runat="server" />
    </div>
    <table style="width:59%;">
      <tr>
        <td class="style3">
            &nbsp;</td>
        <td class="style4">
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <asp:TextBox ID="Message" runat="server" Height="348px" TextMode="MultiLine" 
                            Width="376px" ontextchanged="Message_TextChanged" 
                        ReadOnly="True"></asp:TextBox>
                    <div>
                        <asp:Button ID="StartBackgroundWork" runat="server" height="26px" 
                            onclick="DoBackgroundWork_Click" Text="BackgroundWork" width="129px" />
                        <asp:Button ID="BtnTicker" runat="server" height="26px" 
                            onclick="BtnTicker_Click" Text="Toggle Timer1" width="129px" />
                    </div>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="Timer1" />
                    <asp:AsyncPostBackTrigger ControlID="StartBackgroundWork" />
                </Triggers>
            </asp:UpdatePanel>
        </td>
        <td class="style2">
            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                <ContentTemplate>
                    <asp:TextBox ID="Message2" runat="server" height="348px" ReadOnly="True" 
                        TextMode="MultiLine" width="376px"></asp:TextBox>
                    <div>
                        <asp:Button ID="BtnTicker2" runat="server" height="26px" 
                            onclick="BtnTicker2_Click" Text="Toggle Timer2" width="129px" />
                    </div>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="Timer2" />
                </Triggers>
            </asp:UpdatePanel>
          </td>
      </tr>
      <tr>
        <td class="style3">
            &nbsp;</td>
        <td class="style4">
          <asp:Button ID="StartWork" runat="server" onclick="DoWork_Click" Text="Work" 
                Width="129px" />
          </td>
        <td class="style2">
            
            &nbsp;</td>
      </tr>
      <tr>
        <td class="style3">
            &nbsp;</td>
        <td class="style4">
            <asp:Timer ID="Timer1" runat="server" Enabled="False" Interval="500" 
                ontick="ObserverNotified">
            </asp:Timer>
            <asp:TextBox ID="BtnMaxLoop" runat="server">50</asp:TextBox>
          </td>
        <td class="style2">
            <asp:Timer ID="Timer2" runat="server" Enabled="False" Interval="500" 
                ontick="OnTimer2">
            </asp:Timer>
          </td>
      </tr>
    </table>
    </form>
</body>
</html>
