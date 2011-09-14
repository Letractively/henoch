<%@ Control Language="C#"  AutoEventWireup="true" CodeFile="Trefwoorden.ascx.cs" Inherits="MetaData.Beheer.Views.Trefwoorden" %>

<%@ Register assembly="Microsoft.Practices.Web.UI.WebControls" namespace="Microsoft.Practices.Web.UI.WebControls" tagprefix="pp" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<style type="text/css">
    .style1
    {
        width: 313px;
    }
    </style>

<table >
    <tr>
        <td class="style1">
            <asp:GridView ID="GridView1" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                DataSourceID="TrefwoordTableDataSource" DataKeyNames="Id,DataKeyValue" 
                Width="325px" BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" 
                BorderWidth="1px" CellPadding="3" onrowediting="GridView1_RowEditing" 
                onsorted="GridView1_Sorted">
                <RowStyle ForeColor="#000066" />
                <Columns>
                    <asp:TemplateField ShowHeader="False">
                        <EditItemTemplate>
                            <asp:ImageButton ID="UpdateImage" runat="server" CommandName="Update" 
                                ImageUrl="~/Images/save_32.png" ToolTip="Bewaar trefwoord." 
                                ValidationGroup="EditTrefwoord" />
                            <asp:ImageButton ID="CancelImage" runat="server" CausesValidation="False" 
                                CommandName="Cancel" ImageUrl="~/Images/stop_32.png" 
                                onclick="CancelImage_Click1" />
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:ImageButton ID="BtnEdit" runat="server" CommandName="Edit" 
                                ImageUrl="~/Images/pencil_32.png" ValidationGroup="EditTrefwoord" 
                                Visible="False" />
                            <asp:ImageButton ID="BtnDelete" runat="server" CommandName="Delete" 
                                ImageUrl="~/Images/delete_32.png" style="margin-left: 0px" 
                                ValidationGroup="Delete" Visible="False" />
                            <asp:Image ID="EditBlur" runat="server" ImageUrl="~/Images/pencil_32_blur.png" 
                                Visible="False" />
                            <asp:Image ID="DeleteBlur" runat="server" 
                                ImageUrl="~/Images/delete_32_blur.png" Visible="False" />
                        </ItemTemplate>
                        <ItemStyle Width="100px" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Trefwoord" SortExpression="DataKeyValue">
                        <EditItemTemplate>
                            <asp:TextBox ID="tbEdit" runat="server" Text='<%# Bind("DataKeyValue") %>' 
                                CausesValidation="True" ValidationGroup="EditTrefwoord" Height="22px" 
                                Width="146px"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RfvEdit" runat="server" Display="None" 
                                ErrorMessage="&lt;b&gt;Verplicht veld&lt;/b&gt;" 
                                ControlToValidate="tbEdit" ValidationGroup="EditTrefwoord"></asp:RequiredFieldValidator>
                            <cc1:ValidatorCalloutExtender ID="RfvEdit_ValidatorCalloutExtender" 
                                runat="server" TargetControlID="RfvEdit">
                            </cc1:ValidatorCalloutExtender>
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="Label1" runat="server" Text='<%# Bind("DataKeyValue") %>' 
                                Height="22px" Width="128px"></asp:Label>
                        </ItemTemplate>
                        <ItemStyle VerticalAlign="Top" Width="200px" HorizontalAlign="Left" />
                    </asp:TemplateField>
                </Columns>
                <FooterStyle BackColor="White" ForeColor="#000066" />
                <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
                <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
                <HeaderStyle BackColor="#006699" Font-Bold="True" ForeColor="White" />
            </asp:GridView>
                    <asp:DetailsView ID="DetailsView2" runat="server" 
    Height="50px" Width="325px" AutoGenerateRows="False" DataSourceID="TrefwoordTableDataSource" 
                        DefaultMode="Insert" style="margin-bottom: 0px" 
                DataKeyNames="Id,DataKeyValue" Visible="False">
                        <Fields>
                            <asp:TemplateField HeaderText="Invoer nieuw trefwoord:" 
                                SortExpression="DataKeyValue">
                                <EditItemTemplate>
                                    <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("DataKeyValue") %>'></asp:TextBox>
                                </EditItemTemplate>
                                <InsertItemTemplate>
                                    <asp:TextBox ID="tbInsertTrefwoord" runat="server" Text='<%# Bind("DataKeyValue") %>' 
                                        CausesValidation="True" ValidationGroup="InsertDetailTrefwoord"></asp:TextBox>
                                    
                                    <asp:RequiredFieldValidator ID="RfvInsert" runat="server" 
                                        ControlToValidate="tbInsertTrefwoord" Display="None" 
                                        ErrorMessage="&lt;b&gt;Verplicht veld &lt;/b&gt;" 
                                        ValidationGroup="InsertDetailTrefwoord"></asp:RequiredFieldValidator>
                                    <cc1:ValidatorCalloutExtender ID="RfvInsert_ValidatorCalloutExtender" 
                                        runat="server" Enabled="True" TargetControlID="RfvInsert">
                                    </cc1:ValidatorCalloutExtender>
                                    
                                </InsertItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="Label1" runat="server" Text='<%# Bind("DataKeyValue") %>' 
                                        Visible="False"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField ShowHeader="False">
                                <InsertItemTemplate>
                                    <asp:ImageButton ID="InsertImage" runat="server" CommandName="Insert" 
                                        ImageUrl="~/Images/save_32.png" ValidationGroup="InsertDetailTrefwoord" 
                                        ToolTip="Bewaar trefwoord." onclick="InsertImage_Click" />
                                    <asp:ImageButton ID="CancelImage" runat="server" CommandName="Cancel" 
                                        ImageUrl="~/Images/stop_32.png" ValidationGroup="Cancel" 
                                        onclick="CancelImage_Click" />
                                </InsertItemTemplate>
                                <ItemTemplate>
                                    <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" 
                                        CommandName="New" Text="New"></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Fields>
                    </asp:DetailsView>
        </td>
    </tr>
    <tr>
        <td class="style1" align="left">
            <asp:ImageButton ID="BtnInsertTrefwoord" runat="server" 
                ImageUrl="~/Images/plus_32.png" onclick="BtnInsertTrefwoord_Click" 
                ToolTip="Voeg een nieuwe trefwoord." Visible="False" />
            <asp:Image ID="InvisbleImage" runat="server" 
                ImageUrl="~/Images/invisible.png" />
            <pp:ObjectContainerDataSource ID="TrefwoordTableDataSource" runat="server" 
                
                DataObjectTypeName="Beheer.BusinessObjects.Dictionary.BeheerContextEntity" 
                ondeleted="TrefwoordTableDataSource_Deleted" 
                oninserted="TrefwoordTableDataSource_Inserted" 
                onupdated="TrefwoordTableDataSource_Updated" />
        </td>
    </tr>
    </table>


