<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="ShoppingCart.aspx.cs" Inherits="AdventureWorks.ShoppingCart1" %>

<asp:Content ID="HeaderContent" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <script src="JScript/jquery-1.4.4.min.js" type="text/javascript"></script>
    <h2>
        Shopping Cart
    </h2>
    <asp:Label ID="lblMessage" runat="server" Text="Label"></asp:Label>
    <br />
    <asp:GridView ID="gvCart" runat="server" AutoGenerateColumns="False">
        <Columns>
            <asp:BoundField DataField="ProductId" HeaderText="ID" />
            <asp:BoundField DataField="Name" HeaderText="Name" />
            <asp:BoundField DataField="ListPrice" HeaderText="Price" />
            <asp:BoundField DataField="Quantity" HeaderText="Quantity" />
        </Columns>
    </asp:GridView>
    <br />
    <asp:Button ID="btnContinue" runat="server"  Text="Continue Shopping" />
    <asp:Button ID="btnPlaceOrder" runat="server" 
        Text="Check Out" CssClass="placeOrder" ToolTip="pay using iDeal" />
    <div id = "CheckOut"></div>
</asp:Content>
