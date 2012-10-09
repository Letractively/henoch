<%@ Page Title="About Us" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="About.aspx.cs" Inherits="WebApplication1.About" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <h2>
        About
    </h2>
    <p>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        Put content here.
    </p>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:GridView ID="GridView1" runat="server" AllowPaging="True" 
                AllowSorting="True" AutoGenerateColumns="False" DataKeyNames="pjtcd" 
                DataSourceID="SqlDataSource1">
                <Columns>
                    <asp:CommandField ShowSelectButton="True" />
                    <asp:BoundField DataField="pjtcd" HeaderText="pjtcd" ReadOnly="True" 
                        SortExpression="pjtcd" />
                    <asp:BoundField DataField="ordNo" HeaderText="ordNo" 
                        SortExpression="ordNo" />
                    <asp:BoundField DataField="ordLn" HeaderText="ordLn" 
                        SortExpression="ordLn" />
                    <asp:BoundField DataField="dateFr" HeaderText="dateFr" 
                        SortExpression="dateFr" />
                    <asp:BoundField DataField="dateTo" HeaderText="dateTo" 
                        SortExpression="dateTo" />
                    <asp:BoundField DataField="cust" HeaderText="cust" SortExpression="cust" />
                    <asp:BoundField DataField="dte" HeaderText="dte" SortExpression="dte" />
                    <asp:BoundField DataField="st" HeaderText="st" SortExpression="st" />
                    <asp:BoundField DataField="uid" HeaderText="uid" SortExpression="uid" />
                    <asp:BoundField DataField="recdate" HeaderText="recdate" 
                        SortExpression="recdate" />
                    <asp:BoundField DataField="owner" HeaderText="owner" SortExpression="owner" />
                    <asp:BoundField DataField="lockedby" HeaderText="lockedby" 
                        SortExpression="lockedby" />
                    <asp:BoundField DataField="locktime" HeaderText="locktime" 
                        SortExpression="locktime" />
                    <asp:BoundField DataField="onmachine" HeaderText="onmachine" 
                        SortExpression="onmachine" />
                </Columns>
            </asp:GridView>
            <asp:SqlDataSource ID="SqlDataSource1" runat="server" 
                ConnectionString="<%$ ConnectionStrings:sygnionTro %>" 
                ProviderName="<%$ ConnectionStrings:sygnionTro.ProviderName %>" 
                SelectCommand="GetInstalledTrotters" SelectCommandType="StoredProcedure"></asp:SqlDataSource>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
