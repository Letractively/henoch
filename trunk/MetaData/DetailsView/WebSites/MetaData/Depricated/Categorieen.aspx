
<%@ Page Language="C#" Trace="true" AutoEventWireup="true" CodeFile="Categorieen.aspx.cs" Inherits="MetaData.Shared.Categorieen"
    Title="Categorieen" MasterPageFile="~/Shared/DefaultMaster.master" %>
<%@ Register assembly="Microsoft.Practices.Web.UI.WebControls" namespace="Microsoft.Practices.Web.UI.WebControls" tagprefix="pp" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>

<%@ Register src="../Beheer/Trefwoorden.ascx" tagname="Trefwoorden" tagprefix="uc1" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<asp:Content ID="content" ContentPlaceHolderID="DefaultContent" Runat="Server">
        <h1>Categorieen<table style="width:100%;">
            <tr>
                <td>
                    &nbsp;</td>
            </tr>
            <tr>
                <td>
          <table style="width:100%;">
            <tr>
              <td>
                  &nbsp;</td>
              <td>
                  &nbsp;</td>
            </tr>
            <tr>
              <td>

          <asp:DetailsView ID="DetailsView1" runat="server" AutoGenerateRows="False" 
            DataSourceID="CategorieTableDataSource" Font-Size="Medium" Height="35px" 
            Width="381px" DefaultMode="Insert" CellPadding="3" BackColor="White" BorderColor="#CCCCCC" 
                      BorderStyle="Outset" BorderWidth="1px" DataKeyNames="Id,DataKeyValue">
              <FooterStyle BackColor="White" ForeColor="#000066" />
              <RowStyle ForeColor="#000066" />
              <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
              <Fields>
                  <asp:TemplateField HeaderText="Invoer nieuwe Categorienaam" 
                      SortExpression="DataKeyValue">
                      <EditItemTemplate>
                          <asp:TextBox ID="tbEdit" runat="server" ValidationGroup="InsertDetail" 
                              Text='<%# Bind("DataKeyValue") %>' CausesValidation="True"></asp:TextBox>
                      </EditItemTemplate>
                      <InsertItemTemplate>
                          <asp:TextBox ID="tbInsertDetail" runat="server" CausesValidation="True" 
                              Text='<%# Bind("DataKeyValue") %>' ValidationGroup="InsertDetail"></asp:TextBox>
                          <asp:RequiredFieldValidator ID="RfvInsertDetail" runat="server" 
                              ControlToValidate="tbInsertDetail" Display="None" 
                              ErrorMessage="&lt;b&gt;Verplicht veld.&lt;/&gt;" ValidationGroup="InsertDetail"></asp:RequiredFieldValidator>
                          <asp:ValidatorCalloutExtender ID="RfvInsertDetail_ValidatorCalloutExtender" 
                              runat="server" Enabled="True" TargetControlID="RfvInsertDetail">
                          </asp:ValidatorCalloutExtender>
                      </InsertItemTemplate>
                      <ItemTemplate>
                          <asp:Label ID="Label1" runat="server" Text='<%# Bind("DataKeyValue") %>'></asp:Label>
                      </ItemTemplate>
                  </asp:TemplateField>
                  <asp:TemplateField ShowHeader="False">
                      <InsertItemTemplate>
                          &nbsp;<asp:ImageButton ID="InsertImage" runat="server" CommandName="Insert" 
                              ImageUrl="~/Images/save_32.png" ValidationGroup="InsertDetail" 
                              ToolTip="Categorienaam bewaren" />
                          <asp:ImageButton ID="CancelImage" runat="server" CommandName="Cancel" 
                              ImageUrl="~/Images/stop_32.png" ToolTip="Annuleren." />
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
              <td>

                  &nbsp;</td>
            </tr>
            <tr>
              <td>
                  <asp:ImageButton ID="MasterDetailView" runat="server" Height="22px" 
                                                                    
                      ImageUrl="~/Images/globe_32.png" onclick="OnMasterDetailView" 
                                                                    
                      ToolTip="Master-Detail Overzicht" Width="21px" />
                </td>
              <td>
                  &nbsp;</td>
            </tr>
            <tr>
              <td>

                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                        <ContentTemplate>

                                <ContentTemplate>
                                    <asp:GridView ID="GridView3" runat="server" AllowPaging="True" 
                                        AllowSorting="True" AutoGenerateColumns="False" BackColor="White" 
                                        BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" CellPadding="0" 
                                        DataSourceID="TrefwoordenTableDatasource" Font-Size="Small" 
                                        onselectedindexchanged="GridView3_SelectedIndexChanged" Width="750px" 
                                        DataKeyNames="Id,DataKeyValue,Master,MasterId">
                                        <RowStyle ForeColor="#000066" />
                                        <Columns>
                                            <asp:TemplateField ShowHeader="False">
                                                <EditItemTemplate>
                                                    <table style="width:105px;" cellspacing="0">
                                                        <tr>
                                                            <td style="width: 8px; ">
                                                                <asp:ImageButton ID="UpdateImage" runat="server" CommandName="Update" 
                                                                    ImageUrl="~/Images/save_32.png" ValidationGroup="UpdateCategorie" 
                                                                    ToolTip="Bewaar de wijziging" />
                                                            </td>
                                                            <td style="width: 32px">
                                                                <asp:ImageButton ID="ImageButton1" runat="server" CommandName="Cancel" 
                                                                    ImageUrl="~/Images/stop_32.png" ValidationGroup="Cancel" 
                                                                    ToolTip="Annuleer" />
                                                            </td>
                                                            <td>
                                                                <asp:Image ID="InvisibleEdit0" runat="server" ImageUrl="~/Images/invisible.png" 
                                                                    Visible="False" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </EditItemTemplate>
                                                <FooterTemplate>
                                                    <table cellspacing="0" style="width:105px;">
                                                        <tr>
                                                            <td style="width: 8px; ">
                                                                <asp:ImageButton ID="BtnSaveFooter" runat="server" CommandName="Insert" 
                                                                    ImageUrl="~/Images/save_32.png" onclick="BtnSaveFooter_Click" 
                                                                    ValidationGroup="InsertFooter" ToolTip="Bewaar de wijziging" />
                                                            </td>
                                                            <td style="width: 32px">
                                                                <asp:ImageButton ID="ImageButton1" runat="server" CommandName="Cancel" 
                                                                    ImageUrl="~/Images/stop_32.png" ValidationGroup="Cancel" 
                                                                    ToolTip="Annuleer" />
                                                            </td>
                                                            <td>
                                                                <asp:Image ID="InvisibleEdit0" runat="server" ImageUrl="~/Images/invisible.png" 
                                                                    Visible="False" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </FooterTemplate>
                                                <HeaderTemplate>
                                                    <asp:Image ID="InvisiblebreedImage0" runat="server" Height="1px" 
                                                                    ImageUrl="~/Images/invisible.png" Visible="true" Width="125px" />
                                                                
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <table style="width:14%;" cellspacing="0">
                                                        <tr valign="top">
                                                            <td style="width: 5px; ">
                                                                <asp:ImageButton ID="BtnEdit" runat="server" CommandName="Edit" 
                                                                    ImageUrl="~/Images/pencil_32.png" ToolTip="Bewerk categorie" 
                                                                    ValidationGroup="Edit" onclick="BtnEdit_Click" />
                                                            </td>
                                                            <td style="width: 304px">
                                                                <asp:ImageButton ID="BtnDelete" runat="server" CommandName="Delete" 
                                                                    ImageUrl="~/Images/trash_32.png" ToolTip="Verwijder catergorie" 
                                                                    ValidationGroup="Delete" />
                                                                <asp:ConfirmButtonExtender ID="BtnDelete_ConfirmButtonExtender" runat="server" 
                                                                    ConfirmText="Weet u zeker, dat u de categorie wilt verwijderen?" Enabled="True" 
                                                                    TargetControlID="BtnDelete">
                                                                </asp:ConfirmButtonExtender>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </ItemTemplate>
                                                <ItemStyle Width="200px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Categorienaam" SortExpression="Master">
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="tbCategorie" runat="server" CausesValidation="True" 
                                                        Text='<%# Bind("Master") %>' ValidationGroup="UpdateCategorie" Width="180px"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="RfvEditCategorie" runat="server" 
                                                        ControlToValidate="tbCategorie" Display="None" 
                                                        ErrorMessage="&lt;b&gt;Verplicht veld.&lt;/b&gt;" 
                                                        ValidationGroup="UpdateCategorie"></asp:RequiredFieldValidator>
                                                    <asp:ValidatorCalloutExtender ID="RfvEditCategorie_ValidatorCalloutExtender" 
                                                        runat="server" Enabled="True" TargetControlID="RfvEditCategorie">
                                                    </asp:ValidatorCalloutExtender>
                                                </EditItemTemplate>
                                                <FooterTemplate>
                                                    <asp:TextBox ID="tbCategorieFooter" runat="server" CausesValidation="True" 
                                                        Text='<%# Bind("Master") %>' ValidationGroup="InsertFooter" Width="180px"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="RfvEditCategorieFooter" runat="server" 
                                                        ControlToValidate="tbCategorieFooter" Display="None" 
                                                        ErrorMessage="&lt;b&gt;Verplicht veld.&lt;/b&gt;" 
                                                        ValidationGroup="InsertFooter"></asp:RequiredFieldValidator>
                                                    <asp:ValidatorCalloutExtender ID="RfvEditCategorieFooter_ValidatorCalloutExtender" 
                                                        runat="server" Enabled="True" TargetControlID="RfvEditCategorieFooter">
                                                    </asp:ValidatorCalloutExtender>
                                                    <asp:RequiredFieldValidator ID="RfvEditCategorieDuplicateFooter" runat="server" 
                                                        ControlToValidate="tbCategorieFooter0" Display="None" 
                                                        ErrorMessage="&lt;b&gt;Verplicht veld.&lt;/b&gt;" 
                                                        ValidationGroup="InsertFooter"></asp:RequiredFieldValidator>
                                                    <asp:ValidatorCalloutExtender ID="RfvEditCategorieDuplicateFooter_ValidatorCalloutExtender" 
                                                        runat="server" Enabled="True" TargetControlID="RfvEditCategorieDuplicateFooter">
                                                    </asp:ValidatorCalloutExtender>
                                                    <asp:TextBox ID="tbCategorieFooter0" runat="server" CausesValidation="True" 
                                                        Text='<%# Bind("Master") %>' ValidationGroup="InsertFooter" Visible="False" 
                                                        Width="180px"></asp:TextBox>
                                                </FooterTemplate>
                                                <HeaderTemplate>
                                                    <asp:LinkButton ID="LinkCategorie" runat="server" ForeColor="White" 
                                                        onclick="OnLinkSortCategorie">Categorienaam</asp:LinkButton>
                                                            <asp:Image ID="InvisiblebreedImage1" runat="server" Height="1px" 
                                                                    ImageUrl="~/Images/invisible.png" Visible="true" Width="300px" />                                                        
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="LblCategorienaam" runat="server" Height="26px" 
                                                        Text='<%# Bind("Master") %>' Width="200px"></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="200px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField ShowHeader="False">
                                                <EditItemTemplate>
                                                    <table style="width:10%;">
                                                        <tr>
                                                            <td style="width: 73px; ">
                                                                <asp:ImageButton ID="BtnUpdateDetail" runat="server" CommandName="Update" 
                                                                    ImageUrl="~/Images/save_32.png" ValidationGroup="UpdateCategorie" 
                                                                    ToolTip="Bewaar de wijziging" />
                                                                <asp:Image ID="InvisibleSave" runat="server" ImageUrl="~/Images/invisible.png" 
                                                                    Visible="False" />
                                                            </td>
                                                            <td>
                                                                <asp:ImageButton ID="BtnCancelDetail" runat="server" CommandName="Cancel" 
                                                                    ImageUrl="~/Images/stop_32.png" ValidationGroup="Cancel" 
                                                                    ToolTip="Annuleer" />
                                                                <asp:Image ID="InvisibleCancel" runat="server" 
                                                                    ImageUrl="~/Images/invisible.png" Visible="False" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </EditItemTemplate>
                                                <HeaderTemplate>
                                                    <asp:Image ID="InvisiblebreedImage2" runat="server" Height="1px" 
                                                                    ImageUrl="~/Images/invisible.png" Visible="true" Width="100px" />
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <table style="width:1%;">
                                                        <tr valign="top">
                                                            <td>
                                                                <asp:ImageButton ID="BtnInsertTrefwoord" runat="server" 
                                                                    ImageUrl="~/Images/plus_32.png" onclick="OnInsertTrefwoordItemTemplate" 
                                                                    ToolTip="Voeg een nieuwe trefwoord toe" Visible="False" 
                                                                    CommandName="Edit" />
                                                            </td>
                                                            <td style="width: 192px">
                                                                <asp:ImageButton ID="BtnEditDetail" runat="server" CommandName="Edit" 
                                                                    ImageUrl="~/Images/pencil_32.png" ToolTip="Bewerk trefwoord" 
                                                                    ValidationGroup="Edit" />
                                                                <asp:Image ID="InvisibleItemEdit" runat="server" 
                                                                    ImageUrl="~/Images/invisible.png" Visible="False" />
                                                            </td>
                                                            <td style="width: 2%;">
                                                                <asp:ImageButton ID="BtnDeleteDetail" runat="server" CommandName="Delete" 
                                                                    ImageUrl="~/Images/trash_32.png" onclick="BtnDelete_Click" 
                                                                    ToolTip="Verwijder trefwoord" ValidationGroup="Delete" />
                                                                <asp:ConfirmButtonExtender ID="BtnDeleteDetail_ConfirmButtonExtender" 
                                                                    runat="server" 
                                                                    ConfirmText="Weet u zeker, dat u het trefwoord wilt verwijderen?" 
                                                                    Enabled="True" TargetControlID="BtnDeleteDetail">
                                                                </asp:ConfirmButtonExtender>
                                                                <asp:Image ID="InvisibleItemDelete" runat="server" 
                                                                    ImageUrl="~/Images/invisible.png" Visible="False" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Trefwoord" SortExpression="DataKeyValue">
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="TrefwoordTextBox" runat="server" 
                                                        Text='<%# Bind("DataKeyValue") %>' Width="190px"></asp:TextBox>
                                                    <asp:Image ID="InvisibleTextBoxEdit" runat="server" Height="18px" 
                                                        ImageUrl="~/Images/invisible.png" Visible="False" Width="190px" />
                                                    <asp:RequiredFieldValidator ID="RfvEdit" runat="server" 
                                                        ControlToValidate="TrefwoordTextBox" Display="None" 
                                                        ErrorMessage="&lt;b&gt;Verplicht veld&lt;/b&gt;" 
                                                        ValidationGroup="EditTrefwoord"></asp:RequiredFieldValidator>
                                                    <cc1:ValidatorCalloutExtender ID="RfvEdit_ValidatorCalloutExtender" 
                                                        runat="server" TargetControlID="RfvEdit">
                                                    </cc1:ValidatorCalloutExtender>
                                                </EditItemTemplate>
                                                <FooterTemplate>
                                                    <asp:TextBox ID="tbTrefwoordInsertFooter" runat="server" 
                                                        Text='<%# Bind("DataKeyValue") %>' ValidationGroup="InsertFooter" Width="190px"></asp:TextBox>
                                                    <asp:Image ID="InvisibleTextBoxEditFooter" runat="server" Height="18px" 
                                                        ImageUrl="~/Images/invisible.png" Visible="False" Width="190px" />
                                                </FooterTemplate>
                                                <HeaderTemplate>
                                                    <asp:LinkButton ID="LinkSortTrefwoordAsc" runat="server" CommandArgument="asc" 
                                                        ForeColor="White" onclick="OnLinkSortTrefwoordAsc">Trefwoord</asp:LinkButton>
                                                        <asp:Image ID="InvisiblebreedImage3" runat="server" Height="1px" 
                                                                    ImageUrl="~/Images/invisible.png" Visible="true" Width="300px" />
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="LblTrefwoord" runat="server" Text='<%# Bind("DataKeyValue") %>' 
                                                        Width="125px"></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="358px" />
                                            </asp:TemplateField>
                                        </Columns>
                                        <FooterStyle BackColor="White" ForeColor="#000066" />
                                        <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
                                        <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
                                        <HeaderStyle BackColor="#006699" Font-Bold="True" ForeColor="White" />
                                    </asp:GridView>
                                    <asp:GridView ID="GridView2" runat="server" AllowPaging="True" 
                                        AllowSorting="True" AutoGenerateColumns="False" BackColor="White" 
                                        BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" CellPadding="0" 
                                        DataKeyNames="Tablename,Id,DataKeyValue,DataKeyName" 
                                        DataSourceID="CategorieTableDataSource" Font-Size="Medium" 
                                        onselectedindexchanged="GridView2_SelectedIndexChanged" 
                                        onsorting="GridView2_Sorting" Width="750px" Visible="False">
                                        <RowStyle ForeColor="#000066" />
                                        <Columns>
                                            <asp:TemplateField ShowHeader="False">
                                                <EditItemTemplate>
                                                    <table style="width:100%;">
                                                        <tr>
                                                            <td style="width: 8px; ">
                                                                <asp:ImageButton ID="UpdateImage" runat="server" CommandName="Update" 
                                                                    ImageUrl="~/Images/save_32.png" ValidationGroup="UpdateCategorie" />
                                                            </td>
                                                            <td>
                                                                <asp:ImageButton ID="ImageButton1" runat="server" CommandName="Cancel" 
                                                                    ImageUrl="~/Images/stop_32.png" ValidationGroup="Cancel" />
                                                            </td>
                                                            <td>
                                                                &nbsp;</td>
                                                        </tr>
                                                    </table>
                                                </EditItemTemplate>
                                                <HeaderTemplate>
                                                    <asp:ImageButton ID="DetailsView" runat="server" Height="22px" 
                                                        ImageUrl="~/Images/globe_32.png" onclick="OnDetailsView" 
                                                        ToolTip="Details Overzicht" Width="21px" />
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <table style="width:77%;">
                                                        <tr valign="top">
                                                            <td style="width: 20px; ">
                                                                <asp:ImageButton ID="BtnSelect" runat="server" CommandName="Select" 
                                                                    Height="34px" ImageUrl="~/Images/Button Next.png" onclick="BtnSelect_Click" 
                                                                    ToolTip="Selecteer rij." Width="38px" />
                                                            </td>
                                                            <td style="width: 5px; ">
                                                                <asp:ImageButton ID="BtnEdit" runat="server" CommandName="Edit" 
                                                                    ImageUrl="~/Images/pencil_32.png" ToolTip="Bewerk categorie" 
                                                                    ValidationGroup="Edit" Visible="False" />
                                                            </td>
                                                            <td>
                                                                <asp:ImageButton ID="BtnDelete" runat="server" CommandName="Delete" 
                                                                    ImageUrl="~/Images/trash_32.png" ToolTip="Verwijder catergorie" 
                                                                    ValidationGroup="Delete" Visible="False" />
                                                            </td>
                                                            <td style="width: 224px; ">
                                                                <asp:ImageButton ID="BtnCancel" runat="server" CommandName="Cancel" 
                                                                    ImageUrl="~/Images/stop_32.png" ToolTip="Annuleren" Visible="False" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </ItemTemplate>
                                                <ItemStyle VerticalAlign="Top" Width="200px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Categorienaam" SortExpression="DataKeyValue">
                                                <EditItemTemplate>
                                                    <table cellpadding="0" cellspacing="0" style="width:48%;">
                                                        <tr valign="top">
                                                            <td>
                                                                <asp:TextBox ID="tbCategorie" runat="server" CausesValidation="True" 
                                                                    Text='<%# Bind("DataKeyValue") %>' ValidationGroup="UpdateCategorie" 
                                                                    Width="180px"></asp:TextBox>
                                                                <asp:RequiredFieldValidator ID="RfvEditCategorie" runat="server" 
                                                                    ControlToValidate="tbCategorie" Display="None" 
                                                                    ErrorMessage="&lt;b&gt;Verplicht veld.&lt;/b&gt;" 
                                                                    ValidationGroup="UpdateCategorie"></asp:RequiredFieldValidator>
                                                                <asp:ValidatorCalloutExtender ID="RfvEditCategorie_ValidatorCalloutExtender" 
                                                                    runat="server" Enabled="True" TargetControlID="RfvEditCategorie">
                                                                </asp:ValidatorCalloutExtender>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </EditItemTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="LblCategorienaam" runat="server" Height="26px" 
                                                        Text='<%# Bind("DataKeyValue") %>' Width="200px"></asp:Label>
                                                    <asp:ImageButton ID="btnShowTrefwoorden" runat="server" CommandName="Select" 
                                                        Height="34px" ImageUrl="~/Images/folder_32.png" 
                                                        onclick="btnShowTrefwoorden_Click" oncommand="Select" 
                                                        ToolTip="Toon trefwoorden" Visible="False" Width="38px" />
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Left" />
                                                <ItemStyle VerticalAlign="Top" Width="200px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Trefwoorden" SortExpression="DataKeyValue">
                                                <HeaderTemplate>
                                                    <asp:LinkButton ID="LinkTrefwoord" runat="server" ForeColor="White" 
                                                        onclick="LinkTrefwoord_Click" Visible="False">Trefwoord</asp:LinkButton>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                                        <ContentTemplate>
                                                            <uc1:Trefwoorden ID="Trefwoorden1" runat="server" IsSortable="False" 
                                                                Visible="True" />
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Left" />
                                                <ItemStyle Width="350px" />
                                            </asp:TemplateField>
                                        </Columns>
                                        <FooterStyle BackColor="White" ForeColor="#000066" />
                                        <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
                                        <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
                                        <HeaderStyle BackColor="#006699" Font-Bold="True" ForeColor="White" />
                                    </asp:GridView>
                                </ContentTemplate>

                            <pp:ObjectContainerDataSource ID="CategorieTableDataSource" runat="server" 
                                DataObjectTypeName="Beheer.BusinessObjects.Dictionary.BeheerContextEntity" 
                                ondeleted="CategorieTableDataSource_Deleted" 
                                oninserted="CategorieTableDataSource_Inserted" 
                                onupdated="CategorieTableDataSource_Updated" 
                                    onupdating="CategorieTableDataSource_Updating" />
                            <pp:ObjectContainerDataSource ID="TrefwoordenTableDatasource" runat="server" 
                                DataObjectTypeName="Beheer.BusinessObjects.Dictionary.BeheerContextEntity" 
                                    ondeleted="TrefwoordenTableDatasource_Deleted" 
                                    oninserted="TrefwoordenTableDatasource_Inserted" 
                                    onupdated="TrefwoordenTableDatasource_Updated" 
                                    onupdating="TrefwoordenTableDatasource_Updating" 
                                    ondeleting="TrefwoordenTableDatasource_Deleting" 
                                    oninserting="TrefwoordenTableDatasource_Inserting" />
                                <asp:ImageButton ID="BtnInsertTrefwoord" runat="server" 
                                    ImageUrl="~/Images/plus_32.png" onclick="BtnInsertTrefwoord_Click" 
                                    ToolTip="Voeg een nieuwe categorie toe" CommandName="Cancel" />
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="GridView2" />
                            <asp:AsyncPostBackTrigger ControlID="GridView3" EventName="RowDataBound" />
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
              <td>
                            &nbsp;</td>
            </tr>
          </table>
                </td>
            </tr>
            <tr>
                <td>

                    &nbsp;</td>
            </tr>
            </table>
        </h1>
</asp:Content>
