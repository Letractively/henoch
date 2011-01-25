<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" Async="true"
    CodeBehind="AsyncViewer.aspx.cs" Inherits="TelerikExample.AsyncViewer" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:Button ID="Button1" runat="server" Text="Start Async" 
        onclick="Button1_Click" />
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
                <table style="width: 100%;">
        <tr>
            <td>
                &nbsp;
                <asp:Label ID="Label1" runat="server"></asp:Label>
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
                &nbsp;
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
                &nbsp;
                <asp:Label ID="Label3" runat="server"></asp:Label>
            </td>
            <td>
                &nbsp;
            </td>
            <td>
                &nbsp;
            </td>
        </tr>
    </table>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="Button1" EventName="Click" />
        </Triggers>
    </asp:UpdatePanel>
    <asp:Timer ID="Timer1" runat="server" Interval="50" ontick="Timer1_Tick" 
        Enabled="False">
    </asp:Timer>
</asp:Content>
