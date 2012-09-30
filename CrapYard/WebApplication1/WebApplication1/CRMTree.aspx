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
            <asp:ScriptReference Path="~/Scripts/Util.js" />
        </Scripts>
    </telerik:RadScriptManager>
    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" >

        </telerik:RadAjaxLoadingPanel>


    <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server">

            <table >
            <tr>
                <td>        
                    <telerik:RadTextBox ID="RadTextBox1" Runat="server" 
                        EnableSingleInputRendering="True" LabelWidth="64px" Text="zoekwoord" style="top: 0px; left: 0px" 
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
                        AllowSorting="True" CellSpacing="0" 
                        GridLines="None" ondatabound="RadGrid1_DataBound" 
                        onitemdatabound="RadGrid1_ItemDataBound" 
                        onselectedindexchanged="RadGrid1_SelectedIndexChanged" Skin="Black" 
                        Width="215px">
                        <ClientSettings EnablePostBackOnRowClick="True">
                            <Selecting AllowRowSelect="True" CellSelectionMode="None" />
                        </ClientSettings>
                        <MasterTableView AllowSorting="False" AutoGenerateColumns="False" 
                            DataKeyNames="custname">
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
<div style="background: url(Img/bg.gif) no-repeat; padding: 115px 0px 0px 15px;">
                <div style="width: 180px; float: left;">
                    <span class="label">Corporate Structure 1</span>
                    <telerik:RadTextBox ID="RadTextBox2" runat="server" EnableSingleInputRendering="True"
                        LabelWidth="64px" Text="zoekwoord" Style="top: 0px; left: 0px">
                    </telerik:RadTextBox>
                    <telerik:RadButton ID="RadButton2" runat="server" Text="Zoek" 
                        Style="top: 0px; left: 0px" onclick="RadButton1_Click">
                    </telerik:RadButton>
                    <telerik:RadTreeView ID="RadTreeView1" runat="server" EnableDragAndDrop="True" OnNodeDrop="RadTreeView1_HandleDrop"
                        OnClientNodeDropping="onNodeDropping" 
                        OnClientNodeDragging="onNodeDragging" MultipleSelect="true" 
                        EnableDragAndDropBetweenNodes="true" Skin="Outlook">
                    </telerik:RadTreeView>
                </div>
                <div style="width: 180px; float: left;">
                    <span class="label">Corporate Structure 2</span>
                    <telerik:RadTextBox ID="RadTextBox3" runat="server" EnableSingleInputRendering="True"
                        LabelWidth="64px" Text="zoekwoord" Style="top: 0px; left: 0px">
                    </telerik:RadTextBox>
                    <telerik:RadButton ID="RadButton3" runat="server" Text="Zoek" 
                        Style="top: 0px; left: 0px" onclick="RadButton2_Click">
                    </telerik:RadButton>
                    <telerik:RadTreeView ID="RadTreeView2" runat="server" EnableDragAndDrop="True" OnNodeDrop="RadTreeView1_HandleDrop"
                        OnClientNodeDropping="onNodeDropping" 
                        OnClientNodeDragging="onNodeDragging" MultipleSelect="true" 
                        EnableDragAndDropBetweenNodes="true" Skin="Outlook">
                    </telerik:RadTreeView>
                </div>

                <div style="clear: both">
                </div>
            </div>
<%--        <div style="background: url(Img/bg.gif) no-repeat; padding: 115px 0px 0px 15px;">
            <div style="width: 180px; float: left;">
                <span class="label">RadTreeView1</span>
                <telerik:RadTreeView ID="RadTreeView1" runat="server" EnableDragAndDrop="True" OnNodeDrop="RadTreeView1_HandleDrop"
                    OnClientNodeDropping="onNodeDropping" 
                    OnClientNodeDragging="onNodeDragging" MultipleSelect="true"
                    EnableDragAndDropBetweenNodes="true" Skin="Outlook">
                </telerik:RadTreeView>
            </div>
            <div style="width: 180px; float: left;">
                <span class="label">RadTreeView2</span>
                <telerik:RadTreeView ID="RadTreeView2" runat="server" EnableDragAndDrop="True" OnNodeDrop="RadTreeView1_HandleDrop"
                    OnClientNodeDropping="onNodeDropping" 
                    OnClientNodeDragging="onNodeDragging" MultipleSelect="true"
                    EnableDragAndDropBetweenNodes="true" Skin="Outlook">
                </telerik:RadTreeView>
            </div>
            <div style="width: 110px; float: left">
                <span class="label">TextBox</span>
                <asp:TextBox runat="server" ID="NodeText" Style="width: 82px;"></asp:TextBox>
            </div>
            <div style="float: left">
                <span class="label">RadGrid</span>
                <telerik:RadGrid runat="server" ID="RadGrid2" Width="220px">
                </telerik:RadGrid>
                <asp:Label CssClass="textr" runat="server" ID="Label1"></asp:Label>
            </div>
            <div style="clear: both">
            </div>
        </div>--%>

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

    <div id="msg">
    </div>
    </form>
</body>
</html>
