
<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Audittrail.aspx.cs" Inherits="MetaData.Audittrail.Views.Audittrail"
    Title="Audittrail" MasterPageFile="~/Shared/DefaultMaster.master" %>
<%@ Register assembly="Microsoft.Practices.Web.UI.WebControls" namespace="Microsoft.Practices.Web.UI.WebControls" tagprefix="pp" %>
<asp:Content ID="content" ContentPlaceHolderID="DefaultContent" Runat="Server">
    <h1>Audittrail<pp:ObjectContainerDataSource ID="AudittrailDatasource" 
        runat="server" 
        DataObjectTypeName="Beheer.BusinessObjects.Dictionary.AuditItem" 
        oninserted="AudittrailDataSource_Inserted" />
      <asp:DetailsView ID="DetailsView1" runat="server" AutoGenerateRows="False" 
        DataSourceID="AudittrailDatasource" DefaultMode="Insert" Height="50px" 
        Width="125px" 
        DataKeyNames="Actie,Tabel,Veld,SleutelWaarde,WaardeVan,WaardeNaar,Id">
        <Fields>
          <asp:BoundField DataField="Actie" HeaderText="Actie" SortExpression="Actie" />
          <asp:BoundField DataField="Tabel" HeaderText="Tabel" SortExpression="Tabel" />
          <asp:BoundField DataField="Veld" HeaderText="Veld" SortExpression="Veld" />
          <asp:BoundField DataField="SleutelWaarde" HeaderText="SleutelWaarde" 
            SortExpression="SleutelWaarde" />
          <asp:BoundField DataField="WaardeVan" HeaderText="WaardeVan" 
            SortExpression="WaardeVan" />
          <asp:BoundField DataField="WaardeNaar" HeaderText="WaardeNaar" 
            SortExpression="WaardeNaar" />
          <asp:CommandField ShowInsertButton="True" />
        </Fields>
      </asp:DetailsView>
      <br />
      <asp:GridView ID="GridView1" runat="server" AllowPaging="True" 
        AllowSorting="True" AutoGenerateColumns="False" 
        DataSourceID="AudittrailDatasource">
        <Columns>
          <asp:BoundField DataField="DatumTijd" HeaderText="DatumTijd" 
            SortExpression="DatumTijd" />
          <asp:BoundField DataField="ActieNemer" HeaderText="ActieNemer" 
            SortExpression="ActieNemer" />
          <asp:BoundField DataField="Actie" HeaderText="Actie" SortExpression="Actie" />
          <asp:BoundField DataField="Tabel" HeaderText="Tabel" SortExpression="Tabel" />
          <asp:BoundField DataField="Veld" HeaderText="Veld" SortExpression="Veld" />
          <asp:BoundField DataField="SleutelWaarde" HeaderText="SleutelWaarde" 
            SortExpression="SleutelWaarde" />
          <asp:BoundField DataField="WaardeVan" HeaderText="WaardeVan" 
            SortExpression="WaardeVan" />
          <asp:BoundField DataField="WaardeNaar" HeaderText="WaardeNaar" 
            SortExpression="WaardeNaar" />
          <asp:BoundField DataField="Tablename" HeaderText="Tablename" 
            SortExpression="Tablename" />
          <asp:BoundField DataField="Id" HeaderText="Id" SortExpression="Id" 
            Visible="False" />
        </Columns>
      </asp:GridView>
    </h1>
		
		
</asp:Content>
