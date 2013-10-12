<%@ Page EnableSessionState="True" Language="C#" Trace="true" Async="true" AutoEventWireup="true"  MasterPageFile="~/Site.Master" 
    CodeBehind="AsyncTracer.aspx.cs" Inherits="Observlet.WebForms.AsyncTracer" %>

<asp:content id="Content1" contentplaceholderid="HeadContent" runat="server">
</asp:content>
<asp:content id="Content2" contentplaceholderid="MainContent" runat="server">

    
    
    <asp:label id="Label1" runat="server">
        Label 1</asp:label><br />
      <asp:label id="Label2" runat="server">
        Label 2</asp:label><br />
      <asp:label id="Label3" runat="server">
        Label 3</asp:label><br />
      <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
          <asp:Timer ID="Timer1" runat="server" Enabled="False" Interval="1000" 
            ontick="OnTimer1">
          </asp:Timer>
          <table style="width:100%;">
            <tr>
              <td class="style1" valign="bottom">
                <asp:TextBox ID="result" runat="server" columns="80" ReadOnly="true" rows="25" 
                  textMode="multiLine" Width="642px" Wrap="False" />
              </td>
              <td class="style2" valign="bottom">
                <asp:TextBox ID="Interlocks" runat="server" Enabled="False" Font-Bold="True" 
                  Font-Size="XX-Large" Height="119px" style="margin-top: 0px" 
                  TextMode="MultiLine" Width="260px"></asp:TextBox>
              </td>
              <td>
                &nbsp;</td>
            </tr>
            <tr>
              <td class="style1">
                <asp:RadioButton ID="RbStack" runat="server" 
                  oncheckedchanged="RbStack_CheckedChanged" Text="Stack" AutoPostBack="True" 
                  Checked="True" />
                <asp:RadioButton ID="RbQueue" runat="server" AutoPostBack="True" 
                  oncheckedchanged="RbQueue_CheckedChanged" Text="Queue" />
              </td>
              <td class="style2">
                <iframe id="asyncviewer" src="AsyncViewer2.aspx" frameborder="0" scrolling="no"></iframe>
                &nbsp;</td>
              <td>
                &nbsp;</td>
            </tr>
            <tr>
              <td class="style1">
                <asp:Button ID="BtnShowAll" runat="server" onclick="BtnShowAll_Click" 
                  Text="Show  Complete" ToolTip="shows all stack or queue job schedule" />
              </td>
              <td class="style2">
                &nbsp;</td>
              <td>
                &nbsp;</td>
            </tr>
          </table>
        </ContentTemplate>
        <Triggers>
          <asp:AsyncPostBackTrigger ControlID="BtnToggle" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="BtnClear" />
          <asp:AsyncPostBackTrigger ControlID="BtnTimer" />
          <asp:AsyncPostBackTrigger ControlID="RbStack" EventName="CheckedChanged" />
          <asp:AsyncPostBackTrigger ControlID="RbQueue" EventName="CheckedChanged" />
        </Triggers>
    </asp:UpdatePanel>


    <p>
      <asp:Button ID="BtnToggle" runat="server" onclick="BtnToggle_Click" 
        Text="Toggle 1" />
      <asp:Button ID="BtnToggle0" runat="server" onclick="BtnToggle0_Click" 
        Text="Toggle 2" />
        <asp:Button ID="BtnClear" runat="server" height="26px" onclick="BtnClear_Click" 
            Text="Clear List" width="71px" />
      <asp:Button ID="BtnTimer" runat="server" height="26px" onclick="BtnTimer_Click" 
        Text="Pause Toggle" width="102px" ToolTip="Wait 1 second for respond..." />
    </p>


    <p>
          <asp:TextBox ID="MaxLoop" runat="server">10</asp:TextBox>
          </p>


</asp:content>
