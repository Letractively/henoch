
<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Themas.aspx.cs" Inherits="MetaData.Beheer.Themas"
    Title="Themas" MasterPageFile="~/Shared/DefaultMaster.master" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>

<%@ Register assembly="Microsoft.Practices.Web.UI.WebControls" namespace="Microsoft.Practices.Web.UI.WebControls" tagprefix="pp" %>

<asp:Content ID="content1" ContentPlaceHolderID="DefaultContent" Runat="Server">

		<h1>BeheerThemas<table style="width:100%;">
      <tr>
        <td style="width: 108px">
            &nbsp;</td>
        <td>

            &nbsp;</td>
        <td>
            &nbsp;</td>
      </tr>
      <tr>
        <td style="width: 108px">
          <table style="width:100%;">
            <tr>
              <td>
                &nbsp;</td>
            </tr>
            <tr>
              <td>

          <asp:DetailsView ID="DetailsView1" runat="server" AutoGenerateRows="False" 
            DataSourceID="ThemaTableDataSource" Font-Size="Medium" Height="16px" 
            Width="321px" DefaultMode="Insert" CellPadding="3" 
                      BorderStyle="Outset" BackColor="White" BorderColor="#CCCCCC" 
                      BorderWidth="1px" 
                      DataKeyNames="Tablename,Id,DataKeyValue,DataKeyName">
              <FooterStyle BackColor="White" ForeColor="#000066" />
              <RowStyle ForeColor="#000066" />
              <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
            <Fields>
                <asp:TemplateField HeaderText="Themanaam" SortExpression="DataKeyValue">
                    <EditItemTemplate>
                        <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("DataKeyValue") %>'></asp:TextBox>
                    </EditItemTemplate>
                    <InsertItemTemplate>
                        <asp:TextBox ID="TbThemaNaam" runat="server" Text='<%# Bind("DataKeyValue") %>' 
                            MaxLength="100" CausesValidation="True" ValidationGroup="insert"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RfvThemaNaam" runat="server" 
                            ControlToValidate="TbThemaNaam" Display="None" 
                            ErrorMessage="&lt;b&gt;Themanaam is verplicht.&lt;/b&gt;" 
                            ValidationGroup="insert"></asp:RequiredFieldValidator>
                        <asp:ValidatorCalloutExtender ID="RfvThemaNaamExt" runat="server" 
                            Enabled="True" TargetControlID="RfvThemaNaam">
                        </asp:ValidatorCalloutExtender>
                    </InsertItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="Label1" runat="server" Text='<%# Bind("DataKeyValue") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField ShowHeader="False">
                   <InsertItemTemplate>
                        <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="True" 
                            CommandName="Insert" Text="Insert" ValidationGroup="insert"></asp:LinkButton>
                        &nbsp;<asp:LinkButton ID="LinkButton2" runat="server" CausesValidation="False" 
                            CommandName="Cancel" Text="Cancel"></asp:LinkButton>
                    </InsertItemTemplate>
                    <ItemTemplate>
                        <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" 
                            CommandName="New" Text="New"></asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
            </Fields>
              <HeaderStyle BackColor="#006699" Font-Bold="True" ForeColor="White" />
              <EditRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
          </asp:DetailsView>

              </td>
            </tr>
            <tr>
              <td>
                &nbsp;</td>
            </tr>
          </table>
        </td>
        <td>

          &nbsp;</td>
        <td>
            <br />

          <asp:DetailsView ID="DetailsView2" runat="server" AutoGenerateRows="False" 
            DataSourceID="DatumContainer" Font-Size="Medium" Height="50px" 
            Width="280px" DefaultMode="Insert" CellPadding="3" 
                      GridLines="Horizontal" BackColor="White" BorderColor="#E7E7FF" 
                      BorderStyle="None" BorderWidth="1px" Visible="False">
              <FooterStyle BackColor="#B5C7DE" ForeColor="#4A3C8C" />
              <RowStyle BackColor="#E7E7FF" ForeColor="#4A3C8C" />
              <PagerStyle BackColor="#E7E7FF" ForeColor="#4A3C8C" HorizontalAlign="Right" />
            <Fields>
                <asp:BoundField DataField="DataKeyValue" HeaderText="DataKeyValue" 
                    SortExpression="DataKeyValue" />
                <asp:CommandField ShowInsertButton="True" />
            </Fields>
              <HeaderStyle BackColor="#4A3C8C" Font-Bold="True" ForeColor="#F7F7F7" />
              <EditRowStyle BackColor="#738A9C" Font-Bold="True" ForeColor="#F7F7F7" />
              <AlternatingRowStyle BackColor="#F7F7F7" />
          </asp:DetailsView>

            <br />
          </td>
      </tr>
      <tr>
        <td style="width: 108px">

          <asp:GridView ID="GridView2" runat="server" CellPadding="3" 
            Font-Size="Medium" DataSourceID="ThemaTableDataSource" 
            AutoGenerateColumns="False" AllowPaging="True" AllowSorting="True" 
            Width="277px" BackColor="White" 
                BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" 
                DataKeyNames="Tablename,Id,DataKeyValue,DataKeyName">
            <RowStyle ForeColor="#000066" />
            <Columns>
                <asp:TemplateField ShowHeader="False">
                    <EditItemTemplate>
                        <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="True" 
                            CommandName="Update" Text="Update" ValidationGroup="edit"></asp:LinkButton>
                        &nbsp;<asp:LinkButton ID="LinkButton2" runat="server" CausesValidation="False" 
                            CommandName="Cancel" Text="Cancel"></asp:LinkButton>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" 
                            CommandName="Edit" Text="Edit"></asp:LinkButton>
                        &nbsp;<asp:LinkButton ID="LinkButton2" runat="server" CausesValidation="False" 
                            CommandName="Delete" Text="Delete"></asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Themanaam" SortExpression="DataKeyValue">
                    <EditItemTemplate>
                        <asp:TextBox ID="TbThemaNaamEdit" runat="server" MaxLength="32" 
                            Text='<%# Bind("DataKeyValue") %>' CausesValidation="True" 
                            ValidationGroup="edit"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RfvThemaNaam" runat="server" 
                            ControlToValidate="TbThemaNaamEdit" Display="None" 
                            ErrorMessage="&lt;b&gt;Themanaam is verplicht.&lt;/b&gt;" 
                            ValidationGroup="edit"></asp:RequiredFieldValidator>
                        <asp:ValidatorCalloutExtender ID="RfvThemaNaamEditExt" runat="server" 
                            Enabled="True" TargetControlID="RfvThemaNaam">
                        </asp:ValidatorCalloutExtender>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="Label1" runat="server" Text='<%# Bind("DataKeyValue") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
            <FooterStyle BackColor="White" ForeColor="#000066" />
            <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
            <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
            <HeaderStyle BackColor="#006699" Font-Bold="True" ForeColor="White" />
          </asp:GridView>

          <pp:ObjectContainerDataSource ID="ThemaTableDataSource" runat="server" 
            
            DataObjectTypeName="Beheer.BusinessObjects.Dictionary.BeheerContextEntity" 
            ondeleted="ThemaTableDataSource_Deleted" 
            oninserted="ThemaTableDataSource_Inserted" 
            onupdated="ThemaTableDataSource_Updated" />
        </td>
        <td>
          &nbsp;</td>
        <td>
            <asp:GridView ID="GridView3" runat="server" AllowPaging="True" 
                AutoGenerateColumns="False" BackColor="White" BorderColor="#CCCCCC" 
                BorderStyle="None" BorderWidth="1px" CellPadding="3" 
                DataSourceID="DatumContainer" Font-Size="Medium">
                <RowStyle ForeColor="#000066" />
                <Columns>
                    <asp:BoundField DataField="ThemaNaam" HeaderText="ThemaNaam" 
                        SortExpression="ThemaNaam" />
                    <asp:BoundField DataField="DateTime" HeaderText="DateTime" 
                        SortExpression="DateTime" />
                    <asp:BoundField DataField="Id" HeaderText="Id" SortExpression="Id" />
                    <asp:BoundField DataField="DataKeyValue" HeaderText="DataKeyValue" 
                        SortExpression="DataKeyValue" />
                </Columns>
                <FooterStyle BackColor="White" ForeColor="#000066" />
                <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
                <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
                <HeaderStyle BackColor="#006699" Font-Bold="True" ForeColor="White" />
            </asp:GridView>
            <pp:ObjectContainerDataSource ID="DatumContainer" runat="server" 
                DataObjectTypeName="MetaData.Beheer.Interface.BusinessEntities.Thema" />
          </td>
      </tr>
      </table>
    </h1>
</asp:Content>
