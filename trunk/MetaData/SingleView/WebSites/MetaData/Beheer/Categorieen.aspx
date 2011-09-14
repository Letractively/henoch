
<%@ Page Language="C#" Trace="true" AutoEventWireup="true" CodeFile="Categorieen.aspx.cs" Inherits="MetaData.Beheer.Views.Categorieen"
    Title="Categorieen" MasterPageFile="~/Shared/DefaultMaster.master" %>
<%@ Register assembly="Microsoft.Practices.Web.UI.WebControls" namespace="Microsoft.Practices.Web.UI.WebControls" tagprefix="pp" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>

<%@ Register src="Trefwoorden.ascx" tagname="Trefwoorden" tagprefix="uc1" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<asp:Content ID="content" ContentPlaceHolderID="DefaultContent" Runat="Server">

        
        <h1>Categorieen               
        </h1>
        <table  id = "test" class="module" >
                      <tr>
                          <td>HELLO &nbsp;</td>
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
                      <tr>
                          <td>
                              &nbsp;</td>
                          <td>
                              &nbsp;</td>
                          <td>
                              &nbsp;</td>
                      </tr>
                  </table>    
    <asp:Table runat="server" ID="test2" CssClass="tableview">
        <asp:TableRow>
            <asp:TableCell>
                          HELLO &nbsp;
            </asp:TableCell>
            <asp:TableCell>
                          &nbsp;
            </asp:TableCell>
            <asp:TableCell>
                          &nbsp;
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell>
                          &nbsp;
            </asp:TableCell>
            <asp:TableCell>
                          &nbsp;
            </asp:TableCell>
            <asp:TableCell>
                          &nbsp;
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell>
                          &nbsp;
            </asp:TableCell>
            <asp:TableCell>
                          &nbsp;
            </asp:TableCell>
            <asp:TableCell>
                          &nbsp;
            </asp:TableCell>
        </asp:TableRow>
    </asp:Table>
    <table style="width: 100%;">
        <tr>
            <td>
                <table cellspacing="0" rules="all" border="1" id="gvFlowers" class="tableview" style="border-collapse: collapse;">
                    <thead>
                        <tr>
                            <th scope="col">
                                Name
                            </th>
                            <th scope="col">
                                Height
                            </th>
                            <th scope="col">
                                Width
                            </th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr>
                            <td>
                                tulip.jpg
                            </td>
                            <td>
                                30
                            </td>
                            <td>
                                420
                            </td>
                        </tr>
                        <tr>
                            <td>
                                daisy.jpg
                            </td>
                            <td>
                                32
                            </td>
                            <td>
                                481
                            </td>
                        </tr>
                        <tr>
                            <td>
                                rose.jpg
                            </td>
                            <td>
                                54
                            </td>
                            <td>
                                530
                            </td>
                        </tr>
                    </tbody>
                    <tfoot>
                    </tfoot>
                </table>
            </td>
            <td>
            </td>
        </tr>
        <tr>
            <td>
                <div id="ObservableByErrorHandler">
                </div>
                <div id="DivButton">
                    <input id="Button1" type="button" value="error" /><img  src="../Images/save_32.png" />
                    &nbsp;
                    <input id="RowId" class="visibility" name="RowId" />
                    &nbsp;
                    <input id="InlineInsert" class="visibility" name="InlineInsert" /><pp:ObjectContainerDataSource ID="TrefwoordenTableDatasource" runat="server" DataObjectTypeName="Beheer.BusinessObjects.Dictionary.BeheerContextEntity"
                        OnDeleted="TrefwoordenTableDatasource_Deleted" OnInserted="TrefwoordenTableDatasource_Inserted"
                        OnUpdated="TrefwoordenTableDatasource_Updated" OnUpdating="TrefwoordenTableDatasource_Updating"
                        OnDeleting="TrefwoordenTableDatasource_Deleting" OnInserting="TrefwoordenTableDatasource_Inserting" />
                    <asp:GridView ID="TrefwoordView" runat="server" AllowPaging="True" OnPreRender="OnPreRenderGridView"
                        AllowSorting="True" AutoGenerateColumns="False" class="gridview" DataSourceID="TrefwoordenTableDatasource"
                        Font-Size="Medium" OnRowEditing="TrefwoordView_RowEditing" DataKeyNames="Id,DataKeyValue,Master,MasterId"
                        OnSelectedIndexChanged="TrefwoordView_SelectedIndexChanged">
                        <RowStyle CssClass="record" />
                        <Columns>
                            <asp:TemplateField ShowHeader="False" Visible="False">
                                <ItemTemplate>
                                    <input id="Button2" type="button" value="Nieuw Trefwoord" class="rename in: insertbutton" />
                                    <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
                                    <asp:RegularExpressionValidator ID="RegexValidatorCoordY" runat="server" ControlToValidate="TextBox1"
                                        Display="None" ErrorMessage="&lt;b&gt;Alleen cijfers invullen.&lt;/b&gt;" ValidationExpression="^\d+|-\d+$"
                                        ValidationGroup="editMode"></asp:RegularExpressionValidator>
                                    <cc1:ValidatorCalloutExtender ID="RegexValidatorCoordY_ValidatorCalloutExtender"
                                        runat="server" Enabled="True" TargetControlID="RegexValidatorCoordY">
                                    </cc1:ValidatorCalloutExtender>
                                    <asp:RequiredFieldValidator ID="RequiredValidatorCoordY" runat="server" ControlToValidate="TextBox1"
                                        Display="None" ErrorMessage="<b>Dit veld is verplicht.</b>" ValidationGroup="editMode"></asp:RequiredFieldValidator>
                                    <cc1:ValidatorCalloutExtender ID="RequiredValidatorCoordY_ValidatorCalloutExtender"
                                        runat="server" Enabled="True" TargetControlID="RequiredValidatorCoordY">
                                    </cc1:ValidatorCalloutExtender>
                                    <asp:RangeValidator ID="RangeValidatorCoordYeditMode" runat="server" ControlToValidate="TextBox1"
                                        Display="None" ErrorMessage="<b>Ongeldig bereik.</b><br/>Voer een waarde tussen -10 en 650000 in."
                                        MaximumValue="650000" MinimumValue="-10" Type="Integer" ValidationGroup="editMode"></asp:RangeValidator>
                                    <cc1:ValidatorCalloutExtender ID="RangeValidatorCoordYeditMode_ValidatorCalloutExtender"
                                        runat="server" Enabled="True" TargetControlID="RangeValidatorCoordYeditMode">
                                    </cc1:ValidatorCalloutExtender>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField ShowHeader="False">
                                <EditItemTemplate>
                                    <table>
                                        <tr valign="top">
                                            <td>
                                                <asp:ImageButton ID="BtnEdit" runat="server" CommandName="Update" ImageUrl="~/Images/save_32.png"
                                                    ToolTip="Bewaar de wijziging" ValidationGroup="UpdateCategorie" />
                                            </td>
                                            <td valign="top">
                                                <asp:ImageButton ID="BtnCancelEdit" runat="server" CommandName="Cancel" ImageUrl="~/Images/stop_32.png"
                                                    OnClick="BtnCancelEdit_Click" ToolTip="Annuleer" ValidationGroup="Cancel" />
                                            </td>
                                        </tr>
                                    </table>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <table cellspacing="0" style="width: 105px;">
                                        <tr>
                                            <td style="width: 8px;">
                                                <asp:ImageButton ID="BtnSaveFooter" runat="server" CommandName="Insert" ImageUrl="~/Images/save_32.png"
                                                    OnClick="BtnSaveFooter_Click" ToolTip="Bewaar de wijziging" ValidationGroup="InsertFooter" />
                                            </td>
                                            <td style="width: 32px">
                                                <asp:ImageButton ID="ImageButton1" runat="server" CommandName="Cancel" ImageUrl="~/Images/stop_32.png"
                                                    ToolTip="Annuleer" ValidationGroup="Cancel" />
                                            </td>
                                            <td>
                                                <asp:Image ID="InvisibleEdit0" runat="server" ImageUrl="~/Images/invisible.png" Visible="False" />
                                            </td>
                                        </tr>
                                    </table>
                                </FooterTemplate>
                                <HeaderTemplate>
                                    <asp:Image ID="InvisiblebreedImage0" runat="server" Height="1px" ImageUrl="~/Images/invisible.png"
                                        Visible="true" Width="120px" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:ImageButton ID="BtnEdit" runat="server" CommandName="Edit" ImageUrl="~/Images/pencil_32.png"
                                        OnClick="OnBtnEdit" ToolTip="Bewerk categorie" ValidationGroup="Edit" CssClass="button"/>
                                    <asp:ImageButton ID="BtnDelete" runat="server" CommandName="Delete" ImageUrl="~/Images/trash_32.png"
                                        ToolTip="Verwijder catergorie" ValidationGroup="Delete" CssClass="button"/>
                                    <asp:ConfirmButtonExtender ID="BtnDelete_ConfirmButtonExtender" runat="server" ConfirmText="Weet u zeker, dat u de categorie wilt verwijderen?"
                                        Enabled="True" TargetControlID="BtnDelete">
                                    </asp:ConfirmButtonExtender>
                                    <asp:ImageButton ID="BtnInsert" runat="server" CommandName="Edit" ImageUrl="~/Images/plus_32.png"
                                        OnClick="OnInsert" ToolTip="Voeg een nieuwe categorie toe" Visible="False" CssClass="button"/>
                                </ItemTemplate>
                                <HeaderStyle Width="100px" />
                                <ItemStyle Width="100px" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Categorie" SortExpression="Master">
                                <EditItemTemplate>
                                    <asp:TextBox ID="tbCategorie" runat="server" Text='<%# Bind("Master") %>' CausesValidation="True"
                                        ValidationGroup="UpdateCategorie" Width="180px"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RfvEditCategorie" runat="server" ControlToValidate="tbCategorie"
                                        Display="None" ErrorMessage="&lt;b&gt;Verplicht veld.&lt;/b&gt;" ValidationGroup="UpdateCategorie"></asp:RequiredFieldValidator>
                                    <asp:ValidatorCalloutExtender ID="RfvEditCategorie_ValidatorCalloutExtender" runat="server"
                                        Enabled="True" TargetControlID="RfvEditCategorie">
                                    </asp:ValidatorCalloutExtender>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="tbCategorieFooter" runat="server" CausesValidation="True" Text='<%# Bind("Master") %>'
                                        ValidationGroup="InsertFooter" Width="180px"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RfvEditCategorieFooter" runat="server" ControlToValidate="tbCategorieFooter"
                                        Display="None" ErrorMessage="&lt;b&gt;Verplicht veld.&lt;/b&gt;" ValidationGroup="InsertFooter"></asp:RequiredFieldValidator>
                                    <asp:ValidatorCalloutExtender ID="RfvEditCategorieFooter_ValidatorCalloutExtender"
                                        runat="server" Enabled="True" TargetControlID="RfvEditCategorieFooter">
                                    </asp:ValidatorCalloutExtender>
                                    <asp:RequiredFieldValidator ID="RfvEditCategorieDuplicateFooter" runat="server" ControlToValidate="tbCategorieFooterDummy"
                                        Display="None" ErrorMessage="&lt;b&gt;Verplicht veld.&lt;/b&gt;" ValidationGroup="InsertFooter"></asp:RequiredFieldValidator>
                                    <asp:ValidatorCalloutExtender ID="RfvEditCategorieDuplicateFooter_ValidatorCalloutExtender"
                                        runat="server" Enabled="True" TargetControlID="RfvEditCategorieDuplicateFooter">
                                    </asp:ValidatorCalloutExtender>
                                    <asp:TextBox ID="tbCategorieFooterDummy" runat="server" CausesValidation="True" Text='<%# Bind("Master") %>'
                                        ValidationGroup="InsertFooter" Visible="False" Width="180px"></asp:TextBox>
                                </FooterTemplate>
                                <HeaderTemplate>
                                    <asp:LinkButton ID="LinkCategorie" runat="server" ForeColor="White" OnClick="OnLinkSortCategorie">Categorienaam</asp:LinkButton>
                                    <asp:Image ID="InvisiblebreedImage1" runat="server" Height="1px" ImageUrl="~/Images/invisible.png"
                                        Visible="true" Width="300px" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="LblCategorienaam" runat="server" Text='<%# Bind("Master") %>' Height="26px"
                                        Width="200px" CssClass="label"></asp:Label>
                                    
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField ShowHeader="False">
                                <EditItemTemplate>
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:ImageButton ID="BtnUpdateDetail2" runat="server" CommandName="Update" ImageUrl="~/Images/save_32.png"
                                                    OnClick="BtnUpdateDetail_Click" ToolTip="Bewaar de wijziging" ValidationGroup="InsertInlineTrefwoord"
                                                    Visible="False" />
                                                <asp:ImageButton ID="BtnUpdateDetail" runat="server" CommandName="Update" ImageUrl="~/Images/save_32.png"
                                                    ToolTip="Bewaar de wijziging" ValidationGroup="EditTrefwoord" OnClick="BtnUpdateDetail_Click" />
                                            </td>
                                            <td valign="top">
                                                <asp:ImageButton ID="BtnCancelDetail" runat="server" CommandName="Cancel" ImageUrl="~/Images/stop_32.png"
                                                    ToolTip="Annuleer" ValidationGroup="Cancel" OnClick="BtnCancelDetail_Click" />
                                            </td>
                                        </tr>
                                    </table>
                                    
                                </EditItemTemplate>
                                <HeaderTemplate>
                                    <asp:Image ID="InvisiblebreedImage2" runat="server" Height="1px" ImageUrl="~/Images/invisible.png"
                                        Visible="true" Width="120px" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <table>
                                        <tr valign="top">
                                            <td>
                                                <%--<asp:ImageButton ID="BtnInsertTrefwoord" runat="server" CommandName="Edit" ImageUrl="~/Images/plus_32.png"
                                                    ToolTip="Voeg een nieuwe trefwoord toe" OnClick="OnPrepareInlineInsert" CssClass="insertbutton"/>
                                                <asp:ImageButton ID="BtnInsertInlineTrefwoord" runat="server" CommandName="Edit"
                                                    ImageUrl="~/Images/plus_32.png" OnClick="OnInsertTrefwoordItemTemplate" Visible="false"
                                                    ToolTip="Voeg een nieuwe trefwoord toe" CssClass="insertbutton" />--%>
                                                <asp:Image id="Inlineinsert"  alt="" src="../Images/plus_32.png" class="insertbutton" runat="server"/>  
                                                                                                
                                            </td>
                                            <td>
                                                <asp:ImageButton ID="BtnEditDetail" runat="server" CommandName="Edit" ImageUrl="~/Images/pencil_32.png"
                                                    ToolTip="Bewerk trefwoord" ValidationGroup="Edit" OnClick="OnBtnEditDetail" CssClass="button"/>
                                            </td>
                                            <td>
                                                <asp:ImageButton ID="BtnDeleteDetail" runat="server" CommandName="Delete" ImageUrl="~/Images/trash_32.png"
                                                    OnClick="BtnDelete_Click" ToolTip="Verwijder trefwoord" ValidationGroup="Delete" CssClass="button"/>
                                                <asp:ConfirmButtonExtender ID="BtnDeleteDetail_ConfirmButtonExtender" runat="server"
                                                    ConfirmText="Weet u zeker, dat u het trefwoord wilt verwijderen?" Enabled="True"
                                                    TargetControlID="BtnDeleteDetail">
                                                </asp:ConfirmButtonExtender>
                                            </td>
                                        </tr>
                                    </table>
                                    
                                </ItemTemplate>
                                <ItemStyle VerticalAlign="Top" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Trefwoord" SortExpression="DataKeyValue">
                                <EditItemTemplate>
                                    <table style="height: 2px;" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td>
                                                <asp:TextBox ID="TrefwoordTextBox" runat="server" Text='<%# Bind("DataKeyValue") %>'
                                                    ValidationGroup="EditTrefwoord" Width="190px"></asp:TextBox>
                                                <asp:Label ID="LblTrefwoordInline" runat="server" Width="125px" Visible="false"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:RequiredFieldValidator ID="RfvEdit" runat="server" ControlToValidate="TrefwoordTextBox"
                                                    Display="None" ErrorMessage="&lt;b&gt;Verplicht veld&lt;/b&gt;" ValidationGroup="EditTrefwoord"></asp:RequiredFieldValidator>
                                                <cc1:ValidatorCalloutExtender ID="RfvEdit_ValidatorCalloutExtender" runat="server"
                                                    TargetControlID="RfvEdit">
                                                </cc1:ValidatorCalloutExtender>
                                            </td>
                                            <td>
                                                <asp:UpdateProgress ID="UpdateProgressInsertInline" runat="server" AssociatedUpdatePanelID="UpdatePanel2"
                                                    DisplayAfter="100">
                                                    <ProgressTemplate>
                                                        <asp:Image ID="Image1" runat="server" ImageUrl="~/Images/rotating_arrow.gif" />
                                                    </ProgressTemplate>
                                                </asp:UpdateProgress>
                                                <asp:Image ID="InvisibleInsertInline" runat="server" Height="38px" ImageUrl="~/Images/invisible.png"
                                                    Visible="False" Width="1px" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td valign="bottom">
                                                <asp:TextBox ID="TrefwoordTextBoxInline" runat="server" CausesValidation="True" Height="22px"
                                                    ValidationGroup="InsertInlineTrefwoord" Visible="False" Width="190px" />
                                            </td>
                                            <td>
                                                <asp:RequiredFieldValidator ID="RfvInsertInline" runat="server" ControlToValidate="TrefwoordTextBoxInline"
                                                    Display="None" ErrorMessage="&lt;b&gt;Verplicht veld&lt;/b&gt;" ValidationGroup="InsertInlineTrefwoord"></asp:RequiredFieldValidator>
                                                <cc1:ValidatorCalloutExtender ID="RfvInsertInline_ValidatorCalloutExtender" runat="server"
                                                    TargetControlID="RfvInsertInline">
                                                </cc1:ValidatorCalloutExtender>
                                            </td>
                                            <td>
                                                <asp:Image ID="InvisibleInsertInline2" runat="server" Height="20px" ImageUrl="~/Images/invisible.png"
                                                    Visible="False" Width="1px" />
                                            </td>
                                        </tr>
                                    </table>
                                </EditItemTemplate>
                                <HeaderTemplate>
                                    <asp:LinkButton ID="LinkSortTrefwoordAsc" runat="server" CommandArgument="asc" ForeColor="White"
                                        OnClick="OnLinkSortTrefwoordAsc">Trefwoord</asp:LinkButton>
                                    <asp:Image ID="InvisiblebreedImage3" runat="server" Height="1px" ImageUrl="~/Images/invisible.png"
                                        Visible="true" Width="300px" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="LblTrefwoord" runat="server" Text='<%# Bind("DataKeyValue") %>' Width="125px" CssClass="label"></asp:Label>
                                    &nbsp;
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        
                    </asp:GridView>
                </div>
            </td>
            <td>
                &nbsp;
            </td>
        </tr>
        <tr>
            <td style="width: 40px">
                <asp:ImageButton ID="BtnInsertCategorie" runat="server" CommandName="Cancel" ImageUrl="~/Images/plus_32.png"
                    OnClick="BtnInsertCategorie_Click" ToolTip="Voeg een nieuwe categorie toe" />
                <asp:UpdateProgress ID="UpdateProgressView" runat="server" AssociatedUpdatePanelID="UpdatePanel2"
                    DisplayAfter="100">
                    <ProgressTemplate>
                        <asp:Image ID="ImgBigRotation" runat="server" ImageUrl="~/Images/bigrotation2.gif" />
                    </ProgressTemplate>
                </asp:UpdateProgress>
            </td>
            <td>
                <asp:Image ID="InvisibleProgress" runat="server" ImageUrl="~/Images/invisible.png" />
            </td>
        </tr>
        <tr>
            <td style="width: 40px">
                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                </asp:UpdatePanel>
            </td>
            <td>
                &nbsp;
            </td>
        </tr>
    </table>
</asp:Content>
