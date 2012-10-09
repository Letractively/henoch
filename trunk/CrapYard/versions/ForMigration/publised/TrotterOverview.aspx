<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TrotterOverview.aspx.cs"
    Inherits="TrotterOverview" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        .MyImageButton
        {
            cursor: hand;
        }
        .EditFormHeader td
        {
            font-size: 14px;
            padding: 4px !important;
            color: #0066cc;
        }
    </style>
    <telerik:RadStyleSheetManager ID="RadStyleSheetManager1" runat="server" />
</head>
<body>
    <form id="form1" runat="server">
    <telerik:RadScriptManager ID="RadScriptManager1" runat="server">
        <Scripts>
            <%--Needed for JavaScript IntelliSense in VS2010--%>
            <%--For VS2008 replace RadScriptManager with ScriptManager--%>
            <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.Core.js" />
            <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQuery.js" />
            <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQueryInclude.js" />
        </Scripts>
    </telerik:RadScriptManager>
    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" />
    <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server">
        <telerik:RadGrid ID="RadGrid1" GridLines="None" runat="server" AllowAutomaticDeletes="True"
            AllowSorting="True" AllowAutomaticInserts="True" 
            AllowAutomaticUpdates="True" AllowPaging="True" DataSourceID="DataSource1" 
            OnItemUpdated="RadGrid1_ItemUpdated" OnItemDeleted="RadGrid1_ItemDeleted" OnItemInserted="RadGrid1_ItemInserted"
            OnDataBound="RadGrid1_DataBound" CellSpacing="0" Skin="Black" 
            ondetailtabledatabind="RadGrid1_DetailTableDataBind" 
            onneeddatasource="RadGrid1_NeedDataSource" AutoGenerateEditColumn="True">
            <ClientSettings>
                <Selecting CellSelectionMode="None" />
            </ClientSettings>
            <MasterTableView AutoGenerateColumns="False" CommandItemDisplay="TopAndBottom" 
                DataKeyNames="pjtcd,ordNo" DataSourceID="DataSource1" EditMode="EditForms" 
                HorizontalAlign="NotSet" Width="100%">
                <DetailTables>
                    <telerik:GridTableView runat="server" >
                        <CommandItemSettings ExportToPdfText="Export to PDF" />
                        <RowIndicatorColumn FilterControlAltText="Filter RowIndicator column" 
                            Visible="True">
                        </RowIndicatorColumn>
                        <ExpandCollapseColumn FilterControlAltText="Filter ExpandColumn column" 
                            Visible="True">
                        </ExpandCollapseColumn>
                        <EditFormSettings>
                            <EditColumn FilterControlAltText="Filter EditCommandColumn column">
                            </EditColumn>
                        </EditFormSettings>
                    </telerik:GridTableView>
                </DetailTables>
                <CommandItemSettings ExportToPdfText="Export to PDF" />
                <RowIndicatorColumn FilterControlAltText="Filter RowIndicator column" 
                    Visible="True">
                </RowIndicatorColumn>
                <ExpandCollapseColumn FilterControlAltText="Filter ExpandColumn column" 
                    Visible="True">
                </ExpandCollapseColumn>
                <Columns>
                    <telerik:GridBoundColumn DataField="pjtcd" 
                        FilterControlAltText="Filter pjtcd column" HeaderText="pjtcd" ReadOnly="True" 
                        SortExpression="pjtcd" UniqueName="pjtcd">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="ordNo" 
                        FilterControlAltText="Filter ordNo column" HeaderText="ordNo" 
                        SortExpression="ordNo" UniqueName="ordNo">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="ordLn" DataType="System.Int32" 
                        FilterControlAltText="Filter ordLn column" HeaderText="ordLn" 
                        SortExpression="ordLn" UniqueName="ordLn">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="dateFr" DataType="System.DateTime" 
                        FilterControlAltText="Filter dateFr column" HeaderText="dateFr" 
                        SortExpression="dateFr" UniqueName="dateFr">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="dateTo" DataType="System.DateTime" 
                        FilterControlAltText="Filter dateTo column" HeaderText="dateTo" 
                        SortExpression="dateTo" UniqueName="dateTo">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="cust" 
                        FilterControlAltText="Filter cust column" HeaderText="cust" 
                        SortExpression="cust" UniqueName="cust">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="dte" DataType="System.DateTime" 
                        FilterControlAltText="Filter dte column" HeaderText="dte" SortExpression="dte" 
                        UniqueName="dte">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="st" FilterControlAltText="Filter st column" 
                        HeaderText="st" SortExpression="st" UniqueName="st">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="uid" 
                        FilterControlAltText="Filter uid column" HeaderText="uid" SortExpression="uid" 
                        UniqueName="uid">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="recdate" DataType="System.DateTime" 
                        FilterControlAltText="Filter recdate column" HeaderText="recdate" 
                        SortExpression="recdate" UniqueName="recdate">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="owner" 
                        FilterControlAltText="Filter owner column" HeaderText="owner" 
                        SortExpression="owner" UniqueName="owner">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="lockedby" 
                        FilterControlAltText="Filter lockedby column" HeaderText="lockedby" 
                        SortExpression="lockedby" UniqueName="lockedby">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="locktime" DataType="System.DateTime" 
                        FilterControlAltText="Filter locktime column" HeaderText="locktime" 
                        SortExpression="locktime" UniqueName="locktime">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="onmachine" 
                        FilterControlAltText="Filter onmachine column" HeaderText="onmachine" 
                        SortExpression="onmachine" UniqueName="onmachine">
                    </telerik:GridBoundColumn>
                </Columns>
                <EditFormSettings>
                    <FormTableItemStyle Wrap="False" />
                    <FormCaptionStyle CssClass="EditFormHeader" />
                    <FormMainTableStyle BackColor="White" CellPadding="3" CellSpacing="0" 
                        GridLines="None" Width="100%" />
                    <FormTableStyle BackColor="White" CellPadding="2" CellSpacing="0" 
                        Height="110px" />
                    <FormTableAlternatingItemStyle Wrap="False" />
                    <EditColumn ButtonType="ImageButton" CancelText="Cancel edit" 
                        UniqueName="EditCommandColumn1">
                    </EditColumn>
                    <FormTableButtonRowStyle CssClass="EditFormButtonRow" HorizontalAlign="Right" />
                </EditFormSettings>
            </MasterTableView>
            <FilterMenu EnableImageSprites="False">
            </FilterMenu>
        </telerik:RadGrid>
        <telerik:RadWindowManager ID="RadWindowManager1" runat="server">
        </telerik:RadWindowManager>
    </telerik:RadAjaxPanel>
    <asp:SqlDataSource SelectCommand="SELECT DISTINCT proj.pjtcd, proj.ordNo, proj.ordLn, proj.dateFr, proj.dateTo, proj.cust, proj.dte, proj.st, proj.uid, proj.recdate, proj.owner, proj.lockedby, proj.locktime, proj.onmachine FROM troProject AS proj INNER JOIN (SELECT pjtcd FROM troProject_Act WHERE (st = 'D') AND (DATEDIFF(day, datefr, GETDATE()) > 30) AND (NOT (DATEDIFF(day, dateto, GETDATE()) > 0))) AS pact ON proj.pjtcd = pact.pjtcd ORDER BY proj.dateFr DESC"
        ConnectionString="<%$ ConnectionStrings:sygnionTro %>" ProviderName="System.Data.OleDb"
        ID="DataSource1" runat="server"></asp:SqlDataSource>
    <asp:SqlDataSource ID="SqlDataSource1" runat="server" 
        ConnectionString="<%$ ConnectionStrings:sygnionTro %>" 
        ProviderName="<%$ ConnectionStrings:sygnionTro.ProviderName %>" 
        SelectCommand="GetCandidatesTrotters" SelectCommandType="StoredProcedure">
        <SelectParameters>
            <asp:SessionParameter DefaultValue="" Name="orderNo" SessionField="OrdNo" 
                Type="String" />
        </SelectParameters>
    </asp:SqlDataSource>    
    <telerik:RadSkinManager ID="RadSkinManager1" runat="server" Skin="Black">
    </telerik:RadSkinManager>
    </form>
</body>
</html>
