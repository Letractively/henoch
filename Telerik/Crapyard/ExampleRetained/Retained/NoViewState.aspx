<%@ Page EnableViewState="true" Language="C#" AutoEventWireup="true" CodeBehind="NoViewState.aspx.cs" Inherits="Retained.NoViewState" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Untitled Page</title>
</head>
<body>
    <form id="form1" runat="server">
    <div> 
        <asp:ScriptManager ID="ScriptManager1" runat="server" />
        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="RadGrid1">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="RadGrid1" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
        </telerik:RadAjaxManager>
        <asp:Button ID="grdRebind" runat="server" Text="Rebind grid" OnClick="grdRebind_Click" />
        <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" Skin="Sunset" />
        <telerik:RadGrid ID="RadGrid1" DataSourceID="AccessDataSource1" runat="server" Skin="Sunset"
            Width="600px" AutoGenerateColumns="False" AllowSorting="True" AllowMultiRowSelection="True"
            GridLines="None" OnDataBound="RadGrid1_DataBound" EnableViewState="true"
            OnItemCommand="RadGrid1_ItemCommand">
            <PagerStyle Mode="NumericPages"></PagerStyle>
            <MasterTableView DataSourceID="AccessDataSource1" DataKeyNames="CustomerID" CommandItemDisplay="Top">
                <DetailTables>
                    <telerik:GridTableView DataKeyNames="OrderID" DataSourceID="AccessDataSource2" Width="100%"
                        runat="server" CommandItemDisplay="Top" PageSize="10">
                        <ParentTableRelation>
                            <telerik:GridRelationFields DetailKeyField="CustomerID" MasterKeyField="CustomerID" />
                        </ParentTableRelation>
                        <DetailTables>
                            <telerik:GridTableView DataKeyNames="OrderID" DataSourceID="AccessDataSource3" Width="100%"
                                runat="server" PageSize="10" CommandItemDisplay="Top">
                                <ParentTableRelation>
                                    <telerik:GridRelationFields DetailKeyField="OrderID" MasterKeyField="OrderID" />
                                </ParentTableRelation>

<CommandItemSettings ExportToPdfText="Export to Pdf"></CommandItemSettings>

<RowIndicatorColumn FilterControlAltText="Filter RowIndicator column"></RowIndicatorColumn>

<ExpandCollapseColumn FilterControlAltText="Filter ExpandColumn column"></ExpandCollapseColumn>
                                <Columns>
                                    <telerik:GridBoundColumn SortExpression="UnitPrice" HeaderText="Unit Price" HeaderButtonType="TextButton"
                                        DataField="UnitPrice" UniqueName="UnitPrice">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn SortExpression="Quantity" HeaderText="Quantity" HeaderButtonType="TextButton"
                                        DataField="Quantity" UniqueName="Quantity">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn SortExpression="Discount" HeaderText="Discount" HeaderButtonType="TextButton"
                                        DataField="Discount" UniqueName="Discount">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridButtonColumn UniqueName="OrderDetailsSelectColumn" CommandName="Select"
                                        Text="Select" />
                                    <telerik:GridButtonColumn UniqueName="OrderDetailsDeselectColumn" CommandName="Deselect"
                                        Text="Deselect" />
                                </Columns>

<EditFormSettings>
<EditColumn FilterControlAltText="Filter EditCommandColumn column"></EditColumn>
</EditFormSettings>
                            </telerik:GridTableView>
                        </DetailTables>

<CommandItemSettings ExportToPdfText="Export to Pdf"></CommandItemSettings>

<RowIndicatorColumn FilterControlAltText="Filter RowIndicator column"></RowIndicatorColumn>

<ExpandCollapseColumn Visible="True" FilterControlAltText="Filter ExpandColumn column"></ExpandCollapseColumn>
                        <Columns>
                            <telerik:GridBoundColumn SortExpression="OrderID" HeaderText="OrderID" HeaderButtonType="TextButton"
                                DataField="OrderID" UniqueName="OrderID">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn SortExpression="OrderDate" HeaderText="Date Ordered" HeaderButtonType="TextButton"
                                DataField="OrderDate" UniqueName="OrderDate">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn SortExpression="EmployeeID" HeaderText="EmployeeID" HeaderButtonType="TextButton"
                                DataField="EmployeeID" UniqueName="EmployeeID">
                            </telerik:GridBoundColumn>
                            <telerik:GridButtonColumn UniqueName="OrdersSelectColumn" CommandName="Select" Text="Select" />
                            <telerik:GridButtonColumn UniqueName="OrdersDeselectColumn" CommandName="Deselect"
                                Text="Deselect" />
                        </Columns>

<EditFormSettings>
<EditColumn FilterControlAltText="Filter EditCommandColumn column"></EditColumn>
</EditFormSettings>
                    </telerik:GridTableView>
                </DetailTables>

<CommandItemSettings ExportToPdfText="Export to Pdf"></CommandItemSettings>

<RowIndicatorColumn FilterControlAltText="Filter RowIndicator column"></RowIndicatorColumn>

<ExpandCollapseColumn Visible="True" FilterControlAltText="Filter ExpandColumn column"></ExpandCollapseColumn>
                <Columns>
                    <telerik:GridBoundColumn SortExpression="CustomerID" HeaderText="CustomerID"
                        DataField="CustomerID" UniqueName="CustomerID" ReadOnly="True">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn SortExpression="CompanyName" HeaderText="CompanyName"
                        DataField="CompanyName" UniqueName="CompanyName" 
                        FilterControlAltText="Filter CompanyName column">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn SortExpression="ContactName" HeaderText="ContactName"
                        DataField="ContactName" UniqueName="ContactName" 
                        FilterControlAltText="Filter ContactName column">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="ContactTitle" 
                        FilterControlAltText="Filter ContactTitle column" HeaderText="ContactTitle" 
                        SortExpression="ContactTitle" UniqueName="ContactTitle">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="Address" 
                        FilterControlAltText="Filter Address column" HeaderText="Address" 
                        SortExpression="Address" UniqueName="Address">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="City" 
                        FilterControlAltText="Filter City column" HeaderText="City" 
                        SortExpression="City" UniqueName="City">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="Region" 
                        FilterControlAltText="Filter Region column" HeaderText="Region" 
                        SortExpression="Region" UniqueName="Region">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="PostalCode" 
                        FilterControlAltText="Filter PostalCode column" HeaderText="PostalCode" 
                        SortExpression="PostalCode" UniqueName="PostalCode">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="Country" 
                        FilterControlAltText="Filter Country column" HeaderText="Country" 
                        SortExpression="Country" UniqueName="Country">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="Phone" 
                        FilterControlAltText="Filter Phone column" HeaderText="Phone" 
                        SortExpression="Phone" UniqueName="Phone">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="Fax" 
                        FilterControlAltText="Filter Fax column" HeaderText="Fax" SortExpression="Fax" 
                        UniqueName="Fax">
                    </telerik:GridBoundColumn>
                    <telerik:GridCheckBoxColumn DataField="Bool" DataType="System.Boolean" 
                        FilterControlAltText="Filter Bool column" HeaderText="Bool" 
                        SortExpression="Bool" UniqueName="Bool">
                    </telerik:GridCheckBoxColumn>
                    <telerik:GridBoundColumn DataField="NavigateUrlField" 
                        FilterControlAltText="Filter NavigateUrlField column" 
                        HeaderText="NavigateUrlField" SortExpression="NavigateUrlField" 
                        UniqueName="NavigateUrlField">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="ImageUrl" 
                        FilterControlAltText="Filter ImageUrl column" HeaderText="ImageUrl" 
                        SortExpression="ImageUrl" UniqueName="ImageUrl">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="AccessCode" 
                        FilterControlAltText="Filter AccessCode column" HeaderText="AccessCode" 
                        SortExpression="AccessCode" UniqueName="AccessCode">
                    </telerik:GridBoundColumn>
                </Columns>

<EditFormSettings>
<EditColumn FilterControlAltText="Filter EditCommandColumn column"></EditColumn>
</EditFormSettings>
            </MasterTableView>

<FilterMenu EnableImageSprites="False"></FilterMenu>

<HeaderContextMenu CssClass="GridContextMenu GridContextMenu_Sunset"></HeaderContextMenu>
        </telerik:RadGrid>
        <asp:AccessDataSource ID="AccessDataSource1" DataFile="~/App_Data/Nwind.mdb" SelectCommand="SELECT TOP 7 * FROM Customers"
            runat="server"></asp:AccessDataSource>
        <asp:AccessDataSource ID="AccessDataSource2" DataFile="~/App_Data/Nwind.mdb" SelectCommand="SELECT * FROM Orders Where CustomerID = ?"
            runat="server">
            <SelectParameters>
                <asp:SessionParameter Name="CustomerID" SessionField="CustomerID" Type="string" />
            </SelectParameters>
        </asp:AccessDataSource>
        <asp:AccessDataSource ID="AccessDataSource3" DataFile="~/App_Data/Nwind.mdb" SelectCommand="SELECT * FROM [Order Details] where OrderID = ?"
            runat="server">
            <SelectParameters>
                <asp:SessionParameter Name="OrderID" SessionField="OrderID" Type="Int32" />
            </SelectParameters>
        </asp:AccessDataSource>
    </div>
    </form>
</body>
</html>

