<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CRMTree.aspx.cs" Inherits="WebApplication1.CRMTree" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link href="Styles/RadControls.css" rel="stylesheet" type="text/css" />
    <title></title>
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
    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" >

        </telerik:RadAjaxLoadingPanel>


    <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server">

            <table >
            <tr>
                <td>        <telerik:RadTextBox ID="RadTextBox1" Runat="server" 
                        EnableSingleInputRendering="True" LabelWidth="64px" Text="zoekwoord" 
            >
        </telerik:RadTextBox ></td>
                <td>
                                    <telerik:RadButton ID="RadButton1" runat="server" Text="Zoek" style="top: 0px; left: 0px" 
            >
        </telerik:RadButton></td>
                <td>
                    &nbsp;</td>
            </tr>
            <tr>
                <td>
                    <telerik:RadGrid ID="RadGrid1" runat="server" AllowPaging="True" 
                        AllowSorting="True" CellSpacing="0" DataSourceID="ZoekSqlDataSource" 
                        GridLines="None" ondatabound="RadGrid1_DataBound" 
                        onitemdatabound="RadGrid1_ItemDataBound" 
                        onselectedindexchanged="RadGrid1_SelectedIndexChanged" Skin="Black" 
                        Width="215px">
                        <ClientSettings EnablePostBackOnRowClick="True">
                            <Selecting AllowRowSelect="True" CellSelectionMode="None" />
                        </ClientSettings>
                        <MasterTableView AllowSorting="False" AutoGenerateColumns="False" 
                            DataKeyNames="custname" DataSourceID="ZoekSqlDataSource">
                            <CommandItemSettings ExportToPdfText="Export to PDF" />
                            <RowIndicatorColumn FilterControlAltText="Filter RowIndicator column" 
                                Visible="True">
                            </RowIndicatorColumn>
                            <ExpandCollapseColumn FilterControlAltText="Filter ExpandColumn column" 
                                Visible="True">
                            </ExpandCollapseColumn>
                            <Columns>
                                <telerik:GridBoundColumn DataField="prio" 
                                    FilterControlAltText="Filter prio column" HeaderText="prio" 
                                    SortExpression="prio" UniqueName="prio">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="custname" 
                                    FilterControlAltText="Filter custname column" HeaderText="custname" 
                                    SortExpression="custname" UniqueName="custname">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="Voornaam" 
                                    FilterControlAltText="Filter Voornaam column" HeaderText="Voornaam" 
                                    SortExpression="Voornaam" UniqueName="Voornaam">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="tussenvoegsel" 
                                    FilterControlAltText="Filter tussenvoegsel column" HeaderText="tussenvoegsel" 
                                    SortExpression="tussenvoegsel" UniqueName="tussenvoegsel">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="AchterNaam" 
                                    FilterControlAltText="Filter AchterNaam column" HeaderText="AchterNaam" 
                                    SortExpression="AchterNaam" UniqueName="AchterNaam">
                                </telerik:GridBoundColumn>
                            </Columns>
                            <EditFormSettings>
                                <EditColumn FilterControlAltText="Filter EditCommandColumn column">
                                </EditColumn>
                            </EditFormSettings>
                        </MasterTableView>
                        <FilterMenu EnableImageSprites="False">
                        </FilterMenu>
                    </telerik:RadGrid>
                </td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
            </tr>
            <tr>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
            </tr>
        </table>


        <asp:SqlDataSource ID="ZoekSqlDataSource" runat="server" 
            ConnectionString="<%$ ConnectionStrings:ConnectionStringSygnionDB %>" 
            ProviderName="<%$ ConnectionStrings:ConnectionStringSygnionDB.ProviderName %>" 
            SelectCommand="Zoek" SelectCommandType="StoredProcedure">
            <SelectParameters>
                <asp:ControlParameter ControlID="RadTextBox1" Name="term" 
                    PropertyName="Text" Type="String" />
            </SelectParameters>
        </asp:SqlDataSource>


        <telerik:RadTreeView ID="RadTreeView1" Runat="server" Skin="Outlook" >
            
            <%--<Nodes>
                <telerik:RadTreeNode Text="dewd" ></telerik:RadTreeNode>
            </Nodes>--%>
        </telerik:RadTreeView>
        <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" 
            SelectMethod="GetSubsidiaries" TypeName="Repository.ShareHolders">
            <SelectParameters>
                <asp:ControlParameter ControlID="RadTextBox1" Name="shareHolder" 
                    PropertyName="Text" Type="String" />
            </SelectParameters>
        </asp:ObjectDataSource>
        <asp:LinqDataSource ID="LinqDataSource1" runat="server" EntityTypeName="">
        </asp:LinqDataSource>
    </telerik:RadAjaxPanel>

    </form>
</body>
</html>
