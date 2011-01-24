<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ProductDetail.aspx.cs" Inherits="AdventureWorks.ProductDetail" %>
<asp:Content ID="HeaderContent" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2>
        <asp:Label ID="lblName" runat="server" Text="Label"></asp:Label>
    </h2>
    <TABLE border="1">
        <TR>
            <td class="bold">ID</td>
            <td><asp:Label ID="lblId" runat="server" Text="Label"></asp:Label></td>
        </TR>
        <TR>
            <td class="bold">Color</td>
            <td><asp:Label ID="lblColor" runat="server" Text="Label"></asp:Label></td>
        </TR>
        <TR>
            <td class="bold">Size</td>
            <td><asp:Label ID="lblSize" runat="server" Text="Label"></asp:Label></td>
        </TR>
        <TR>
            <td class="bold">Weight</td>
            <td><asp:Label ID="lblWeight" runat="server" Text="Label"></asp:Label></td>
        </TR>
        <TR>
            <td class="bold">List Price</td>
            <td><asp:Label ID="lblListPrice" runat="server" Text="Label"></asp:Label></td>
        </TR>                     
    </table>
    <p>
    Quantity 
    <asp:TextBox ID="txtQuantity" runat="server">1</asp:TextBox>    
        <asp:RequiredFieldValidator ID="RequiredFieldValidatorQty" runat="server" 
            ControlToValidate="txtQuantity" ErrorMessage="Please enter a quantity"></asp:RequiredFieldValidator>
    </p>
    <asp:HiddenField ID="hdnProductId" runat="server" />
    <asp:HiddenField ID="hdnProductName" runat="server" />
    <asp:HiddenField ID="hdnListPrice" runat="server" />
    <asp:Button ID="btnOrder" runat="server" Text="Order" 
        onclick="btnOrder_Click" />
    
    <br />
    <asp:Label ID="lblError" runat="server"></asp:Label>
    
    </asp:Content>
