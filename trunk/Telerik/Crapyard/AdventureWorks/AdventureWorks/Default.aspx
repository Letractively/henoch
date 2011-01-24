<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="Default.aspx.cs" Inherits="AdventureWorks._Default" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">

</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <h2>
        Welcome to AdventureWorks!
    </h2>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnCategory" EventName="Click" />
        </Triggers>
        <ContentTemplate>
            <p>
                Product Categories
                <br />
                <asp:ListBox ID="lbCategories" runat="server" DataTextField="Name" DataValueField="ProductCategoryID"
                    Height="270px" Width="186px"></asp:ListBox>
            </p>
            <asp:UpdateProgress ID="UpdateProgress1" runat="server">
                <ProgressTemplate>
                    <div style="font-size: large">
                        Processing ...</div>
                </ProgressTemplate>
            </asp:UpdateProgress>
        </ContentTemplate>
    </asp:UpdatePanel>

    <p>
    
    <br />
    
    <asp:Button ID="btnCategory" runat="server" Text="Submit" 
            ></asp:Button>

    </p>
</asp:Content>
